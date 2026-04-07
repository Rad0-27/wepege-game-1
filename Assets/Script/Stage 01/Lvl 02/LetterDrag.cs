using UnityEngine;

public class LetterDrag : MonoBehaviour
{
    public char letter;

    private Vector3 offset;
    private Vector3 startPos;

    public void Setup(char c, Sprite sprite)
    {
        letter = c;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Start()
    {
        startPos = transform.position;
    }

    void OnMouseDown()
    {
        offset = transform.position - MouseWorld();
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorld() + offset;
    }

    Vector3 MouseWorld()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    public void ReturnToStart()
    {
        transform.position = startPos;
    }
}