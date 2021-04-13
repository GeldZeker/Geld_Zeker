using GameStudio.GeldZeker.MiniGames.InvoiceDragging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private GameObject wrongMark = null;

    [SerializeField]
    private GameObject checkMark = null;

    public bool isCorrect = false;

    // Update is called each frame update.
    private void Update()
    {
        if(gameObject.transform.childCount < 3)
        {
            wrongMark.SetActive(false);
            checkMark.SetActive(false);
        }
    }

    /// <summary> Method is called when an object is being dropped on the object where this script is assigned to. </summary>
    public void OnDrop(PointerEventData eventData)
    {
        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
        if (dragDrop != null)
        {
            dragDrop.parentToReturnTo = transform;
            // if item is in correct box
            if (dragDrop.transform.name == dragDrop.parentToReturnTo.name)
            {
                checkMark.SetActive(true);
                wrongMark.SetActive(false);
                isCorrect = true;
            }
            // if item is NOT in correct box
            else
            {
                checkMark.SetActive(false);
                wrongMark.SetActive(true);
                isCorrect = false;
            }
        }
    }
}
