using System.Collections.Generic;
using UnityEngine;

public enum CardLocation { Set = -1, Deck = 0, Hand, Discard, Removed };

public class CardManager : MonoBehaviour {

    //Player Variables
    public int preferredHandSize = 5;
    public int maxHandSize = 7;
    public Card preparedCard;
    //Card Set Data
    public static Card[] set; //all cards in the game
    //Card Collections
    public List<Card> deck = new List<Card>();    //All owned cards

    //Initialise
    public void Init()
    {
        int c = 30;
        //Initialise Collection
        for (int i = 0; i <= c; i++)
        {
            deck.Add(new Card(  "Spark (n." + i + ")",
                                "Deal 1 Damage",
                                TargetType.Enemy,
                                new Condition[] { new Condition.MinHealth(0) },
                                new Action[] { new Action.Damage(GameManager.gameManager.GetPlayer(), 1, DamageType.Physical) }));
            i++;
            deck.Add(new Card(  "Agility (n." + i + ")",
                                "Draw 2 Cards",
                                TargetType.None,
                                new Condition[] { },
                                new Action[] { new Action.Draw(), new Action.Draw() }));
        }
        //Debug.Log("Card Manager Initialised!");
    }

    //Add a card
    public CardMovedEventData AddCard(Card card)
    {
        //Get 'from' location
        CardLocation from = card.location;
        //Edit card collection
        CardLocation to = card.location = CardLocation.Hand;
        return new CardMovedEventData(card, from, to);
    }

    //Play a card
    public CardMovedEventData RemoveCard(Card card)
    {
        //Get 'from' location
        CardLocation from = card.location;
        //Assign new location
        CardLocation to = card.location = CardLocation.Discard;
        return new CardMovedEventData(card, from, to);
    }

    //Draw card(s) to hand
    public CardMovedEventData DrawCard()
    {
        //Check if deck has any cards
        if (!(deck.Count > 0))
        {
            Debug.Log("Deck is empty, card not drawn!");
            return null;
        }
        //Check if hand size is at max
        if (!(GetHand().Count < maxHandSize))
        {
            Debug.Log("Hand is full, card not drawn!");
            return null;
        }
        //Add card from top of deck
        return AddCard(GetDeck()[0]);
    }

    //Shuffle a card collection
    public void ShuffleDeck()
    {
        System.Random r = new System.Random();
        Card card;

        for (int i = 0, c = deck.Count; i < c; i++)
        {
            int n = i + (int)(r.NextDouble() * (c - i));
            card = deck[n];
            deck[n] = deck[i];
            deck[i] = card;
        }
    }

    //Return collection
    private List<Card> GetCollection(CardLocation location)
    {
        return deck.FindAll(c => c.location == location);
    }

    //Get Cards in Deck
    public List<Card> GetDeck()
    {
        return GetCollection(CardLocation.Deck);
    }

    //Get Cards in Hand
    public List<Card> GetHand()
    {
        return GetCollection(CardLocation.Hand);
    }

    //Get Cards in Discard
    public List<Card> GetDiscard()
    {
        return GetCollection(CardLocation.Discard);
    }

    //Get Cards in Removed
    public List<Card> GetRemoved()
    {
        return GetCollection(CardLocation.Removed);
    }

    //Get prepared card
    public void SetPreparedCard(Card preparedCard)
    {
        this.preparedCard = preparedCard;
    }

    //Get prepared card
    public Card GetPreparedCard()
    {
        return preparedCard;
    }
}
