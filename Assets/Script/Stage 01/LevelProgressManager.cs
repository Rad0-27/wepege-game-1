using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager instance;

    void Awake()
    {
        instance = this;
    }

    // =========================
    // SAVE (LEVEL SELESAI)
    // =========================
    public void SetLevelComplete(int levelIndex)
    {
        PlayerPrefs.SetInt("Level_" + levelIndex, 1);
        PlayerPrefs.Save();
    }

    // =========================
    // CHECK STATUS
    // =========================
    public bool IsLevelComplete(int levelIndex)
    {
        return PlayerPrefs.GetInt("Level_" + levelIndex, 0) == 1;
    }
}