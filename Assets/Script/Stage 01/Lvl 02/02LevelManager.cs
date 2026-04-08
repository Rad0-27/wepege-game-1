using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NameLevelManager : MonoBehaviour
{
    public static NameLevelManager instance;

    [Header("Reference")]
    public LetterLibrary library;

    public GameObject letterPrefab;
    public GameObject slotPrefab;

    public Transform spawnArea;
    public Transform slotContainer;

    private List<LetterSlot> slots = new List<LetterSlot>();

    private string playerName;

    private bool isChecking = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // ambil nama player (pakai default biar aman)
        playerName = PlayerPrefs.GetString("PlayerName", "ABC").ToUpper();

        GenerateSlots();
        GenerateLetters();
    }

    // =========================
    // GENERATE SLOT
    // =========================
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

    // =========================
    // GENERATE LETTER
    // =========================
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

            if (sprite == null)
            {
                Debug.LogError("SPRITE TIDAK ADA: " + c);
            }

            drag.Setup(c, sprite);
        }
    }

    // =========================
    // SHUFFLE HURUF
    // =========================
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

    // =========================
    // AUTO CHECK SLOT
    // =========================
    public void CheckSlots()
    {
        if (isChecking) return;

        foreach (LetterSlot s in slots)
        {
            if (s.currentLetter == null)
                return;
        }

        isChecking = true;

        CheckAnswer();
    }

    // =========================
    // CEK JAWABAN
    // =========================
    void CheckAnswer()
    {
        bool correct = true;

        List<LetterSlot> wrongSlots = new List<LetterSlot>();

        foreach (LetterSlot slot in slots)
        {
            if (slot.currentLetter == null ||
                slot.currentLetter.letter != slot.correctLetter)
            {
                correct = false;
                wrongSlots.Add(slot);
            }
        }

        if (correct)
        {
            Debug.Log("BENAR!");
            isChecking = false;

            // 👉 nanti bisa lanjut level di sini
            return;
        }

        // 🚀 eject semua yang salah
        foreach (LetterSlot slot in wrongSlots)
        {
            StartCoroutine(WrongEffect(slot));
        }

        isChecking = false;
    }

    // =========================
    // WRONG EFFECT
    // =========================
    IEnumerator WrongEffect(LetterSlot slot)
    {
        Vector3 start = slot.transform.position;

        // wiggle
        for (int i = 0; i < 6; i++)
        {
            slot.transform.position = start + Vector3.left * 0.1f;
            yield return new WaitForSeconds(0.02f);

            slot.transform.position = start + Vector3.right * 0.1f;
            yield return new WaitForSeconds(0.02f);
        }

        slot.transform.position = start;

        // eject huruf
        if (slot.currentLetter != null)
        {
            slot.currentLetter.ReturnToStart();
            slot.ClearSlot();
        }
    }
}