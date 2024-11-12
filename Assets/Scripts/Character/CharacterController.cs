using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2DKinematic : MonoBehaviour
{
    [Header("Speed settings")]
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float speed = 10f;

    [Header("Gravity settings")]
    [SerializeField] private float gravity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isAbove;
    [SerializeField] private bool isLeftWall;
    [SerializeField] private bool isRightWall;
    [SerializeField] private LayerMask groundLayer;

    [Header("Character colliders")]
    [SerializeField] private CapsuleCollider2D groundCollider;
    [SerializeField] private CapsuleCollider2D aboveCollider;
    [SerializeField] private CapsuleCollider2D leftWallCollider;
    [SerializeField] private CapsuleCollider2D rightWallCollider;

    [Header("Other")]
    [SerializeField] private float maxSlopeAngle = 60f;

    private Rigidbody2D body;
    private ContactFilter2D groundFilter;
    private Collider2D[] collides = new Collider2D[16];  // Буфер для хранения res коллизий
    private RaycastHit2D[] raycasts = new RaycastHit2D[16];
    private Vector2? slopeNormal;
    private Vector2 gravityVelocity;
    private Vector2 movement;
    private bool isJump;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        groundFilter.SetLayerMask(groundLayer);
        groundFilter.useLayerMask = true;
        groundFilter.useTriggers = false;
    }

    public void OnJump()
    {
        isJump = true;
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        // SlopeCheck();
        LeftWallCheck();
        RightWallCheck();
        AboveCheck();

        if (movement.x == Vector2.right.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (movement.x == Vector2.left.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }


        if (!isGrounded)
        {
            gravityVelocity = Vector2.ClampMagnitude(gravityVelocity - Vector2.down * Physics2D.gravity * gravity * Time.fixedDeltaTime, 20f);
        }
        else
        {
            gravityVelocity = Vector2.Max(Vector2.zero, gravityVelocity);
        }

        if (isJump && isGrounded)
        {
            gravityVelocity += jumpSpeed * Vector2.up;
        }
        isJump = false;

        if (isAbove && gravityVelocity.y > 0)
        {
            gravityVelocity.y = 0;
        }

        var movementDirection = movement;

        if (movementDirection.normalized == Vector2.right)
            if (isRightWall)
                movementDirection.x = 0;
            else
                movementDirection = movementDirection;

        if (movementDirection.normalized == Vector2.left)
            if (isLeftWall)
                movementDirection.x = 0;
            else
                movementDirection = movementDirection;

        if (Mathf.Abs(movement.x) > 0.1f)
        {
            if (slopeNormal.HasValue)
            {
                movement = -Mathf.Sign(movement.x) * Vector2.Perpendicular(slopeNormal.Value).normalized;
            }
        }
        Debug.DrawLine(body.position, body.position + movementDirection, Color.red, 0.1f);
        var deltaPos = movementDirection * speed * Time.fixedDeltaTime;
        var deltaGravity = gravityVelocity * Time.fixedDeltaTime;
        body.MovePosition(body.position + deltaPos + deltaGravity);
    }

    private void GroundCheck()
    {
        var count = groundCollider.OverlapCollider(groundFilter, collides);
        isGrounded = count > 0;
    }


    private void LeftWallCheck()
    {
        var countL = leftWallCollider.OverlapCollider(groundFilter, collides);
        isLeftWall = countL > 0;
    }

    private void RightWallCheck()
    {
        var countR = rightWallCollider.OverlapCollider(groundFilter, collides);
        isRightWall = countR > 0;
    }

    private void AboveCheck()
    {
        var count = aboveCollider.OverlapCollider(groundFilter, collides);
        isAbove = count > 0;
    }

    private void SlopeCheck()
    {
        var maxAngle = 0f;
        Vector2? normal = null;
        Vector2[] directions = { Vector2.down, Vector2.right, Vector2.left };
        foreach (var direction in directions)
        {
            var count = groundCollider.Cast(direction, groundFilter, raycasts, 0.1f);
            for (int i = 0; i < count; i++)
            {
                var hit = raycasts[i];
                if (hit.collider != null)
                {
                    float angle = Vector2.Angle(hit.normal, Vector2.up);
                    if (angle <= maxSlopeAngle && angle > maxAngle)
                    {
                        maxAngle = angle;
                        normal = hit.normal;    // Результат проверки
                    }
                }
            }
        }

        slopeNormal = normal;
    }

}
