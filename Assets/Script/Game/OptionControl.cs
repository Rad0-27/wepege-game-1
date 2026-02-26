using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Animator optionsAnimator;

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
}