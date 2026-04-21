using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LetterDatabase database;
    public Transform spawnPoint;

    public float spacing = 2.5f;

    public GameObject NextButton;

    private string playerName;
    public int currentLetterIndex = 0;

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "PLAYER").ToUpper();
        SpawnAllLetters();
    }

    void SpawnAllLetters()
    {
        int letterCount = playerName.Length;

        float totalWidth = (letterCount - 1) * spacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < letterCount; i++)
        {
            string letter = playerName[i].ToString();

            if (letter == " ") continue;

            GameObject prefab = database.GetLetter(letter);

            if (prefab != null)
            {
                Vector3 pos = spawnPoint.position + new Vector3(startX + i * spacing, 0, 0);

                GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
                obj.transform.SetParent(spawnPoint);

                // 🔥 hanya huruf pertama aktif
                //obj.SetActive(i == 0);
            }
            else
            {
                Debug.LogWarning("Prefab huruf tidak ditemukan: " + letter);
            }
        }
    }

    public void NextLetter()
    {
        // disable huruf sekarang
        if (currentLetterIndex < spawnPoint.childCount)
        {
            
        }

        currentLetterIndex++;

        // aktifkan huruf berikutnya
        if (currentLetterIndex < spawnPoint.childCount)
        {
            spawnPoint.GetChild(currentLetterIndex).gameObject.SetActive(true);
        }
        else
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        Debug.Log("Level selesai!");
        NextButton.SetActive(true);
    }

    public void Next()
    {
        SceneManager.LoadScene("Stage1_Lvl 4");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Stage Map");
    }
}