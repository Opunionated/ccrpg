public class CharacterHighlightedEventData : EventData {

    public Character character;

    public CharacterHighlightedEventData (Character character)
    {
        this.character = character;
    }

    public override string ToString()
    {
        return "Character " + character.name + " Highlighted";
    }
}
