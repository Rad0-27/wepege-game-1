using UnityEngine;
using TMPro;

public class PlayerSetupUI : MonoBehaviour
{
    public GameObject setupPanel;
    public GameObject page2;

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
        if (nameInput.text == "")
        {
            Debug.Log("Nama atau gender belum diisi");
            return;
        }

        PlayerPrefs.SetString("PlayerName", nameInput.text);
        
        setupPanel.SetActive(false);
        page2.SetActive(true);

        Debug.Log("Player Saved: " + nameInput.text);
    }

    public void closeSetUp()
    {
        if (selectedGender == "")
        {
            Debug.Log("Nama atau gender belum diisi");
            return;
        }
        PlayerPrefs.SetString("Gender", selectedGender);
        PlayerPrefs.Save();
        page2.SetActive(false);
    }

    // ⭐ dipanggil oleh tombol reset
    public void ShowSetupPanel()
    {
        nameInput.text = "";
        selectedGender = "";
        setupPanel.SetActive(true);
    }
}
