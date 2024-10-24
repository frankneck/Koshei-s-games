using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D body;
    private Vector2 movementInput;

    [SerializeField]
    private float force = 10;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        body.AddForce(movementInput * force);
    }
}
