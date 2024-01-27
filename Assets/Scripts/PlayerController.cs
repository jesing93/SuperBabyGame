using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //COmponents
    private PlayerInput playerInput;
    public GameObject playerHead;

    //Vars
    private float moveSpeed = 5;
    private float minXRot = -90;
    private float maxXRot = 90;
    private float rotSpeed = .5f;
    private float curXRot = 0;
    private float mouseX;
    private Vector2 moveInput;

    private void Awake()
    {
        playerInput = new PlayerInput();

        //Assign this function to the input event
        playerInput.Character.Move.performed += ctx => Move(ctx);
        playerInput.Character.Move.canceled += ctx => Move(ctx);
        playerInput.Character.Rotation.performed += ctx => Rotate(ctx);

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
        curXRot = Mathf.Clamp(curXRot, minXRot, maxXRot);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Ray ray = new(transform.position, new Vector3(Camera.main.transform.forward.x, playerHead.transform.forward.y, 0));
        RaycastHit hit;
        Debug.DrawRay(transform.position, PlayerController.instance.transform.position - transform.position);
        if (Physics.Raycast(ray, out hit, 5f, groundLayer | playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerInSight = true;
                //Set last sight time + sight delay
                lastPlayerSight = Time.time + 3f;
            }
            else
            {
                playerInSight = false;
            }
        }
        else
        {
            playerInSight = false;
        }
    }


    private void LateUpdate()
    {
        //Rotation)
        if(!(playerInput.Character.Rotation.ReadValue<Vector2>() == Vector2.zero))
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + (mouseX * rotSpeed), 0);
            playerHead.transform.localEulerAngles = new Vector3(curXRot, 0, 0);
        }

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
