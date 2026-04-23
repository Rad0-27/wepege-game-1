using UnityEngine;

public class ResetPlayerData : MonoBehaviour
{
    public PlayerSetupUI setupUI;

    public Animator optionsAnimator;

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        optionsAnimator.ResetTrigger("OpenPanel");
        optionsAnimator.SetTrigger("ClosePanel");

        setupUI.ShowSetupPanel();

        Debug.Log("Player Data Reset");
    }
}
