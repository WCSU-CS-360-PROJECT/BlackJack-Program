using System.Collections.Generic;
using UnityEngine;

public class CardSpriteSheetScript : MonoBehaviour
{

    public Sprite[] CardSprites;
    public Sprite backSprite;
    Dictionary<int, Sprite> cardSpriteMap = new Dictionary<int, Sprite>();

    void Start()
    {
        backSprite = Resources.Load<Sprite>("Sprites/back");

        // Setup the card sprite map using 1-52 as keys
        // SUIT: CLOVER (C)
        cardSpriteMap[1] = Resources.Load<Sprite>("Sprites/WhiteCards/CA");
        cardSpriteMap[2] = Resources.Load<Sprite>("Sprites/WhiteCards/C2");
        cardSpriteMap[3] = Resources.Load<Sprite>("Sprites/WhiteCards/C3");
        cardSpriteMap[4] = Resources.Load<Sprite>("Sprites/WhiteCards/C4");
        cardSpriteMap[5] = Resources.Load<Sprite>("Sprites/WhiteCards/C5");
        cardSpriteMap[6] = Resources.Load<Sprite>("Sprites/WhiteCards/C6");
        cardSpriteMap[7] = Resources.Load<Sprite>("Sprites/WhiteCards/C7");
        cardSpriteMap[8] = Resources.Load<Sprite>("Sprites/WhiteCards/C8");
        cardSpriteMap[9] = Resources.Load<Sprite>("Sprites/WhiteCards/C9");
        cardSpriteMap[10] = Resources.Load<Sprite>("Sprites/WhiteCards/C10");
        cardSpriteMap[11] = Resources.Load<Sprite>("Sprites/WhiteCards/CJ");
        cardSpriteMap[12] = Resources.Load<Sprite>("Sprites/WhiteCards/CQ");
        cardSpriteMap[13] = Resources.Load<Sprite>("Sprites/WhiteCards/CK");

        // SUIT: DIAMOND (D)
        cardSpriteMap[14] = Resources.Load<Sprite>("Sprites/WhiteCards/DA");
        cardSpriteMap[15] = Resources.Load<Sprite>("Sprites/WhiteCards/D2");
        cardSpriteMap[16] = Resources.Load<Sprite>("Sprites/WhiteCards/D3");
        cardSpriteMap[17] = Resources.Load<Sprite>("Sprites/WhiteCards/D4");
        cardSpriteMap[18] = Resources.Load<Sprite>("Sprites/WhiteCards/D5");
        cardSpriteMap[19] = Resources.Load<Sprite>("Sprites/WhiteCards/D6");
        cardSpriteMap[20] = Resources.Load<Sprite>("Sprites/WhiteCards/D7");
        cardSpriteMap[21] = Resources.Load<Sprite>("Sprites/WhiteCards/D8");
        cardSpriteMap[22] = Resources.Load<Sprite>("Sprites/WhiteCards/D9");
        cardSpriteMap[23] = Resources.Load<Sprite>("Sprites/WhiteCards/D10");
        cardSpriteMap[24] = Resources.Load<Sprite>("Sprites/WhiteCards/DJ");
        cardSpriteMap[25] = Resources.Load<Sprite>("Sprites/WhiteCards/DQ");
        cardSpriteMap[26] = Resources.Load<Sprite>("Sprites/WhiteCards/DK");

        // SUIT: HEART (H)
        cardSpriteMap[27] = Resources.Load<Sprite>("Sprites/WhiteCards/HA");
        cardSpriteMap[28] = Resources.Load<Sprite>("Sprites/WhiteCards/H2");
        cardSpriteMap[29] = Resources.Load<Sprite>("Sprites/WhiteCards/H3");
        cardSpriteMap[30] = Resources.Load<Sprite>("Sprites/WhiteCards/H4");
        cardSpriteMap[31] = Resources.Load<Sprite>("Sprites/WhiteCards/H5");
        cardSpriteMap[32] = Resources.Load<Sprite>("Sprites/WhiteCards/H6");
        cardSpriteMap[33] = Resources.Load<Sprite>("Sprites/WhiteCards/H7");
        cardSpriteMap[34] = Resources.Load<Sprite>("Sprites/WhiteCards/H8");
        cardSpriteMap[35] = Resources.Load<Sprite>("Sprites/WhiteCards/H9");
        cardSpriteMap[36] = Resources.Load<Sprite>("Sprites/WhiteCards/H10");
        cardSpriteMap[37] = Resources.Load<Sprite>("Sprites/WhiteCards/HJ");
        cardSpriteMap[38] = Resources.Load<Sprite>("Sprites/WhiteCards/HQ");
        cardSpriteMap[39] = Resources.Load<Sprite>("Sprites/WhiteCards/HK");

        // SUIT: SPADE (S)
        cardSpriteMap[40] = Resources.Load<Sprite>("Sprites/WhiteCards/SA");
        cardSpriteMap[41] = Resources.Load<Sprite>("Sprites/WhiteCards/S2");
        cardSpriteMap[42] = Resources.Load<Sprite>("Sprites/WhiteCards/S3");
        cardSpriteMap[43] = Resources.Load<Sprite>("Sprites/WhiteCards/S4");
        cardSpriteMap[44] = Resources.Load<Sprite>("Sprites/WhiteCards/S5");
        cardSpriteMap[45] = Resources.Load<Sprite>("Sprites/WhiteCards/S6");
        cardSpriteMap[46] = Resources.Load<Sprite>("Sprites/WhiteCards/S7");
        cardSpriteMap[47] = Resources.Load<Sprite>("Sprites/WhiteCards/S8");
        cardSpriteMap[48] = Resources.Load<Sprite>("Sprites/WhiteCards/S9");
        cardSpriteMap[49] = Resources.Load<Sprite>("Sprites/WhiteCards/S10");
        cardSpriteMap[50] = Resources.Load<Sprite>("Sprites/WhiteCards/SJ");
        cardSpriteMap[51] = Resources.Load<Sprite>("Sprites/WhiteCards/SQ");
        cardSpriteMap[52] = Resources.Load<Sprite>("Sprites/WhiteCards/SK");


    }

    // When coding func to get sprite by 1-52, do math on A to get its val to be 1/11,
    // and set any (x % 12) > 10 to be 10.

    Sprite GetCardByNum(int num)
    {
        return cardSpriteMap[num];
    }

}
