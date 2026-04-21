using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public Vector3 startPos;
    public Vector3 endPos;

    public float speed = 2f;

    [Header("Timing")]
    public float delayBeforeShow = 1.5f;
    public float fadeSpeed = 2f;

    private bool goingForward = true;
    private bool isActive = false;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // mulai invisible
        Color c = sr.color;
        c.a = 0;
        sr.color = c;
    }

    void Start()
    {
        transform.position = startPos;
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        // ⏱ delay sebelum muncul
        yield return new WaitForSeconds(delayBeforeShow);

        // ✨ fade in
        yield return StartCoroutine(Fade(0, 1));

        isActive = true;
    }

    void Update()
    {
        if (!isActive) return;

        MoveLoop();
    }

    void MoveLoop()
    {
        Vector3 target = goingForward ? endPos : startPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            goingForward = !goingForward;
        }
    }

    public void StopTutorial()
    {
        StartCoroutine(StopRoutine());
    }

    IEnumerator StopRoutine()
    {
        isActive = false;

        // ✨ fade out
        yield return StartCoroutine(Fade(1, 0));

        gameObject.SetActive(false);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;

            float alpha = Mathf.Lerp(from, to, t);

            Color c = sr.color;
            c.a = alpha;
            sr.color = c;

            yield return null;
        }

        Color final = sr.color;
        final.a = to;
        sr.color = final;
    }
}