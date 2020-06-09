using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{
    public GameObject closeButton;
    protected const float timeToInteract = 0.5f;
    protected float timeElapsed;
    protected bool canInteract = true;

    virtual protected void Start()
    {
        GameObject newCloseButton = transform.Find("CloseButton").gameObject;
        if (newCloseButton == null)
        {
            newCloseButton = GameObject.Instantiate(closeButton, gameObject.transform);
            RectTransform rectTransform = newCloseButton.GetComponent<RectTransform>();
            Vector2 windowSize = gameObject.GetComponent<RectTransform>().rect.size;
            Debug.Log("windowSize: UIWindow, Start: " + windowSize);
            rectTransform.anchoredPosition = new Vector2(windowSize.x - 5 - rectTransform.rect.width, windowSize.y - 5 - rectTransform.rect.height);
        }
        Button button = newCloseButton.GetComponent<Button>();
        button.onClick.AddListener(Close);

    }
    virtual protected void Update()
    {
        Debug.Log("timeElapsed: UIWindow, Update: " + timeElapsed);
        if (timeElapsed <= 0)
        {
            canInteract = true;
        }
        else
        {
            timeElapsed -= Time.deltaTime;
            canInteract = false;
        }
    }
    virtual public void Open()
    {
        GlobalVar.isUIOpen = true;
        gameObject.SetActive(true);
    }
    virtual public void Close()
    {
        GlobalVar.CloseUI();
        gameObject.SetActive(false);
    }
}
