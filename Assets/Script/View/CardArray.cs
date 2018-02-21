using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardArray : MonoBehaviour {

    public GameObject deckObject;
    public GameObject discardObject;
    private List<GameObject> cardObjects = new List<GameObject>();

    public void Awake()
    {
        //Assign Listeners
        GameManager.gameManager.OnMatchBegin += RefreshCards;
        GameManager.gameManager.OnCardDrawn += AddCard;
        GameManager.gameManager.OnCardDiscard += RemoveCard;
        GameManager.gameManager.OnCardMoved += MoveCard;
    }

    public void OnDisable()
    {
        //Remove Listeners
        GameManager.gameManager.OnMatchBegin -= RefreshCards;
        GameManager.gameManager.OnCardDrawn -= AddCard;
        GameManager.gameManager.OnCardDiscard -= RemoveCard;
        GameManager.gameManager.OnCardMoved -= MoveCard;
    }

    public void Test(CardMovedEventData eD)
    {
        Debug.Log(eD.ToString());
    }

    //Re-order cardObjects by index
    public void UpdateCards()
    {
        //Remove all null CardObject references
        cardObjects.RemoveAll(o => o == null);
        //Get spacing of cards
        float spacing = Mathf.Min(new float[]{GetComponent<RectTransform>().rect.width / cardObjects.Count, 105});
        //Instantiate cardObject
        CardObject cardObject;
        //Iterate through, and order, all cardObjects
        for (int i = 0, j = cardObjects.Count; i < j; i++)
        {
            //Set card local position relative to the CardArray
            cardObject = cardObjects[i].GetComponent<CardObject>();
            //Get new position
            Vector2 position = new Vector2(transform.position.x + 50 - ((j * spacing) / 2) + (i * spacing), transform.position.y);
            cardObject.SetPosition(position);
            cardObject.SetRestingPosition(position);
        }
        //Debug.Log(this.name + " Cards Ordered");
    }

    //Refresh cards
    public void RefreshCards()
    {
        //Remove old cardObjects
        foreach (GameObject o in cardObjects)
        {
            Destroy(o);
        }
        //Clear list
        cardObjects.Clear();
        //Add new cards
        foreach (Card c in GameManager.gameManager.GetHand())
        {
            AddCard(new CardMovedEventData(c, c.location, CardLocation.Hand));
        }
    }

    //Add a card if possible, then order cardObjects
    public void AddCard(CardMovedEventData eventData)
    {
        //Add new CardObject to array
        if (eventData != null)
        {
            //Create New Card
            GameObject cardObject = CardObject.Create(eventData.card, transform, deckObject.transform.position);
            //Add to Array
            cardObjects.Insert(0, cardObject);
        }
        //Reorder cardObjects in hand
        UpdateCards();
    }

    //Remove a card if possible, then order cardObjects
    public void RemoveCard(CardMovedEventData eventData)
    {
        //Look through cards
        foreach (GameObject o in cardObjects)
        {
            //Locate GameObject with the right card attached
            if (o.GetComponent<CardObject>().GetCard() == eventData.card)
            {
                //Remove Card from Array
                cardObjects.Remove(o);
                //Remove the Card GameObject
                Destroy(o);
                //Reorder cardObjects in hand
                UpdateCards();
                //Debug.Log("Card \"" + eventData.card.name + "\" Removed");
                return;
            }
        }
        Debug.Log("Card " + eventData.card.name + " was not in our hand, Cannot remove it!");
    }

    //When card is moved arbitrarily
    public void MoveCard(CardMovedEventData eventData)
    {
        //If card was moved from hand, remove it's object
        if (eventData.from == CardLocation.Hand)
        {
            RemoveCard(eventData);
            return;
        }
        //If card was moved to the hand, add it's object
        if (eventData.to == CardLocation.Hand)
        {
            AddCard(eventData);
            return;
        }
    }
}
