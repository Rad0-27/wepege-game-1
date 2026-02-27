using UnityEngine;
using System.Collections;

public class IntroZoom : MonoBehaviour
{
    public float startZoom = 3f;
    public float endZoom = 8f;
    public float zoomDuration = 2f;

    void Start()
    {
        StartCoroutine(ZoomOut());
    }

    IEnumerator ZoomOut()
    {
        Camera.main.orthographicSize = startZoom;

        yield return new WaitForSeconds(1f);

        float timer = 0;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(startZoom, endZoom, timer / zoomDuration);
            yield return null;
        }
    }
}