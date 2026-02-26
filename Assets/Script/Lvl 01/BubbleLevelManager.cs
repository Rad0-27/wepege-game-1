using UnityEngine;

public class BubbleLevelManager : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform parent;

    public Sprite[] letterSprites;      // 26 sprite A-Z
    public AudioClip[] letterSounds;    // 26 suara
    public AudioClip popSound;

    public GameObject nextButton;

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

        if (poppedCount >= targetToFinish)
            nextButton.SetActive(true);
    }
}