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
    private Rigidbody rb;

    [Header("Player Variables")]
    [SerializeField] private float m_fJumpForce = 7.0f;
    [SerializeField] private float m_fmoveSpeed = 15.0f;

    [SerializeField] private Vector2 m_moveInputVector = Vector2.zero;
    [SerializeField] private Transform checkGroundRay;

    /// <summary>
    /// Initializer
    /// </summary>
    
    [Header("States")]
    [SerializeField]
    private bool isActive = false;
    [SerializeField] private bool isGrounded;

    private void Awake()
    {
        playerInputControls = new PlayerInputControls();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
    }
    void Start()
    {
        isActive = true;
        Debug.Log("BIND DAMMIT");
        playerInputControls.Player.Enable();
        playerInputControls.Player.Move.performed += OnMove;
        playerInputControls.Player.Move.canceled += OnMove;

        if (!GameManager.Instance.cursorActive)
        {
            AppEvents.InvokeOnMouseCursorEnable(false);
        }
    }
    private void OnEnable()
    {
        if (isActive)
        {
            Debug.Log("BIND DAMMIT");
            playerInputControls.Player.Move.Enable();
            playerInputControls.Player.Move.performed += OnMove;
            playerInputControls.Player.Move.canceled += OnMove;
        }
    }

    private void OnDisable()
    {
        playerInputControls.Player.Move.Disable();
        playerInputControls.Player.Move.performed -= OnMove;
        playerInputControls.Player.Move.canceled -= OnMove;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// MoveUpdate
    /// </summary>
    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();
    }

    public void OnMove(InputAction.CallbackContext obj)
    {
        m_moveInputVector = obj.ReadValue<Vector2>();

        Debug.Log(m_moveInputVector);
    }

    public void OnLook(InputAction.CallbackContext obj)
    {
         
    }

    public void OnFire(InputAction.CallbackContext obj)
    {

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
