using UnityEngine;
using System.Collections;

public class LetterDrag : MonoBehaviour
{
    public char letter;

    private Vector3 offset;
    private Vector3 startPos;

    private bool isPlaced = false;



    void Start()
    {
        startPos = transform.position;
    }

    public float scaleMultiplier = 1f;

    public void Setup(char c, Sprite sprite)
    {
        letter = c;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;

        // auto normalize size
        float targetSize = 1.5f;

        float spriteSize = sr.bounds.size.x;

        float scale = targetSize / spriteSize;

        transform.localScale = Vector3.one * scale * scaleMultiplier;
    }

    void OnMouseDown()
    {
        TManager.instance.PlayerDidAction();

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

    Vector3 MouseWorld()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    LetterSlot FindNearestSlot()
    {
        var allSlots = NameLevelManager.instance.GetAllSlots();

        float minDistance = 0.8f;
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

    void PlaceInSlot(LetterSlot slot)
    {
        isPlaced = true;

        slot.SetLetter(this);

        StartCoroutine(SmoothSnap(slot.transform.position));
    }

    IEnumerator SmoothSnap(Vector3 target)
    {
        float t = 0;
        Vector3 start = transform.position;

        while (t < 1)
        {
            t += Time.deltaTime * 6f;
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
}