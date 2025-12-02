using System.Collections.Generic;
using UnityEngine;

public static class CardSpriteProvider
{
    private static bool initialized = false;
    private static Dictionary<string, Sprite> spriteMap;

    private static void EnsureLoaded()
    {
        if (initialized) return;

        spriteMap = new Dictionary<string, Sprite>();

        // Loads all sprites at: Assets/Resources/Sprites/WhiteCards/*
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/WhiteCards");

        foreach (var s in sprites)
        {
            if (!spriteMap.ContainsKey(s.name))
            {
                spriteMap.Add(s.name, s);
            }
        }

        initialized = true;
    }

    public static Sprite GetCardSprite(Card card, bool faceDown = false)
    {
        EnsureLoaded();

        string key = faceDown ? "back" : card.GetSpriteKey();

        if (spriteMap.TryGetValue(key, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"CardSpriteProvider: Missing sprite with key '{key}'");
        return null;
    }

    public static Sprite GetBackSprite()
    {
        EnsureLoaded();
        if (spriteMap.TryGetValue("back", out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }
}
