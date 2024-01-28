using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{
    //COmponents
    private PlayerInput playerInput;
    public GameObject playerHead;
    public GameObject handSlot;
    public GameObject cartSlot;

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
    private bool isCartGrabbed = false;
    private GameObject cartObject;

    private bool isOnDialog;

    public bool IsOnDialog { get => isOnDialog; set => isOnDialog = value; }

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
        if (!isCartGrabbed)
        {
            Ray ray = new(playerHead.transform.position, playerHead.transform.forward);
            RaycastHit hit;
            Debug.DrawRay(playerHead.transform.position, playerHead.transform.forward * 3, Color.red, .5f);
            if (Physics.Raycast(ray, out hit, 3f, ~CartLayer))
            {
                if (hit.collider.CompareTag("Product")) //Pick up
                {
                    if (hit.collider.transform.parent != null)
                    {
                        if (hit.collider.transform.parent.TryGetComponent<ShelvesManager>(out ShelvesManager shelve))
                        {
                            if (!shelve.IsBusyNpc)
                            {
                                if (inHand == null)
                                {
                                    PickUp(hit.collider.gameObject);
                                }
                                else
                                {
                                    Debug.Log("Hands full");
                                }
                            }
                            else
                            {
                                Debug.Log("Shelf busy");
                                //TODO: Popup shelf busy
                            }
                        }
                        else
                        {
                            PickUp(hit.collider.gameObject);
                            PopUpManager.instance.CreatePopUp("", Color.white, "La estantería está ocupada.");
                        }
                    }
                    else
                    {
                        PickUp(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.CompareTag("Npc"))
                {
                    if (hit.collider.GetComponent<DialogManager>().CanTalk)
                    {
                        hit.collider.GetComponent<DialogManager>().OpenDialog();
                    }

                }
                else if (hit.collider.CompareTag("Baby"))
                {

                    if (hit.collider.GetComponent<BabyManager>().CanTalk)
                    {
                        hit.collider.GetComponent<BabyManager>().OpenBabyOptions();
                    }
                }
                else if (hit.collider.CompareTag("Cart"))
                {
                    if(cartObject == null)
                    {
                        cartObject = hit.collider.gameObject;
                    }
                    isCartGrabbed = true;
                    hit.transform.SetParent(cartSlot.transform);
                    hit.transform.position = cartSlot.transform.position;
                    hit.transform.rotation = cartSlot.transform.rotation;
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
    }

    /// <summary>
    /// Pick up item
    /// </summary>
    /// <param name="item"></param>
    private void PickUp(GameObject item)
    {
        inHand = item;
        inHand.GetComponent<Rigidbody>().isKinematic = true;
        inHand.transform.SetParent(handSlot.transform);
        inHand.transform.position = handSlot.transform.position;
    }

    public void Throw()
    {
        if (isCartGrabbed)
        {
            cartObject.transform.SetParent(null);
            isCartGrabbed = false;
        }
        else { 
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
    }


    private void LateUpdate()
    {
        //Rotation)
        if (!isOnDialog)
        {
            if (!(playerInput.Character.Rotation.ReadValue<Vector2>() == Vector2.zero))
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
}
