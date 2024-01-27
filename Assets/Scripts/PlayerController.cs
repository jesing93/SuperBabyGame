using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //COmponents
    private PlayerInput playerInput;
    public GameObject playerHead;
    public GameObject handSlot;

    //Vars
    private float moveSpeed = 5;
    private float minXRot = -90;
    private float maxXRot = 90;
    private float rotSpeed = .5f;
    private float throwStrength = 400;
    private LayerMask CartLayer;

    private float curXRot = 0;
    private float mouseX;
    private Vector2 moveInput;
    private GameObject inHand = null;

    private void Awake()
    {
        CartLayer = LayerMask.GetMask("Cart");
        playerInput = new PlayerInput();

        //Assign this function to the input event
        playerInput.Character.Move.performed += ctx => Move(ctx);
        playerInput.Character.Move.canceled += ctx => Move(ctx);
        playerInput.Character.Rotation.performed += ctx => Rotate(ctx);
        playerInput.Character.Interact.performed += ctx => Interact();
        playerInput.Character.Throw.performed += ctx => Throw();

        //Enable player input
        playerInput.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    public void Interact()
    {
        Ray ray = new(playerHead.transform.position, playerHead.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(playerHead.transform.position, playerHead.transform.forward * 3, Color.red, .5f);
        if (Physics.Raycast(ray, out hit, 3f, ~CartLayer))
        {
            if (hit.collider.CompareTag("Product")) //Pick up
            {
                if (inHand == null)
                {
                    Debug.Log("Pick Product");
                    inHand = hit.collider.gameObject;
                    inHand.GetComponent<Rigidbody>().isKinematic = true;
                    inHand.transform.SetParent(handSlot.transform);
                    inHand.transform.position = handSlot.transform.position;
                }
                else
                {
                    Debug.Log("Hands full");
                }
            }
            else if (hit.collider.CompareTag("Npc"))
            {

            }
            else
            {
                Debug.Log("Invalid");
            }
        }
        else
        {
            Debug.Log("None");
        }
    }

    public void Throw()
    {
        if (inHand != null)
        {
            Debug.Log("Throw");
            inHand.transform.SetParent(null);
            inHand.GetComponent<Rigidbody>().isKinematic = false;
            inHand.GetComponent<Rigidbody>().AddForce(playerHead.transform.forward * throwStrength);
            inHand = null;
        }
        else
        {
            Debug.Log("Nothing in hand");
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
