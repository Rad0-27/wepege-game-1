using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    public int levelIndex;
    public Image starImage;

    private Sprite starFull;
    private Sprite starEmpty;

    [Header("Lock")]
    public GameObject lockImage;

    private Button button;
    private Image buttonImage;

    [Header("Color")]
    public Color normalColor = Color.white;
    public Color lockedColor = new Color(0.6f, 0.6f, 0.6f);

    void Awake()
    {
        starFull = Resources.Load<Sprite>("UI/Stars/stars_full");
        starEmpty = Resources.Load<Sprite>("UI/Stars/stars_empty");
        // ambil otomatis dari object ini
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    void Start()
    {
        UpdateStars();
        UpdateLockState();
    }

    void UpdateStars()
    {
        bool complete = LevelProgressManager.instance.IsLevelComplete(levelIndex);
        starImage.sprite = complete ? starFull : starEmpty;
    }

    void UpdateLockState()
    {
        if (levelIndex == 1)
        {
            Unlock();
            return;
        }

        bool prevComplete = LevelProgressManager.instance.IsLevelComplete(levelIndex - 1);

        if (prevComplete)
            Unlock();
        else
            Lock();
    }

    void Lock()
    {
        button.interactable = false;

        if (lockImage != null)
            lockImage.SetActive(true);

        if (buttonImage != null)
            buttonImage.color = lockedColor;
    }

    void Unlock()
    {
        button.interactable = true;

        if (lockImage != null)
            lockImage.SetActive(false);

        if (buttonImage != null)
            buttonImage.color = normalColor;
    }
}