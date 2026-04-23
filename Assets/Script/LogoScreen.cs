using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LogoScreen : MonoBehaviour
{
    public Image logo;
    public float fadeDuration = 1f;
    public float showTime = 1f;

    void Start()
    {
        StartCoroutine(PlayLogo());
    }

    IEnumerator PlayLogo()
    {
        yield return Fade(0, 1); // fade in
        yield return new WaitForSeconds(showTime);
        yield return Fade(1, 0); // fade out

        SceneManager.LoadScene("Start Screen");
    }

    IEnumerator Fade(float start, float end)
    {
        float t = 0;
        Color c = logo.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / fadeDuration);
            logo.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
    }
}