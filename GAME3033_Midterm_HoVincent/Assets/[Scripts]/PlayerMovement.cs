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

    [SerializeField]
    private bool isActive = false;

    private void Awake()
    {
        playerInputControls = new PlayerInputControls();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        isActive = true;
        Debug.Log("BIND DAMMIT");
        playerInputControls.Player.Enable();
        playerInputControls.Player.Move.performed += OnMove;
        playerInputControls.Player.Move.canceled += OnMove;
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

    public void OnMove(InputAction.CallbackContext obj)
    {
        Vector2 moveVector = obj.ReadValue<Vector2>();

        Debug.Log(moveVector);
    }

    public void OnLook(InputAction.CallbackContext obj)
    {
         
    }

    public void OnFire(InputAction.CallbackContext obj)
    {

    }

}
