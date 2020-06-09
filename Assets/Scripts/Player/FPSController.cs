using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{

    public float speed;
    private Rigidbody rigidbody;
    bool isGrounded;
    bool isControllable;

    float jumpForce = 5;
    //Camera camera;

    private PlayerInventory inventory;
    private GameObject inventoryPool;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        //camera = gameObject.AddComponent<Camera>() as Camera;

        isGrounded = true;
        inventory = GetComponent<PlayerInventory>();
        inventoryPool = transform.Find("Inventory").gameObject;
    }

    private void Start()
    {
    }

    // Update is called once per frame

    void FixedUpdate()
    {

        if (!GlobalVar.isUIOpen)
        {
            Move();
            Turn();
            Jump();
            Use();
            SetFPSMouse();
        }
        else
        {
            SetInventoryMouse();
        }

        ButtonsHandler();
    }

    void SetFPSMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void SetInventoryMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement);
        movement *= speed * Time.deltaTime;
        movement.y = 0;
        rigidbody.MovePosition(transform.position + movement);
    }

    void Turn()
    {
        //Physics.Ra
        float turn = Input.GetAxis("Mouse X");
        float lookUp = Input.GetAxis("Mouse Y");
        Vector3 rot = new Vector3(-lookUp, turn, 0) + transform.eulerAngles;
        transform.eulerAngles = rot;
        
        //rigidbody.MoveRotation(quaternion);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
            isGrounded = false;
        }
        if (transform.position.y >= 0.5 && transform.position.y <= 0.6)
        {
            isGrounded = true;
        }
    }

    void Use()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition).direction, Color.black); 
        if (Physics.Raycast(GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("Takeable")))
        {
            Debug.Log("Use: Takeable");
            Item item = hit.transform.gameObject.GetComponent<Item>();
            if (Input.GetKey(KeyCode.E))
            {
                inventory.TakeItem(item);
                hit.transform.gameObject.transform.parent = inventoryPool.transform;
                hit.transform.gameObject.SetActive(false);
            }
        }

        if (Physics.Raycast(GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("Interactable")))
        {
            Debug.Log("Use: Interactable");
            if (Input.GetKey(KeyCode.E))
            {
                Interactable interactable = hit.transform.gameObject.GetComponent<Interactable>();
                interactable.Interact(gameObject);
            }
        }

        else if (Physics.Raycast(GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("Creature", "Enemy")))
        {
            Creature creature = hit.transform.gameObject.GetComponent<Creature>();
            if (Input.GetKey(KeyCode.E))
            {
                if (creature.isLootable)
                {
                    creature.Loot(gameObject);
                }
            }
        }
    }

    void ButtonsHandler()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventory.Open();
        }
        if (Input.GetButtonDown("CloseUI"))
        {
            GlobalVar.isUIOpen = !GlobalVar.isUIOpen;
        }
    }

    void Reload()
    {

    }
}
