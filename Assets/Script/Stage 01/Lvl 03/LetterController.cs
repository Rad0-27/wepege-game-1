using UnityEngine;

public class LetterController : MonoBehaviour
{
    public StrokeTracer[] strokes;

    public int currentStroke = 0;

    void Start()
    {
        ActivateStroke(0);
    }

    void ActivateStroke(int index)
    {
        for (int i = 0; i < strokes.Length; i++)
        {
            bool isActive = (i == index);

            strokes[i].enabled = isActive;

            // aktifkan arrow hanya di stroke aktif
            if (strokes[i].arrow != null)
                strokes[i].arrow.SetActive(isActive);
        }
    }

    public void NextStroke()
    {
        currentStroke++;

        if (currentStroke >= strokes.Length)
        {
            CompleteLetter();
            return;
        }

        ActivateStroke(currentStroke);
    }

    public void ClearArrow()
    {
        if (currentStroke > 0)
        {
            if (strokes[currentStroke - 1].arrow != null)
                strokes[currentStroke - 1].arrow.SetActive(false);
        }

    }

    void CompleteLetter()
    {
        Debug.Log("Huruf selesai!");

        FindObjectOfType<LevelManager>().NextLetter();

        foreach (var s in strokes)
        {
            if (s.arrow != null)
                s.arrow.SetActive(false);
        }


    }
}