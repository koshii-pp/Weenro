using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTakeUno : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    
    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float staminaDrainRate = 20f;
    [SerializeField] private float minStaminaToRun = 0.1f;
    
    [Header("Sorting Settings")]
    [SerializeField] private float sortingBaseOffset = 5000f;
    
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Vector2 movement;
    private float currentStamina;
    private bool isRunning;
    private string currentAnimation;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;
    
    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        currentStamina = maxStamina;
        
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0f;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        movement = new Vector2(horizontal, vertical).normalized;
        
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool hasEnoughStamina = currentStamina >= minStaminaToRun;
        isRunning = wantsToRun && hasEnoughStamina;
        
        if (isRunning && movement.magnitude > 0.1f)
        {
            currentStamina = Mathf.Max(0, currentStamina - staminaDrainRate * Time.deltaTime);
        }
        else if (!wantsToRun || movement.magnitude <= 0.1f)
        {
            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRegenRate * Time.deltaTime);
        }
        
        if (animator != null)
        {
            string newAnimation = "Idle";
            
            if (movement.magnitude > 0.1f)
            {
                if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
                {
                    newAnimation = vertical > 0 ? 
                        (isRunning ? "RunUp" : "WalkUp") : 
                        (isRunning ? "RunDown" : "WalkDown");
                }
                else
                {
                    newAnimation = isRunning ? "Run" : "Walk";
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.flipX = horizontal < 0;
                    }
                }
            }
            
            if (newAnimation != currentAnimation)
            {
                animator.Play(newAnimation);
                currentAnimation = newAnimation;
            }
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(sortingBaseOffset - transform.position.y);
        }
    }

    public float GetGroundSpeed()
    {
        return isRunning ? runSpeed : walkSpeed;
    }

    void FixedUpdate()
    {
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = movement * currentSpeed;
    }
}
