using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PopEffect : MonoBehaviour
{
    public float duration = 0.3f;
    public float maxScale = 1.2f;

    public GameObject target;

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
        target.SetActive(false);
    }

    public void PlayPop()
    {
        target.SetActive(true);
        transform.localScale = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(PopAnim());
    }

    IEnumerator PopAnim()
    {
        float time = 0f;

        // fase 1: membesar
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float scale = Mathf.Lerp(0f, maxScale, EaseOutBack(t));
            transform.localScale = originalScale * scale;

            yield return null;
        }

        time = 0f;

        // fase 2: kembali ke normal
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            float scale = Mathf.Lerp(maxScale, 1f, EaseInOut(t));
            transform.localScale = originalScale * scale;

            yield return null;
        }

        transform.localScale = originalScale;
    }

    // easing biar mantul enak
    float EaseOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }

    float EaseInOut(float x)
    {
        return x < 0.5f
            ? 2 * x * x
            : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }
}