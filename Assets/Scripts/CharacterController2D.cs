using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float speedJump = 10;

    [Header("Gravity Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CircleCollider2D groundCollider;
    [SerializeField] private float gravity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float maxSlopeAngle = 60f; // ќграничим угол поверхности по которой мы можем двигатьс€

    private Rigidbody2D body;
    private ContactFilter2D groundFilter;
    private Collider2D[] collides = new Collider2D[16];
    private RaycastHit2D[] raycasts = new RaycastHit2D[16]; // —оздим буфер дл€ сохранени€ результатов коллизий
    private Vector2? slopeNormal; // Ќаша нормаль поверзности по которой мы будем двигатьс€, если null то поверхности нет

    private bool isJump;
    private Vector2 movementInput;

    private Vector2 gravityVelocity;

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

    public void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        // ѕроверка угла наклона поверхностей
        SlopeCheck();

        if (!isGrounded)
        {
            gravityVelocity = gravityVelocity - Vector2.down * Physics2D.gravity * gravity * Time.fixedDeltaTime;
        }
        else
        {
            gravityVelocity = Vector2.Max(Vector2.zero, gravityVelocity);
        }

        if (isJump && isGrounded)
        {
            gravityVelocity += speedJump * Vector2.up;
        }

        isJump = false;

        var movementDirection = movementInput;
        if (Mathf.Abs(movementInput.x) > 0.1f)
        {
            // ≈сли поверхность есть, ескорректируем вектор направлени€
            if (slopeNormal.HasValue)
            {
                movementDirection = -Mathf.Sign(movementInput.x) * Vector2.Perpendicular(slopeNormal.Value).normalized;
            }
        }

        var deltaPosition = movementDirection * speed * Time.fixedDeltaTime;
        var deltaGravity = gravityVelocity * Time.fixedDeltaTime;
        body.MovePosition(body.position + deltaPosition + deltaGravity);
    }

    private void GroundCheck()
    {
        var count = groundCollider.OverlapCollider(groundFilter, collides);
        isGrounded = count > 0;
    }

    // ћетод дл€ поиска поверхностей
    private void SlopeCheck()
    {
        // —амый большой угол который мы нашли
        var maxAngle = 0f;
        // ¬ эту переменнную мы будем складывать результат проверки
        Vector2? normal = null;
        // Ѕудем провер€ть коллизии в трех направлени€х дл€ надежности
        Vector2[] directions = { Vector2.down, Vector2.right, Vector2.left };
        foreach (var direction in directions)
        {
            // Cast провер€ет с чем столкнетс€ наш коллайдер если мы его перенесм на рассто€ние 0.1f в направлении direction.
            var count = groundCollider.Cast(direction, groundFilter, raycasts, 0.1f);
            for (int index = 0; index < count; index++)
            {
                var hit = raycasts[index];
                if (hit.collider != null)
                {
                    // ѕровер€ем что угол наклона удовлетвор€ет нашему ограничению
                    float angle = Vector2.Angle(hit.normal, Vector2.up);
                    if (angle <= maxSlopeAngle && angle > maxAngle)
                    {
                        // ≈сли это так сохран€ем нормаль и угол
                        maxAngle = angle;
                        normal = hit.normal;
                    }
                }
            }
        }

        slopeNormal = normal;
    }
}