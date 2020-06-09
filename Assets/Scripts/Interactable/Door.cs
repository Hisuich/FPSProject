using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    // Start is called before the first frame update
    public bool isClosed;
    public string key;

    private bool isOpen;

    override protected void Start()
    {
        
    }

    // Update is called once per frame
    override protected void Update()
    {
        
    }

    public override void Interact(GameObject interactee)
    {
        base.Interact(interactee);
        Debug.Log("Hi, i'm interact");
        if (isClosed)
        {
            PlayerInventory playerInventory = interactee.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                if (playerInventory.GetItem(key))
                    OpenCloseDoor();
                else
                {
                    Debug.Log("Doesn't have a key");
                    return;
                }
            }
            else
            {
                Debug.Log("Interactee doesn't have PlayerInventory: Door, Interact: " + interactee.name);
                return;
            }
        }
        OpenCloseDoor();
        
    }

    public void OpenCloseDoor()
    {
        Quaternion doorRotate = Quaternion.identity;
        Vector3 offset = Vector3.zero;
        if (isOpen)
        {
            doorRotate.eulerAngles = new Vector3(0, 90, 0);
            offset.z += 0.5f;
            offset.x -= 0.5f;
        }
        else
        {
            doorRotate.eulerAngles = new Vector3(0, -90, 0);
            offset.z -= 0.5f;
            offset.x += 0.5f;
        }
        isOpen = !isOpen;
        transform.Rotate(doorRotate.eulerAngles);
        transform.position = transform.position + offset;
    }
}
