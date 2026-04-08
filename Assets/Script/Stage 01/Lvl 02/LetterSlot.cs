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
            letter.SetSlotCandidate(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        LetterDrag letter = other.GetComponent<LetterDrag>();

        if (letter != null)
        {
            letter.ClearSlotCandidate();
        }
    }

    public void SetLetter(LetterDrag letter)
    {
        currentLetter = letter;

        NameLevelManager.instance.CheckSlots();
    }

    public void ClearSlot()
    {
        currentLetter = null;
    }
}