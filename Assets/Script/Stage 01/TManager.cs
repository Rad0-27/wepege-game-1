using UnityEngine;

public class TManager : MonoBehaviour
{
    public static TManager instance;

    public Tutorial tutorialHand;

    private bool hasPlayerActed = false;

    void Awake()
    {
        instance = this;
    }

    public void PlayerDidAction()
    {
        if (hasPlayerActed) return;

        hasPlayerActed = true;

        tutorialHand.StopTutorial();
    }
}