using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public PlayerData playerData = new PlayerData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData(string name, string gender)
    {
        playerData.playerName = name;
        playerData.gender = gender;

        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.SetString("Gender", gender);
        PlayerPrefs.SetInt("HasPlayed", 1);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        if (PlayerPrefs.GetInt("HasPlayed", 0) == 1)
        {
            playerData.playerName = PlayerPrefs.GetString("PlayerName");
            playerData.gender = PlayerPrefs.GetString("Gender");
        }
    }

    public bool HasPlayedBefore()
    {
        return PlayerPrefs.GetInt("HasPlayed", 0) == 1;
    }
}