using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float force = 40f;

    [Header("Gravity Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private Rigidbody2D body;
    private Vector2 movementInput;
    private bool isJump;
    private bool isGrounded;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>().normalized;
    }

    public void OnJump()
    {
        if (isGrounded)
        {
            isJump = true;
        }
    }


    private void FixedUpdate()
    {
        if (isJump)
        {
            body.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            isJump = false;
        }

        body.AddForce(movementInput * force);
    }
}