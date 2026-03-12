using UnityEngine;
using System.Collections;

public class IntroCameraMove : MonoBehaviour
{
    public Vector2 startTarget;   // Character
    public Transform endTarget;     // Titik tengah
    public float startZoom = 3f;
    public float endZoom = 8f;
    public float duration = 3f;
    public float waitDuration = 1f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        StartCoroutine(IntroMove());
    }

    IEnumerator IntroMove()
    {
        cam.orthographicSize = startZoom;
        transform.position = new Vector3(startTarget.x, startTarget.y, -10);

        yield return new WaitForSeconds(waitDuration);

        float timer = 0;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(endTarget.position.x, endTarget.position.y, -10);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, timer / duration);

            transform.position = Vector3.Lerp(startPos, endPos, t);
            cam.orthographicSize = Mathf.Lerp(startZoom, endZoom, t);

            yield return null;
        }
    }
}