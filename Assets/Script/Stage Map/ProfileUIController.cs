using UnityEngine;

public class ProfileUIController : MonoBehaviour
{
    public GameObject profilePanel;
    public GameObject editPanel;

    private void Start()
    {
        profilePanel.SetActive(false);
        editPanel.SetActive(false);
    }
    public void OpenProfile()
    {
        profilePanel.SetActive(true);
    }

    public void CloseProfile()
    {
        profilePanel.SetActive(false);
    }

    public void OpenEdit()
    {
        editPanel.SetActive(true);
        ProfileManager.instance.OpenEdit();
    }

    public void CloseEdit()
    {
        editPanel.SetActive(false);
        ProfileManager.instance.SaveData();
    }
}