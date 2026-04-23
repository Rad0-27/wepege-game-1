using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator optionsAnimator;
    public Animator Credit;

    public void OpenOptions()
    {
        optionsAnimator.ResetTrigger("ClosePanel");
        optionsAnimator.SetTrigger("OpenPanel");
    }

    public void CloseOptions()
    {
        optionsAnimator.ResetTrigger("OpenPanel");
        optionsAnimator.SetTrigger("ClosePanel");
    }

    public void OpenCredit()
    {
        Credit.ResetTrigger("ClosePanel");
        Credit.SetTrigger("OpenPanel");
    }

    public void CloseCredit()
    {
        Credit.ResetTrigger("OpenPanel");
        Credit.SetTrigger("ClosePanel");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // cari ulang animator di scene baru
        optionsAnimator = GameObject.Find("OptionPanel")?.GetComponent<Animator>();
        Credit = GameObject.Find("CreditPanel")?.GetComponent<Animator>();
    }
}
