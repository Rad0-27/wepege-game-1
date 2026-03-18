using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public char correctLetter;

    public LetterDrag currentLetter;

    void OnTriggerEnter2D(Collider2D other)
    {
        LetterDrag letter = other.GetComponent<LetterDrag>();

        if (letter != null && currentLetter == null)
        {
            currentLetter = letter;

            letter.transform.position = transform.position;
        }
    }

    public void ClearSlot()
    {
        currentLetter = null;
    }
}