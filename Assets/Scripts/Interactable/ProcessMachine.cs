using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessMachine : Interactable
{
    public CraftUI craftUI;
    private bool isCraftActive;

    public List<Item> craftItems;
    // Start is called before the first frame update

    void Awake()
    {

    }
    protected override void Start()
    {
        base.Start();
        craftItems = new List<Item>();
        InteractableName = "ProcessMachine";
        craftUI.gameObject.SetActive(false);
    }
    // Update is called once per frame
    override protected void Update()
    {
        if (Input.GetButtonDown("CloseAllUI"))
            CloseCraftUI();
    }

    public override void Interact(GameObject interactee)
    {
        base.Interact(interactee);
        Craft(interactee);
    }

    private void Craft(GameObject interactee)
    {
        craftUI.SetProcessMachine(this);
        craftUI.SetInteractee(interactee);
        craftUI.SetCraftItems();
        OpenCraftUI();
    }

    private void OpenCraftUI()
    {
        craftUI.Open();
    }

    private void CloseCraftUI()
    {
        craftUI.Close();
    }
}
