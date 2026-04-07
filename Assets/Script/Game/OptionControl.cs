using UnityEngine;

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
}
