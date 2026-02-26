using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Text text;
    public string currentLetter = "";

    public void SetLetter(char c)
    {
        currentLetter = c.ToString();
        text.text = currentLetter;
    }

    public void ClearSlot()
    {
        currentLetter = "";
        text.text = "";
    }

    public bool HasLetter()
    {
        return currentLetter != "";
    }
}