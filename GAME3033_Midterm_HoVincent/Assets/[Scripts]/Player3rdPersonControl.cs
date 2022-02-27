using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player3rdPersonControl : MonoBehaviour
{

    [SerializeField] private GameObject CinemachineCameraTarget;
    [SerializeField] private float m_fAimSensitivity = 5.0f;
    [SerializeField] private float m_fBottomClamp = -30.0f;
    [SerializeField] private float m_fTopClamp = 70.0f;
    [SerializeField] private float m_fThreshold = 1.0f;
    [SerializeField] private float m_fCameraAngleOverride = 0.0f;
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private Transform aimLocation;
    [SerializeField] private WeaponScript weapon;
    public Transform getAimLocation => aimLocation;

    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;

    bool init = false;
    [SerializeField] private Vector2 m_lookVector;

    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        CinemachineCameraTarget.transform.rotation = Quaternion.identity;
        init = true;
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.playerInputControls.Player.Look.performed += OnLook;
        playerMovement.playerInputControls.Player.Look.canceled += OnLook;
    }

    private void OnEnable()
    {
        if (init)
        {
            playerMovement.playerInputControls.Player.Look.performed += OnLook;
            playerMovement.playerInputControls.Player.Look.canceled += OnLook;
        }
    }
    private void OnDisable()
    {
        playerMovement.playerInputControls.Player.Look.performed -= OnLook;
        playerMovement.playerInputControls.Player.Look.canceled -= OnLook;
    }
    // Update is called once per frame
    void Update()
    {
        CameraRotation();
        AimUpdate();
    }

    private void OnLook(InputAction.CallbackContext obj)
    {
        m_lookVector = obj.ReadValue<Vector2>();
    }

    void CameraRotation()
    {
        if (m_lookVector.sqrMagnitude >= m_fThreshold)
        {
            _cinemachineTargetYaw += m_lookVector.x * Time.deltaTime * m_fAimSensitivity;
            _cinemachineTargetPitch += m_lookVector.y * Time.deltaTime * m_fAimSensitivity;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, m_fBottomClamp, m_fTopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + m_fCameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void AimUpdate()
    {

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, aimColliderMask))
        {
            aimLocation.position = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

      

        // rotate the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, CinemachineCameraTarget.transform.rotation.eulerAngles.y, 0);
    }

}
