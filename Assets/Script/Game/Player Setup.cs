using UnityEngine;
using TMPro;

public class PlayerSetupUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public string selectedGender;
    public GameObject setupPanel;

    private void Start()
    {
        if (PlayerDataManager.Instance.HasPlayedBefore())
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
        if (nameInput.text != "" && selectedGender != "")
        {
            PlayerDataManager.Instance.SaveData(nameInput.text, selectedGender);
            setupPanel.SetActive(false);
        }
    }
}