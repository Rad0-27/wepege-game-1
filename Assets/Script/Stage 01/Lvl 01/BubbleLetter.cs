using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BubbleLetter :
MonoBehaviour, IPointerClickHandler
{
    public Image letterImage;
    public Image bubbleImage;

    public AudioClip letterSound;
    public AudioClip popSound;

    public AudioSource audioSource;

    bool popped = false;

    public void Setup(Sprite letterSprite, AudioClip sound)
    {
        letterImage.sprite = letterSprite;
        letterSound = sound;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (popped) return;
        popped = true;

        StartCoroutine(PopRoutine());
    }

    IEnumerator PopRoutine()
    {
        Vector3 original = transform.localScale;

        // goyang kecil
        for (int i = 0; i < 6; i++)
        {
            transform.localScale = original * 1.08f;
            yield return new WaitForSeconds(0.04f);
            transform.localScale = original;
            yield return new WaitForSeconds(0.04f);
        }

        // suara pop
        audioSource.PlayOneShot(popSound);

        float t = 0;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(original, original * 1.5f, t * 4);
            yield return null;
        }

        bubbleImage.enabled = false;
        letterImage.enabled = false;

        audioSource.PlayOneShot(letterSound);

        FindObjectOfType<BubbleLevelManager>().RegisterPop();
    }
}