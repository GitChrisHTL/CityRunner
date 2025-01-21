using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private Sprite[] runSprites, jumpSprites, fallSprites;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float jumpForce = 10f, groundDistance = 0.25f, frameTime = 0.1f, jumpTime = 0.3f;
    [SerializeField] private GameObject player;

    private SpriteRenderer spriteRenderer;
    private bool isGrounded, isJumping;
    private int currentFrame = 0;
    private float frameTimer, jumpTimer;
    private Sprite[] currentAnimationSprites;
    private bool jumpAnimationComplete = false;

    private void Start()
    {
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer konnte nicht gefunden werden! Stelle sicher, dass GFX einen SpriteRenderer besitzt.");
        }

        frameTimer = frameTime;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
        HandleJump();
        HandleSlide();
        UpdateAnimations();
    }

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimer = 0;
            rb.linearVelocity = Vector2.up * jumpForce;
            ResetJumpAnimation();
        }

        if (isJumping && Input.GetButton("Jump") && jumpTimer < jumpTime)
        {
            rb.linearVelocity = Vector2.up * jumpForce;
            jumpTimer += Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump") || jumpTimer >= jumpTime)
        {
            isJumping = false;
        }
    }

    private void HandleSlide()
    {
        if (Input.GetButtonDown("Slide"))
        {
            //isSliding = true;
            GFX.localScale = new Vector3(GFX.localScale.x, 0.3f, GFX.localScale.z);
        }

        if (Input.GetButtonUp("Slide"))
        {
            //isSliding = false;
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
    }

    private void UpdateAnimations()
    {
        if (isGrounded && !isJumping)
        {
            SwitchAnimation(runSprites);
        }
        else if (isJumping)
        {
            if (currentAnimationSprites != jumpSprites)
            {
                SwitchAnimation(jumpSprites);
            }
        }
        else if (!isGrounded && !isJumping)
        {
            SwitchAnimation(fallSprites);
        }

        UpdateAnimation(currentAnimationSprites);
    }

    private void SwitchAnimation(Sprite[] newAnimation)
    {
        if (currentAnimationSprites != newAnimation)
        {
            currentAnimationSprites = newAnimation;
            currentFrame = 0;
            frameTimer = frameTime;
            spriteRenderer.sprite = currentAnimationSprites[currentFrame];
        }
    }

    private void UpdateAnimation(Sprite[] sprites)
    {
        if (sprites == null || sprites.Length == 0 || spriteRenderer == null)
        {
            Debug.LogError("Animation konnte nicht aktualisiert werden: sprites ist null oder leer.");
            return;
        }

        frameTimer -= Time.deltaTime;

        if (frameTimer <= 0f)
        {
            if (sprites == jumpSprites)
            {
                if (jumpAnimationComplete)
                {
                    return;
                }

                if (currentFrame >= 4) // Bis Bild 5 stoppen (Index 4)
                {
                    spriteRenderer.sprite = sprites[4];
                    jumpAnimationComplete = true;
                    return;
                }
            }

            if (currentFrame >= sprites.Length)
            {
                currentFrame = 0;
            }

            spriteRenderer.sprite = sprites[currentFrame];
            currentFrame++;
            frameTimer = frameTime;
        }
    }

    private void ResetJumpAnimation()
    {
        jumpAnimationComplete = false;
        currentFrame = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            StopGame();
        }
    }

    private void StopGame()
    {
        spriteRenderer.sprite = null;
        this.enabled = false;
        Time.timeScale = 0f;
        Debug.Log("Spiel beendet!");
    }
}
