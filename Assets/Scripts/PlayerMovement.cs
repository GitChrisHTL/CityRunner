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
    [SerializeField] private Sprite[] jumpSprites;  // Array der Jumpanimation-Sprites (Player_Jump1 bis Player_Jump5)
    [SerializeField] private Sprite[] fallSprites;  // Array der Fallanimation-Sprites (Player_Fall1 bis Player_Fall3)
    [SerializeField] private float frameTime = 0.1f; // Zeit zwischen Frames
    [SerializeField] private GameObject player;
    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float frameTimer;

    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isSliding = false;
    private float jumpTimer;

    private void Start()
    {
        // SpriteRenderer der GFX-Komponente holen
        if (GFX != null)
        {
            spriteRenderer = player.GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer konnte nicht gefunden werden! Überprüfe, ob GFX eine Komponente mit SpriteRenderer ist.");
        }

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
                UpdateFallAnimation();
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0;
        }
        #endregion

        #region Sliding
        if (isGrounded && Input.GetButtonDown("Slide"))
        {
            isSliding = true;
        }

        if (Input.GetButtonDown("Slide"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 0.3f, GFX.localScale.z);
        }

        if (Input.GetButtonUp("Slide"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
        #endregion

        #region Running Animation
        if (isGrounded && !isJumping)
        {
            UpdateRunAnimation();
        }
        #endregion

        #region Jumping Animation
        if (!isGrounded && isJumping)
        {
            UpdateJumpAnimation();
        }
        #endregion

        #region Falling Animation
        if (!isGrounded && !isJumping)
        {
            UpdateFallAnimation();
        }
        #endregion
    }

    private void UpdateRunAnimation()
    {
        if (runSprites == null || runSprites.Length == 0) return; // Schutz vor leeren Arrays
        if (spriteRenderer == null) return; // Schutz vor fehlendem SpriteRenderer

        // Timer für die Animation
        frameTimer -= Time.deltaTime;

        if (frameTimer <= 0f)
        {
            // Zum nächsten Frame wechseln
            currentFrame = (currentFrame + 1) % runSprites.Length; // Loop durch das Array
            spriteRenderer.sprite = runSprites[currentFrame];

            Debug.Log($"Sprite gewechselt zu: {runSprites[currentFrame].name}");

            // Timer zurücksetzen
            frameTimer = frameTime;
        }
    }

    private void UpdateJumpAnimation()
{
    if (jumpSprites == null || jumpSprites.Length == 0) return;
    if (spriteRenderer == null) return;

    // Wenn die Animation noch nicht abgeschlossen ist
    if (currentFrame < jumpSprites.Length - 1)
    {
        frameTimer -= Time.deltaTime;

        // Wenn der Timer abgelaufen ist, zum nächsten Frame wechseln
        if (frameTimer <= 0f)
        {
            currentFrame = (currentFrame + 1) % jumpSprites.Length;
            spriteRenderer.sprite = jumpSprites[currentFrame];
            frameTimer = frameTime;  // Timer zurücksetzen
        }
    }
    else
    {
        // Die Sprunganimation ist abgeschlossen, wir bleiben im letzten Frame
        spriteRenderer.sprite = jumpSprites[jumpSprites.Length - 1];
    }
}


    private void UpdateFallAnimation()
    {
        if (fallSprites == null || fallSprites.Length == 0) return;
        if (spriteRenderer == null) return;

        frameTimer -= Time.deltaTime;

        if (frameTimer <= 0f)
        {
            currentFrame = (currentFrame + 1) % fallSprites.Length;
            spriteRenderer.sprite = fallSprites[currentFrame];
            frameTimer = frameTime;
        }
    }
}
