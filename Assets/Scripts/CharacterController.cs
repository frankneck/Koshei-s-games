using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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
    [SerializeField] private CapsuleCollider2D groundCollider;
    //[SerializeField] private CapsuleCollider2D aboveCollider;
    //[SerializeField] private CapsuleCollider2D leftWallCollider;
    //[SerializeField] private CapsuleCollider2D rightWallCollider;
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
        Debug.Log("Space performed");
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
 
    }

    private void FixedUpdate()
    {
        GroundCheck();
        // SlopeCheck();
        WallCheck();
        //RightWallCheck();
        //AboveCheck();


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

        if (movement.normalized == Vector2.right)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (movement.normalized == Vector2.left)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }


        if (Mathf.Abs(this.movement.x) > 0.1f)
        {
            if (slopeNormal.HasValue)
            {
                this.movement = -Mathf.Sign(this.movement.x) * Vector2.Perpendicular(slopeNormal.Value).normalized;
            }
        }
        Debug.DrawLine(body.position, body.position + movement, Color.red, 0.1f);
        var deltaPos = movement * speed * Time.fixedDeltaTime;
        var deltaGravity = gravityVelocity * Time.fixedDeltaTime;
        body.MovePosition(body.position + deltaPos + deltaGravity);
    }

    private void GroundCheck()
    {
        var count = groundCollider.OverlapCollider(groundFilter, collides);
        isGrounded = count > 0;
    }


    private void WallCheck()
    {
        Vector2[] directions = { Vector2.up, Vector2.right, Vector2.left };

        foreach (var direction in directions)
        {
            var count = GetComponent<Collider2D>().Cast(direction, groundFilter, raycasts, 0.1f);

            if (count > 0)
            {
                if (movement.y == Vector2.up.y && direction == Vector2.up && gravityVelocity.y > 0)
                {
                    gravityVelocity.y = 0;
                }
                else
                    movement = movement;
                
                if (movement.normalized == Vector2.right && direction == Vector2.right)
                {
                    movement *= 0;
                }
                else
                    movement = movement;
                
                if (movement.normalized == Vector2.left && direction == Vector2.left)
                {
                    movement *= 0;
                }
                else
                    movement = movement;
            }
        }
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
