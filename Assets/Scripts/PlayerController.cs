using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    //COmponents
    private PlayerInput playerInput;

    //Vars
    private float moveSpeed = 5;
    public float rotSpeed = 1;
    private float curXRot = 0;
    private float mouseX;
    private Vector2 moveInput;

    private void Awake()
    {
        playerInput = new PlayerInput();

        //Assign this function to the input event
        playerInput.Character.Move.performed += e => Move();
        playerInput.Character.Rotation.performed += e => Rotate();

        //Enable player input
        playerInput.Enable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        mouseX = context.ReadValue<Vector2>().x;
        float mouseY = context.ReadValue<Vector2>().y;

        curXRot += -mouseY * rotSpeed;
    }


    private void LateUpdate()
    {
        //Rotation
        transform.eulerAngles = new Vector3(curXRot, transform.eulerAngles.y + (mouseX * rotSpeed), 0);

        //--Camera movement--
        //Normalize direction
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;

        Vector3 dir = forward * moveInput.y + right * moveInput.x;
        dir.Normalize();

        //Actual movement
        dir *= moveSpeed * Time.deltaTime;
        transform.position += dir;
    }
}
