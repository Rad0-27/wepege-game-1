using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LetterEntry
{
    public string letter;
    public GameObject prefab;
}

public class LetterDatabase : MonoBehaviour
{
    public List<LetterEntry> letters;

    private Dictionary<string, GameObject> letterDict;

    void Awake()
    {
        letterDict = new Dictionary<string, GameObject>();

        foreach (var entry in letters)
        {
            string key = entry.letter.ToUpper();

            if (!letterDict.ContainsKey(key))
            {
                letterDict.Add(key, entry.prefab);
            }
        }
    }

    public GameObject GetLetter(string letter)
    {
        letter = letter.ToUpper();

        if (letterDict.ContainsKey(letter))
        {
            return letterDict[letter];
        }

        return null;
    }
}