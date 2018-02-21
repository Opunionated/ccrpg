using System.Collections.Generic;

public class CardActionEventData {

    public Card card;
    public Character[] validTargets;

    public CardActionEventData(Card card, Character[] validTargets)
    {
        this.card = card;
        this.validTargets = validTargets;
    }

    public override string ToString()
    {
        return "Card: \"" + card.name + " played";
    }
}