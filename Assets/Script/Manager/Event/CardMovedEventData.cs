public class CardMovedEventData : EventData {

    public Card card;
    public CardLocation from;
    public CardLocation to;

    public CardMovedEventData(Card card, CardLocation from, CardLocation to)
    {
        this.card = card;
        this.from = from;
        this.to = to;
    }

    public override string ToString()
    {
        return ("CardMovedEventData: Card " + this.card.name + " was moved from " + from.ToString() + " to " + to.ToString());
    }
}
