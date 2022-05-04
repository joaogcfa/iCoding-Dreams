using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Movimentacao : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float crouchSpeed = 3f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float deceleration = 0.0f;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Desacelera()
    {
        if (deceleration < 0.3f)
        {
            deceleration += 0.01f;
        }
        else
        {
            deceleration = 5.0f;
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        InvokeRepeating("Desacelera", 1.0f, 0.1f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        walkingSpeed = 7.5f;

    }

    void Crouch()
    {
        if (Input.GetButton("Crouch"))
        {
            characterController.height = 1f;
            if (characterController.isGrounded)
            {
                walkingSpeed = crouchSpeed;
                runningSpeed = crouchSpeed;
            }
        }
        else
        {
            characterController.height = 2.0f;
            if (characterController.isGrounded)
            {
                walkingSpeed = walkingSpeed;
                runningSpeed = runningSpeed;
            }  
        }
    }



    void Update()
    {
        Crouch();

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? (runningSpeed-deceleration) : (walkingSpeed-deceleration)) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? (runningSpeed-deceleration) : (walkingSpeed-deceleration)) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (walkingSpeed <= 0f)
        {
            walkingSpeed = 0f;
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}