public class CharacterStateEventData : EventData {

    public Character character;

    public CharacterStateEventData(Character character)
    {
        this.character = character;
    }

    public override string ToString()
    {
        return "Character " + character + " has entered a new state"; //TODO fix
    }
}
