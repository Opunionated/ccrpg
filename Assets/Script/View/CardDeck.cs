using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    public void DrawCard()
    {
        //Draw card and add it to CardArray
        GameManager.gameManager.DrawCard();
    }
}
