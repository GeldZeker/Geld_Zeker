using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Transform parentToReturnTo = null;
 
     public void OnBeginDrag (PointerEventData eventData)
     {
        parentToReturnTo = transform.parent;
        transform.SetParent (transform.parent);
        GetComponent<CanvasGroup>().alpha = .6f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
     }
 
     public void OnDrag (PointerEventData eventData)
     {
        transform.position = eventData.position;
     }
 
     public void OnEndDrag (PointerEventData eventData)
     {
        transform.SetParent (parentToReturnTo);
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.localPosition = Vector3.zero;
     }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
}
