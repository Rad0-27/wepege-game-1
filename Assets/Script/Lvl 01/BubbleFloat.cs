using UnityEngine;

public class BubbleFloat : MonoBehaviour
{
    Vector3 startLocalPos;
    float speed;
    float height;

    void Start()
    {
        startLocalPos = transform.localPosition;
        speed = Random.Range(0.8f, 1.4f);
        height = Random.Range(5f, 10f);
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * height;
        transform.localPosition = startLocalPos + new Vector3(0, y, 0);
    }
}