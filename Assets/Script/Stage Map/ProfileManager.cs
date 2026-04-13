using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    [Header("UI Display")]
    public TMP_Text nameText;
    public TMP_Text namaedit;
    public Image profileIcon;
    public Image InSettingI;

    [Header("Edit UI")]
    public TMP_InputField inputName;

    [Header("Sprites")]
    public Sprite maleIcon;
    public Sprite femaleIcon;
    public Sprite mengIcon;

    private string playerName;
    private string gender;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadData();
        UpdateUI();
    }

    // =========================
    // LOAD DATA
    // =========================
    public void LoadData()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        gender = PlayerPrefs.GetString("Gender", "Male");
    }

    // =========================
    // SAVE DATA
    // =========================
    public void SaveData()
    {
        PlayerPrefs.SetString("PlayerName", inputName.text);
        PlayerPrefs.Save();
        LoadData();
        UpdateUI();
    }

    public void BoyUp()
    {
        PlayerPrefs.SetString("Gender", "Boy");
        profileIcon.sprite = maleIcon;
        InSettingI.sprite = maleIcon;
    }

    public void GirlUp()
    {
        PlayerPrefs.SetString("Gender", "Girl");
        profileIcon.sprite = femaleIcon;
        InSettingI.sprite = femaleIcon;
    }

    public void MengUp()
    {
        PlayerPrefs.SetString("Gender", "Meng");
        profileIcon.sprite = mengIcon;
        InSettingI.sprite = mengIcon;
    }

    // =========================
    // UPDATE UI
    // =========================

    void UpdateUI()
    {
        nameText.text = playerName;
        namaedit.text = playerName;

        if (gender == "Boy")
        {
            profileIcon.sprite = maleIcon;
            InSettingI.sprite = maleIcon;
        }
        if (gender == "Girl")
        {
            profileIcon.sprite = femaleIcon;
            InSettingI.sprite = femaleIcon;
        }
        if(gender == "Meng")
        {  
            profileIcon.sprite = mengIcon;
            InSettingI.sprite = mengIcon;
        }
    }


    // =========================
    // OPEN EDIT
    // =========================
    public void OpenEdit()
    {

    }
}