using UnityEngine;

public class NPCBase : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Vector2 inputVector;
    public bool isWalking;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>(); 
    }
    protected virtual void Update()
    {
        UpdateAnimator();
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }
    protected void UpdateAnimator()
    {
        isWalking = inputVector != Vector2.zero;
        animator.SetFloat("X", inputVector.x);
        animator.SetFloat("Y", inputVector.y);
        animator.SetBool("IsWalking", isWalking);
    }
    protected void Move()
    {
        rb.MovePosition(rb.position + inputVector * moveSpeed * Time.fixedDeltaTime);
    }
    public void StopMoving()
    {
        inputVector = Vector2.zero;
        rb.linearVelocity = Vector2.zero; 
        UpdateAnimator(); 
    }
}
