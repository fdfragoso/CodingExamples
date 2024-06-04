using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Character
{
    public InventoryObject inventory;

    [Range(5.0f, 10.0f)]
    [SerializeField] float moveSpeed = 5; // make it a slider
    private PlayerInputActions playerInputActions;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Animator anim;
    private InputAction move, look;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        move = playerInputActions.Player.Move;
        move.Enable();
        look = playerInputActions.Player.Look;
        look.Enable();
    }

    private void Update()
    {
        MovementAndLook();

        if (Keyboard.current[Key.S].wasPressedThisFrame) //TODO testing save
        {
            inventory.Save();
        }

        if (Keyboard.current[Key.L].wasPressedThisFrame) //TODO testing load
        {
            inventory.Load();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
        bool isMoving = rb.velocity.magnitude > 0;
        anim.SetBool("Moving", isMoving);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<Item>();
        if(item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(collision.gameObject);
        }
    }

    private void MovementAndLook()
    {
        moveDirection = move.ReadValue<Vector2>();
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var mouseDir = mousePos - transform.position;
        anim.SetFloat("LookX", mouseDir.x);
        anim.SetFloat("LookY", mouseDir.y);
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
    }
}
