using UnityEngine;
using UnityEngine.SceneManagement;

public class BubbleLevelManager : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform parent;

    public Sprite[] letterSprites;      // 26 sprite A-Z
    public AudioClip[] letterSounds;    // 26 suara
    public AudioClip popSound;

    public GameObject nextButton;
    public PopEffect winPop;
    public AudioClip winsfx;

    int poppedCount = 0;
    public int targetToFinish = 10;

    void Start()
    {
        nextButton.SetActive(false);

        for (int i = 0; i < 26; i++)
        {
            GameObject b = Instantiate(bubblePrefab, parent);



            BubbleLetter bl = b.GetComponent<BubbleLetter>();

            bl.Setup(letterSprites[i], letterSounds[i]);
            bl.popSound = popSound;
        }
    }

    public void RegisterPop()
    {
        poppedCount++;

        if (poppedCount >= targetToFinish && LevelProgressManager.instance.IsLevelComplete(1))
            nextButton.SetActive(true);
        if (poppedCount >= 26)
        {
            LevelProgressManager.instance.SetLevelComplete(1);
            nextButton.SetActive(true);
            AudioManager.instance.PlaySFX(winsfx);
            winPop.PlayPop();
        }

    }
    public void Exit()
    {
        SceneManager.LoadScene("Stage Map");
    }

    public void Next()
    {
        SceneManager.LoadScene("Stage1_Lvl 2");
    }
}