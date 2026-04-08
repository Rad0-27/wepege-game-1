using UnityEngine;

public class LetterDrag : MonoBehaviour
{
    public char letter;

    private Vector3 offset;
    private Vector3 startPos;

    private bool isPlaced = false;

    private LetterSlot currentSlotCandidate;

    void Start()
    {
        startPos = transform.position;
    }

    // =========================
    // SETUP DARI LEVEL MANAGER
    // =========================
    public void Setup(char c, Sprite sprite)
    {
        letter = c;

        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void OnMouseDown()
    {
        if (isPlaced)
        {
            isPlaced = false;
        }

        offset = transform.position - MouseWorld();
    }

    void OnMouseDrag()
    {
        if (isPlaced) return;

        transform.position = MouseWorld() + offset;
    }

    void OnMouseUp()
    {
        LetterSlot nearest = FindNearestSlot();

        if (nearest != null)
        {
            PlaceInSlot(nearest);
        }
    }

    LetterSlot FindNearestSlot()
    {
        LetterSlot[] allSlots = FindObjectsByType<LetterSlot>(FindObjectsSortMode.None);

        float minDistance = 0.7f; // radius snap (bisa diatur)
        LetterSlot nearest = null;

        foreach (LetterSlot slot in allSlots)
        {
            if (slot.currentLetter != null) continue;

            float dist = Vector2.Distance(transform.position, slot.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = slot;
            }
        }

        return nearest;
    }
    Vector3 MouseWorld()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    public void SetSlotCandidate(LetterSlot slot)
    {
        currentSlotCandidate = slot;
    }

    public void ClearSlotCandidate()
    {
        currentSlotCandidate = null;
    }

    void PlaceInSlot(LetterSlot slot)
    {
        isPlaced = true;

        slot.SetLetter(this);

        StartCoroutine(SmoothSnap(slot.transform.position));
    }

    System.Collections.IEnumerator SmoothSnap(Vector3 target)
    {
        float t = 0;
        Vector3 start = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * 10f;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
    }

    public void ReturnToStart()
    {
        isPlaced = false;
        transform.position = startPos;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}