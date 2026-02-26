using UnityEngine;

public class DragBread : MonoBehaviour
{

    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnMouseDown()
    {
        transform.position = GetMouseWorldPos();
    }
    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos();
    }
    public Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 10f;
        return mouse;
    }
}