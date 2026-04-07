using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NameLevelManager : MonoBehaviour
{
    public static NameLevelManager instance;

    public LetterLibrary library;

    public GameObject letterPrefab;
    public GameObject slotPrefab;

    public Transform spawnArea;
    public Transform slotContainer;

    private List<LetterSlot> slots = new List<LetterSlot>();

    string playerName;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName").ToUpper();

        Debug.Log("PlayerName: " + PlayerPrefs.GetString("PlayerName"));
        Debug.Log("SlotPrefab: " + slotPrefab);
        Debug.Log("SlotContainer: " + slotContainer);

        GenerateSlots();
        GenerateLetters();
    }

    void GenerateSlots()
    {
        float spacing = 1.5f;

        for (int i = 0; i < playerName.Length; i++)
        {
            Vector3 pos = slotContainer.position + new Vector3(i * spacing, 0, 0);

            GameObject obj = Instantiate(slotPrefab, pos, Quaternion.identity, slotContainer);

            LetterSlot slot = obj.GetComponent<LetterSlot>();

            slot.correctLetter = playerName[i];

            slots.Add(slot);
        }
    }

    void GenerateLetters()
    {
        List<char> letters = new List<char>(playerName.ToCharArray());

        Shuffle(letters);

        foreach (char c in letters)
        {
            Vector3 pos = spawnArea.position +
                new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0);

            GameObject obj = Instantiate(letterPrefab, pos, Quaternion.identity);

            LetterDrag drag = obj.GetComponent<LetterDrag>();

            Sprite sprite = library.GetSprite(c);

            drag.Setup(c, sprite);
        }
    }

    void Shuffle(List<char> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            char temp = list[i];

            int rand = Random.Range(i, list.Count);

            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public void CheckSlots()
    {
        foreach (LetterSlot s in slots)
        {
            if (s.currentLetter == null)
                return;
        }

        CheckAnswer();
    }

    void CheckAnswer()
    {
        bool correct = true;

        foreach (LetterSlot slot in slots)
        {
            if (slot.currentLetter.letter != slot.correctLetter)
            {
                correct = false;
                WrongEffect(slot);
            }
        }

        if (correct)
        {
            Debug.Log("BENAR!");
        }
    }

    void WrongEffect(LetterSlot slot)
    {
        StartCoroutine(Wiggle(slot.transform));

        if (slot.currentLetter != null)
        {
            slot.currentLetter.ReturnToStart();
            slot.ClearSlot();
        }
    }

    IEnumerator Wiggle(Transform t)
    {
        Vector3 start = t.position;

        for (int i = 0; i < 8; i++)
        {
            t.position = start + Vector3.left * 0.1f;
            yield return new WaitForSeconds(0.03f);

            t.position = start + Vector3.right * 0.1f;
            yield return new WaitForSeconds(0.03f);
        }

        t.position = start;
    }
}