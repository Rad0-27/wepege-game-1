using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LetterLibrary : MonoBehaviour
{
    public Dictionary<char, Sprite> normalLetters = new Dictionary<char, Sprite>();
    public Dictionary<char, Sprite> shadowLetters = new Dictionary<char, Sprite>();

    void Awake()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/Letter");

        // ✅ SORT BERDASARKAN ANGKA NAMA FILE
        var sorted = sprites.OrderBy(s =>
        {
            int number;
            int.TryParse(s.name, out number);
            return number;
        }).ToArray();

        char currentChar = 'A';

        for (int i = 0; i < sorted.Length; i += 2)
        {
            if (i + 1 >= sorted.Length) break;

            // genap = normal
            normalLetters[currentChar] = sorted[i];

            // ganjil = shadow
            shadowLetters[currentChar] = sorted[i + 1];

            currentChar++;
        }
    }

    public Sprite GetNormal(char c)
    {
        c = char.ToUpper(c);

        if (normalLetters.ContainsKey(c))
            return normalLetters[c];

        return null;
    }

    public Sprite GetShadow(char c)
    {
        c = char.ToUpper(c);

        if (shadowLetters.ContainsKey(c))
            return shadowLetters[c];

        return null;
    }
}