using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Scrollbar;

public class ListScrollUI : UIWindow
{

    public GameObject listElement;
    public GameObject Container;
    protected Scrollbar scrollBar;
    protected List<RectTransform> listTransforms;
    protected List<Item> items;



    // Start is called before the first frame update
    protected void Awake()
    {
        scrollBar = Container.transform.Find("Scrollbar").gameObject.GetComponent<Scrollbar>();
        if (scrollBar == null)
        {
            Container.AddComponent<Scrollbar>();
        }
        scrollBar.onValueChanged.AddListener(OnValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValueChange(float value)
    {
    }
}
