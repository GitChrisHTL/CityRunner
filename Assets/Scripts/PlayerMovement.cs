using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;

    [Header("Running Animation")]
    [SerializeField] private Sprite[] runSprites;  // Array der Laufanimation-Sprites (Player_Run1 bis Player_Run5)
    [SerializeField] private float frameTime = 0.1f; // Zeit zwischen Frames
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float frameTimer;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;

    private void Start()
    {
        // SpriteRenderer der GFX-Komponente holen
        spriteRenderer = GFX.GetComponent<SpriteRenderer>();
        frameTimer = frameTime; // Timer initialisieren

        if (runSprites == null || runSprites.Length == 0)
        {
            Debug.LogError("Run Sprites wurden nicht im Inspektor gesetzt!");
        }
    }

    private void Update()
    {
        // Prüfen, ob der Spieler auf dem Boden ist
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        #region Jumping 
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimer < jumpTime)
            {
                rb.linearVelocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0;
        }
        #endregion

        #region Sliding
        if (isGrounded && Input.GetButton("Slide"))
        {
            // Sliding-Logik hinzufügen
        }
        #endregion

        #region Running Animation
        if (isGrounded && !isJumping)
        {
            UpdateRunAnimation();
        }
        #endregion
    }

    private void UpdateRunAnimation()
    {
        if (runSprites == null || runSprites.Length == 0) return; // Schutz vor leeren Arrays

        // Timer für die Animation
        frameTimer -= Time.deltaTime;

        if (frameTimer <= 0f)
        {
            // Zum nächsten Frame wechseln
            currentFrame = (currentFrame + 1) % runSprites.Length; // Loop durch das Array
            spriteRenderer.sprite = runSprites[currentFrame];

            // Timer zurücksetzen
            frameTimer = frameTime;
        }
    }
}
