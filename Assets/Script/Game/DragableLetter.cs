using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableLetter :
MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Text text;

    Vector3 startPos;
    Transform startParent;
    Canvas canvas;

    Slot previousSlot;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetLetter(string s)
    {
        text.text = s;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        startPos = transform.position;
        startParent = transform.parent;

        previousSlot = startParent.GetComponent<Slot>();

        if (previousSlot != null)
            previousSlot.ClearSlot();

        transform.SetParent(canvas.rootCanvas.transform);
    }

    public void OnDrag(PointerEventData e)
    {
        transform.position += (Vector3)e.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(e, hits);

        foreach (RaycastResult hit in hits)
        {
            Slot slot = hit.gameObject.GetComponent<Slot>();

            if (slot != null && !slot.HasLetter())
            {
                Snap(slot);
                return;
            }
        }

        // balik kalau tidak kena slot
        transform.position = startPos;
        transform.SetParent(startParent);
    }

    void Snap(Slot slot)
    {
        slot.SetLetter(text.text[0]);

        transform.position = slot.transform.position;
        transform.SetParent(slot.transform);

        FindObjectOfType<WordLevelManager>().CheckAnswer();
    }
}