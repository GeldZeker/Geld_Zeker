using GameStudio.GeldZeker.MiniGames.InvoiceDragging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Transform parentToReturnTo = null;
    [SerializeField]
    private ItemSlot itemSlot = null;

    private Outline outline = null;

    private bool isDragging = false;

    [SerializeField]
    private InvoiceDraggingBehaviour behaviour = null;

    public bool IsDraggable { get; private set; } = true;

    /// <summary> Method is called when the object begins beging dragged. </summary>
    public void OnBeginDrag (PointerEventData eventData)
     {
        if (!itemSlot.isCorrect)
        {
            parentToReturnTo = transform.parent;
            transform.SetParent(transform.parent);
            GetComponent<CanvasGroup>().alpha = .6f;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
     }

    /// <summary> Method is called when the object is being dragged. </summary>
    public void OnDrag (PointerEventData eventData)
     {
        if (!itemSlot.isCorrect) transform.position = eventData.position;
     }

    /// <summary> Method is called when the object stops being dragged. </summary>
    public void OnEndDrag (PointerEventData eventData)
     {
        transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.localPosition = Vector3.zero;
        behaviour.itemsCorrectPlace();
    }

    /// <summary> Sets whether this image is draggable or not. </summary>
    public void SetDraggability(bool value)
    {
        IsDraggable = value;
        if (isDragging && !value)
        {
            SetOutline(false);
        }
    }

    /// <summary> Sets the outline enableability of this dragdrop item. </summary>
    private void SetOutline(bool enabled)
    {
        outline.enabled = enabled;
    }
}
