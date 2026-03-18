using UnityEngine;
using TMPro;

public class PlayerSetupUI : MonoBehaviour
{
    public GameObject setupPanel;

    public TMP_InputField nameInput;

    private string selectedGender;

    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            setupPanel.SetActive(false);
        }
        else
        {
            setupPanel.SetActive(true);
        }
    }

    public void SelectBoy()
    {
        selectedGender = "Boy";
    }

    public void SelectGirl()
    {
        selectedGender = "Girl";
    }

    public void OnStartButton()
    {
        if (nameInput.text == "" || selectedGender == "")
        {
            Debug.Log("Nama atau gender belum diisi");
            return;
        }

        PlayerPrefs.SetString("PlayerName", nameInput.text);
        PlayerPrefs.SetString("Gender", selectedGender);
        PlayerPrefs.Save();

        setupPanel.SetActive(false);

        Debug.Log("Player Saved: " + nameInput.text);
    }

    // ⭐ dipanggil oleh tombol reset
    public void ShowSetupPanel()
    {
        nameInput.text = "";
        selectedGender = "";
        setupPanel.SetActive(true);
    }
}
