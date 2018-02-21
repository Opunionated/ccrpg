public class CharacterDamagedEventData : EventData {

    public Character source;
    public Character target;
    public int damageDealt;
    public int damageTaken;
    public bool characterDied;

    public CharacterDamagedEventData(Character source, Character target, int damageDealt, int damageTaken, bool characterDied)
    {
        this.source = source;
        this.target = target;
        this.damageDealt = damageDealt;
        this.damageTaken = damageTaken;
        this.characterDied = characterDied;
    }

    public override string ToString()
    {
        return target.name + "took " + damageTaken + "from " + target.name;
    }
}