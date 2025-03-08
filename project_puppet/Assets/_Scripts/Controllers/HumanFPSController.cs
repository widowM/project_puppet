using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CharacterController))]
public class HumanFPSController : MonoBehaviour
{
    [SerializeField] private Camera m_playerCamera;
    [SerializeField] private float m_walkSpeed = 6f;
    [SerializeField] private float m_runSpeed = 12f;
    [SerializeField] private float m_jumpPower = 7f;
    [SerializeField] private float m_gravity = 10f;
 
 
    [SerializeField] private float m_lookSpeed = 2f;
    [SerializeField] private float m_lookXLimit = 45f;
 
 
    private Vector3 m_moveDirection = Vector3.zero;
    private float m_rotationX = 0;
 
    private bool m_canMove = true;
 
    
    private CharacterController m_characterController;
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {
 
        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = m_canMove ? (isRunning ? m_runSpeed : m_walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = m_canMove ? (isRunning ? m_runSpeed : m_walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = m_moveDirection.y;
        m_moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        #endregion
 
        #region Handles Jumping
        if (Input.GetButton("Jump") && m_canMove && m_characterController.isGrounded)
        {
            m_moveDirection.y = m_jumpPower;
        }
        else
        {
            m_moveDirection.y = movementDirectionY;
        }
 
        if (!m_characterController.isGrounded)
        {
            m_moveDirection.y -= m_gravity * Time.deltaTime;
        }
 
        #endregion
 
        #region Handles Rotation
        m_characterController.Move(m_moveDirection * Time.deltaTime);
 
        if (m_canMove)
        {
            m_rotationX += -Input.GetAxis("Mouse Y") * m_lookSpeed;
            m_rotationX = Mathf.Clamp(m_rotationX, -m_lookXLimit, m_lookXLimit);
            m_playerCamera.transform.localRotation = Quaternion.Euler(m_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * m_lookSpeed, 0);
        }
 
        #endregion
    }
}