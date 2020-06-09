using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButtonUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    public event EventHandler<PointerEventData> onLeftMouseClick;
    public event EventHandler<PointerEventData> onRightMouseClick;
    public event EventHandler<PointerEventData> onDragClick;
    public event EventHandler<PointerEventData> onEndDragClick;
    public event EventHandler<PointerEventData> onPointer;
    public event EventHandler<PointerEventData> onDoubleLeftMouseClick;

    private float clickTime;
    private bool isDoubleClick;

    protected virtual void Update()
    {
        if (clickTime > 0)
        {
            isDoubleClick = true;
            clickTime -= Time.deltaTime;
        }
        else
        {
            isDoubleClick = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        onDragClick?.Invoke(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Last Press: " + eventData.lastPress.name);
        onEndDragClick?.Invoke(this, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointer?.Invoke(this, eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && isDoubleClick)
        {
            Debug.Log("DoubleClick");
            onDoubleLeftMouseClick?.Invoke(this, eventData);
        }
        else if (Input.GetMouseButton(0))
        {
            clickTime += 0.5f;
            onLeftMouseClick?.Invoke(this, eventData);
        }
        else if (Input.GetMouseButton(1))
        {
            onRightMouseClick?.Invoke(this, eventData);
        }
    }


}
