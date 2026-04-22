using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BreadSpawner spawner;
    public TMP_InputField inputField;
    public GameObject confirmButton;

    public BasketZone basketZone;

    private int correctTotal;
    private int totalBreadCount;


    void Start()
    {
        inputField.gameObject.SetActive(false);
        confirmButton.SetActive(false);

        StartNewRound();
        Debug.Log("this is start");
    }


    public void StartNewRound()
    {
        correctTotal = 0;

        inputField.text = "";
        inputField.gameObject.SetActive(false);
        confirmButton.SetActive(false);

        List<BreadItem> breads = spawner.SpawnRound();

        totalBreadCount = breads.Count;

        foreach (BreadItem b in breads)
        {
            correctTotal += b.price;
        }

        Debug.Log("TOTAL = " + correctTotal);
    }


    // ⭐ DIPANGGIL OLEH BASKET
    public void CheckAllCollected()
    {
        if (basketZone.CountInside() >= totalBreadCount)
        {
            Debug.Log("ALL BREAD COLLECTED");

            inputField.gameObject.SetActive(true);
            confirmButton.SetActive(true);
        }
    }


    public void CheckAnswer()
    {
        int playerValue;

        if (int.TryParse(inputField.text, out playerValue))
        {
            if (playerValue == correctTotal)
            {
                Debug.Log("BENAR!");
                StartNewRound();
            }
            else
            {
                Debug.Log("SALAH!");
            }
        }
    }
    public void ExitLevel()
    {
        SceneManager.LoadScene("Start screen");
    }
}