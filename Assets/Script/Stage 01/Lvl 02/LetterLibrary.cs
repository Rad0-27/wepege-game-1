using UnityEngine;
using System.Collections.Generic;

public class LetterLibrary : MonoBehaviour
{
    public Dictionary<char, Sprite> letters = new Dictionary<char, Sprite>();

    void Awake()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Art/Letter");

        foreach (Sprite s in sprites)
        {
            char c = char.ToUpper(s.name[0]);

            if (!letters.ContainsKey(c))
            {
                letters.Add(c, s);
            }
        }
    }

    public Sprite GetSprite(char c)
    {
        c = char.ToUpper(c);

        if (letters.ContainsKey(c))
            return letters[c];

        return null;
    }
}