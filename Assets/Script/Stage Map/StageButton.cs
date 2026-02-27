using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject levelPanel;
    public Button[] levelButtons;

    public void OpenStage(int stageIndex)
    {
        levelPanel.SetActive(true);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNumber = i + 1;
            string sceneName = "Stage" + stageIndex + "_Lvl " + levelNumber;

            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            });
        }
    }

    public void ClosePanel()
    {
        levelPanel.SetActive(false);
    }
}