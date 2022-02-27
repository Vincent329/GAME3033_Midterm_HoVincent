using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInputControls playerInputControls;
    [SerializeField]
    private PlayerInput playerInput;
    private Animator playerAnimController;

    private Rigidbody rb;

    [Header("Player Variables")]
    [SerializeField] private float m_fJumpForce = 7.0f;
    [SerializeField] private float m_fmoveSpeed = 15.0f;

    [SerializeField] private Vector2 m_moveInputVector = Vector2.zero;
    [SerializeField] private Vector3 m_ForceVector = Vector3.zero; // different so that we get the force applied to the character in world space

    [SerializeField] private Vector2 m_lookVector = Vector2.zero;
    [SerializeField] private Transform checkGroundRay;

    [SerializeField] private Camera playerCamera;

    /// <summary>
    /// Initializer
    /// </summary>

    [Header("States")]
    [SerializeField]
    private bool isActive = false;
    private bool pause = false;
    [SerializeField] private bool isGrounded;

    // Animator Hashes
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isGroundedHash = Animator.StringToHash("isGrounded");
    public readonly int isJumpingHash = Animator.StringToHash("didJump");
    public readonly int isFiringHash = Animator.StringToHash("isFiring");

    private void Awake()
    {
        playerInputControls = new PlayerInputControls();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerAnimController = GetComponent<Animator>();
        isGrounded = false;
    }

    private void InitPlayerActions()
    {
        Debug.Log("BIND DAMMIT");
        playerInputControls.Player.Enable();
        playerInputControls.Player.Move.performed += OnMove;
        playerInputControls.Player.Move.canceled += OnMove;
        playerInputControls.Player.Fire.started += OnFire;
        playerInputControls.Player.Jump.started += OnJump;
        playerInputControls.Player.Pause.started += OnPause;
    }

    void Start()
    {
        isActive = true;
        InitPlayerActions();


        if (!GameManager.Instance.cursorActive)
        {
            AppEvents.InvokeOnPauseEvent(false);
        }
    }
    private void OnEnable()
    {
        if (isActive)
        {
            InitPlayerActions();

        }
    }

    private void OnDisable()
    {
        playerInputControls.Player.Move.Disable();
        playerInputControls.Player.Move.performed -= OnMove;
        playerInputControls.Player.Move.canceled -= OnMove;
        playerInputControls.Player.Fire.started -= OnFire;
        playerInputControls.Player.Jump.started -= OnJump;
        playerInputControls.Player.Pause.started -= OnPause;

    }

    // Update is called once per frame
    void Update()
    {
        if (m_moveInputVector.magnitude > 0) m_moveInputVector = Vector2.zero;


    }

    /// <summary>
    /// MoveUpdate
    /// </summary>
    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        playerAnimController.SetBool(isGroundedHash, isGrounded);
    }

    public void OnMove(InputAction.CallbackContext obj)
    {
        m_moveInputVector = obj.ReadValue<Vector2>();
        playerAnimController.SetFloat(movementYHash, m_moveInputVector.y);
        playerAnimController.SetFloat(movementXHash, m_moveInputVector.x);
    }

    public void OnLook(InputAction.CallbackContext obj)
    {
        m_lookVector = obj.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext obj)
    {
        Debug.Log("Shoot some shit");
    }

    public void OnJump(InputAction.CallbackContext obj)
    {
        Debug.Log("Jump");
        rb.AddForce((Vector3.up * m_fJumpForce), ForceMode.Impulse);
        playerAnimController.SetTrigger(isJumpingHash);
    }
    public void OnPause(InputAction.CallbackContext obj)
    {
        Debug.Log("Pause");
        pause = !pause;
        AppEvents.InvokeOnPauseEvent(pause);
    }

    /// <summary>
    /// Raycasts a line down to check if the player has touched the ground
    /// </summary>
    private bool CheckGrounded()
    {
        bool groundCheck = Physics.Raycast(checkGroundRay.position, Vector3.down, 3.0f);
        return groundCheck;
    }

}
