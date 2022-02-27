using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerInputControls playerInputControls;
    private PlayerInput playerInput;
    private Animator playerAnimController;
    private Rigidbody rb;

    [Header("Player Variables")]
    [SerializeField] private float m_fJumpForce = 7.0f;
    [SerializeField] private float m_fMoveSpeed = 15.0f;
    [SerializeField] private float m_fGroundedRadius = 2.0f;
    [SerializeField] private float m_fVerticalBlowbackForce = 4.0f;

    [SerializeField] private Vector3 m_moveInputVector = Vector3.zero;
    [SerializeField] private Vector3 m_ForceVector = Vector3.zero; // different so that we get the force applied to the character in world space

    [SerializeField] private Transform checkGroundRay;
    [SerializeField] private LayerMask GroundedLayers;
    [SerializeField] private Camera playerCamera;

    [Header("States")]
    [SerializeField]
    private bool isActive = false;
    public bool pause = false;
    public bool firing = false;
    [SerializeField] private bool isGrounded;

    [Header("Pause Menu")]
    [SerializeField] private GameObject PausePanel;
    // Animator Hashes
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isGroundedHash = Animator.StringToHash("isGrounded");
    public readonly int isJumpingHash = Animator.StringToHash("didJump");
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int didFireHash = Animator.StringToHash("didFire");

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
        playerInputControls.Player.Fire.canceled += OnFire;
        playerInputControls.Player.Jump.started += OnJump;
        playerInputControls.Player.Pause.started += OnPause;
    }

    void Start()
    {
        isActive = true;
        InitPlayerActions();
        rb.velocity = Vector3.zero;
        PausePanel.SetActive(false);
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
        playerInputControls.Player.Fire.canceled -= OnFire;
        playerInputControls.Player.Jump.started -= OnJump;
        playerInputControls.Player.Pause.started -= OnPause;

    }

    // Update is called once per frame
    void Update()
    {
        if (!(m_moveInputVector.magnitude >= 0)) m_moveInputVector = Vector2.zero;


    }

    /// <summary>
    /// MoveUpdate
    /// </summary>
    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        playerAnimController.SetBool(isGroundedHash, isGrounded);
        m_ForceVector += (m_moveInputVector.x * GetCameraRight(playerCamera) + m_moveInputVector.z * GetCameraForward(playerCamera)) * m_fMoveSpeed * Time.deltaTime;
        //rb.AddForce(m_ForceVector, ForceMode.Impulse);
        rb.MovePosition(rb.position + m_ForceVector * m_fMoveSpeed * Time.deltaTime);
        m_ForceVector = Vector3.zero;

        
    }

    private Vector3 GetCameraForward(Camera pCam)
    {
        Vector3 forward = pCam.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    
    private Vector3 GetCameraRight(Camera pCam)
    {
        Vector3 right = pCam.transform.right;
        right.y = 0;
        return right.normalized;
    }


    private void OnMove(InputAction.CallbackContext obj)
    {
        
        Vector2 velocity = obj.ReadValue<Vector2>();
        m_moveInputVector = new Vector3(velocity.x, 0, velocity.y);
        playerAnimController.SetFloat(movementYHash, velocity.y);
        playerAnimController.SetFloat(movementXHash, velocity.x);
    }

    private void OnFire(InputAction.CallbackContext obj)
    {
        float fired = obj.ReadValue<float>();
        if (fired > 0)
        {
            firing = true;
            playerAnimController.SetTrigger(didFireHash);
        }
        else
        {
            firing = false;
        }
        Debug.Log(firing);
        //playerAnimController.SetBool(isFiringHash, firing);
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            Debug.Log("Jump");

            rb.AddForce((Vector3.up * m_fJumpForce), ForceMode.Impulse);
            playerAnimController.SetTrigger(isJumpingHash);
        }
    }

    public void PauseButtonFunction()
    {
        pause = !pause;
        PausePanel.SetActive(pause);
        AppEvents.InvokeOnPauseEvent(pause);
    }

    public void OnPause(InputAction.CallbackContext obj)
    {
        Debug.Log("Pause");
        PauseButtonFunction();

    }

    /// <summary>
    /// Raycasts a line down to check if the player has touched the ground
    /// </summary>
    private bool CheckGrounded()
    {
        Vector3 CheckPosition = checkGroundRay.position;
        bool groundCheck = Physics.CheckSphere(CheckPosition, m_fGroundedRadius, GroundedLayers, QueryTriggerInteraction.Ignore);

        return groundCheck;
    }

    public void BounceBack(Vector3 blastOrigin, float blowbackForce)
    {
        Vector3 distToBlast = transform.position - blastOrigin;
        distToBlast.y = m_fVerticalBlowbackForce;
        Vector3 distToBlastNormalized = distToBlast.normalized;
        rb.AddForce(distToBlast * blowbackForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkGroundRay.position, m_fGroundedRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bounds")) // bounds is layer 6
        {
            if (GameSceneManager.Instance != null)
            {
                GameSceneManager.Instance.LoadEndScene();
            }
        }
    }
}
