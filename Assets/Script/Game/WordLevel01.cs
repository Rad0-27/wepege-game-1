using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordLevelManager : MonoBehaviour
{
    public List<WordData> words;

    public Transform slotParent;
    public Transform letterParent;

    public GameObject slotPrefab;
    public GameObject letterPrefab;

    public Image clueImage;
    public GameObject nextButton;

    WordData current;

    void Start()
    {
        LoadRandomWord();
    }

    public void LoadRandomWord()
    {
        nextButton.SetActive(false);

        current = words[Random.Range(0, words.Count)];
        clueImage.sprite = current.clueImage;

        foreach (Transform c in slotParent) Destroy(c.gameObject);
        foreach (Transform c in letterParent) Destroy(c.gameObject);

        char[] chars = current.word.ToCharArray();
        List<char> spawnLetters = new List<char>();

        for (int i = 0; i < chars.Length; i++)
        {
            GameObject s = Instantiate(slotPrefab, slotParent);
            Slot slot = s.GetComponent<Slot>();

            if (System.Array.Exists(current.revealedIndexes, x => x == i))
            {
                slot.SetLetter(chars[i]);
            }
            else
            {
                spawnLetters.Add(chars[i]);
            }
        }

        Shuffle(spawnLetters);

        foreach (char c in spawnLetters)
        {
            GameObject l = Instantiate(letterPrefab, letterParent);
            l.GetComponent<DraggableLetter>().SetLetter(c.ToString());
        }
    }

    public void CheckAnswer()
    {
        string result = "";

        foreach (Slot s in slotParent.GetComponentsInChildren<Slot>())
        {
            if (!s.HasLetter()) return;
            result += s.currentLetter;
        }

        if (result == current.word)
            nextButton.SetActive(true);
    }

    void Shuffle(List<char> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            char tmp = list[i];
            int r = Random.Range(i, list.Count);
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}