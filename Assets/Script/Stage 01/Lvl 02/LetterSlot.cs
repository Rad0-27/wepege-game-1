using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public char correctLetter;

    public LetterDrag currentLetter;

    [Header("Size Control")]
    public float targetSize = 1.5f;   // ukuran slot di world

    public void SetVisual(Sprite sprite)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;

        // transparansi (bayangan)
        Color c = sr.color;
        c.a = 0.5f;
        sr.color = c;

        // ✅ AUTO SCALE
        float spriteSize = sr.bounds.size.x;

        if (spriteSize > 0)
        {
            float scale = targetSize / spriteSize;
            transform.localScale = Vector3.one * scale;
        }
    }

    public void SetLetter(LetterDrag letter)
    {
        currentLetter = letter;

        // highlight saat terisi
        GetComponent<SpriteRenderer>().color = Color.white;

        NameLevelManager.instance.CheckSlots();
    }

    public void ClearSlot()
    {
        currentLetter = null;

        // kembali ke transparan
        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 0.5f;
        GetComponent<SpriteRenderer>().color = c;
    }
}