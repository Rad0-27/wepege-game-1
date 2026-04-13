using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LetterDatabase database;
    public Transform spawnPoint;

    private string playerName;
    private int currentIndex = 0;

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "PLAYER").ToUpper();
        SpawnLetter();
    }

    void SpawnLetter()
    {
        // jika semua huruf selesai
        if (currentIndex >= playerName.Length)
        {
            LevelComplete();
            return;
        }

        string letter = playerName[currentIndex].ToString();

        // skip spasi
        if (letter == " ")
        {
            NextLetter();
            return;
        }

        GameObject prefab = database.GetLetter(letter);

        if (prefab != null)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefab huruf tidak ditemukan: " + letter);
            NextLetter();
        }
    }

    public void NextLetter()
    {
        currentIndex++;
        SpawnLetter();
    }

    void LevelComplete()
    {
        Debug.Log("Level selesai!");
    }
}