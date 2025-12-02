using System.Collections.Generic;
using UnityEngine;

public class CardSpriteSheetScript : MonoBehaviour
{

    public Sprite[] CardSprites;
    public Sprite backSprite;
    Dictionary<string, Sprite> cardSpriteMap = new Dictionary<string, Sprite>();

    void Start()
    {
        cardSpriteMap = new Dictionary<string, Sprite>();
        backSprite = Resources.Load<Sprite>("Sprites/BackCard.png");

        // Setup the card sprite map
        // SUIT: CLOVER (C)
        cardSpriteMap.Add("CloverA", Resources.Load<Sprite>("Sprites/WhiteCards/CA"));
        cardSpriteMap.Add("Clover2", Resources.Load<Sprite>("Sprites/WhiteCards/C2"));
        cardSpriteMap.Add("Clover3", Resources.Load<Sprite>("Sprites/WhiteCards/C3"));
        cardSpriteMap.Add("Clover4", Resources.Load<Sprite>("Sprites/WhiteCards/C4"));
        cardSpriteMap.Add("Clover5", Resources.Load<Sprite>("Sprites/WhiteCards/C5"));
        cardSpriteMap.Add("Clover6", Resources.Load<Sprite>("Sprites/WhiteCards/C6"));
        cardSpriteMap.Add("Clover7", Resources.Load<Sprite>("Sprites/WhiteCards/C7"));
        cardSpriteMap.Add("Clover8", Resources.Load<Sprite>("Sprites/WhiteCards/C8"));
        cardSpriteMap.Add("Clover9", Resources.Load<Sprite>("Sprites/WhiteCards/C9"));
        cardSpriteMap.Add("Clover10", Resources.Load<Sprite>("Sprites/WhiteCards/C10"));
        cardSpriteMap.Add("CloverJ", Resources.Load<Sprite>("Sprites/WhiteCards/CJ"));
        cardSpriteMap.Add("CloverQ", Resources.Load<Sprite>("Sprites/WhiteCards/CQ"));
        cardSpriteMap.Add("CloverK", Resources.Load<Sprite>("Sprites/WhiteCards/CK"));

        // SUIT: HEART (H)
        cardSpriteMap.Add("HeartA", Resources.Load<Sprite>("Sprites/WhiteCards/HA"));
        cardSpriteMap.Add("Heart2", Resources.Load<Sprite>("Sprites/WhiteCards/H2"));
        cardSpriteMap.Add("Heart3", Resources.Load<Sprite>("Sprites/WhiteCards/H3"));
        cardSpriteMap.Add("Heart4", Resources.Load<Sprite>("Sprites/WhiteCards/H4"));
        cardSpriteMap.Add("Heart5", Resources.Load<Sprite>("Sprites/WhiteCards/H5"));
        cardSpriteMap.Add("Heart6", Resources.Load<Sprite>("Sprites/WhiteCards/H6"));
        cardSpriteMap.Add("Heart7", Resources.Load<Sprite>("Sprites/WhiteCards/H7"));
        cardSpriteMap.Add("Heart8", Resources.Load<Sprite>("Sprites/WhiteCards/H8"));
        cardSpriteMap.Add("Heart9", Resources.Load<Sprite>("Sprites/WhiteCards/H9"));
        cardSpriteMap.Add("Heart10", Resources.Load<Sprite>("Sprites/WhiteCards/H10"));
        cardSpriteMap.Add("HeartJ", Resources.Load<Sprite>("Sprites/WhiteCards/HJ"));
        cardSpriteMap.Add("HeartQ", Resources.Load<Sprite>("Sprites/WhiteCards/HQ"));
        cardSpriteMap.Add("HeartK", Resources.Load<Sprite>("Sprites/WhiteCards/HK"));

        // SUIT: DIAMOND (D)
        cardSpriteMap.Add("DiamondA", Resources.Load<Sprite>("Sprites/WhiteCards/DA"));
        cardSpriteMap.Add("Diamond2", Resources.Load<Sprite>("Sprites/WhiteCards/D2"));
        cardSpriteMap.Add("Diamond3", Resources.Load<Sprite>("Sprites/WhiteCards/D3"));
        cardSpriteMap.Add("Diamond4", Resources.Load<Sprite>("Sprites/WhiteCards/D4"));
        cardSpriteMap.Add("Diamond5", Resources.Load<Sprite>("Sprites/WhiteCards/D5"));
        cardSpriteMap.Add("Diamond6", Resources.Load<Sprite>("Sprites/WhiteCards/D6"));
        cardSpriteMap.Add("Diamond7", Resources.Load<Sprite>("Sprites/WhiteCards/D7"));
        cardSpriteMap.Add("Diamond8", Resources.Load<Sprite>("Sprites/WhiteCards/D8"));
        cardSpriteMap.Add("Diamond9", Resources.Load<Sprite>("Sprites/WhiteCards/D9"));
        cardSpriteMap.Add("Diamond10", Resources.Load<Sprite>("Sprites/WhiteCards/D10"));
        cardSpriteMap.Add("DiamondJ", Resources.Load<Sprite>("Sprites/WhiteCards/DJ"));
        cardSpriteMap.Add("DiamondQ", Resources.Load<Sprite>("Sprites/WhiteCards/DQ"));
        cardSpriteMap.Add("DiamondK", Resources.Load<Sprite>("Sprites/WhiteCards/DK"));

        // SUIT: SPADE (S)
        cardSpriteMap.Add("SpadeA", Resources.Load<Sprite>("Sprites/WhiteCards/SA"));
        cardSpriteMap.Add("Spade2", Resources.Load<Sprite>("Sprites/WhiteCards/S2"));
        cardSpriteMap.Add("Spade3", Resources.Load<Sprite>("Sprites/WhiteCards/S3"));
        cardSpriteMap.Add("Spade4", Resources.Load<Sprite>("Sprites/WhiteCards/S4"));
        cardSpriteMap.Add("Spade5", Resources.Load<Sprite>("Sprites/WhiteCards/S5"));
        cardSpriteMap.Add("Spade6", Resources.Load<Sprite>("Sprites/WhiteCards/S6"));
        cardSpriteMap.Add("Spade7", Resources.Load<Sprite>("Sprites/WhiteCards/S7"));
        cardSpriteMap.Add("Spade8", Resources.Load<Sprite>("Sprites/WhiteCards/S8"));
        cardSpriteMap.Add("Spade9", Resources.Load<Sprite>("Sprites/WhiteCards/S9"));
        cardSpriteMap.Add("Spade10", Resources.Load<Sprite>("Sprites/WhiteCards/S10"));
        cardSpriteMap.Add("SpadeJ", Resources.Load<Sprite>("Sprites/WhiteCards/SJ"));
        cardSpriteMap.Add("SpadeQ", Resources.Load<Sprite>("Sprites/WhiteCards/SQ"));
        cardSpriteMap.Add("SpadeK", Resources.Load<Sprite>("Sprites/WhiteCards/SK"));
    }

    // When coding func to get sprite by 1-52, do math on A to get its val to be 1/11,
    // and set any (x % 12) > 10 to be 10.


}
