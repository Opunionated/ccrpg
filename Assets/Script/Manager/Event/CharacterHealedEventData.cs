public class CharacterHealedEventData  {

    public Character source;
    public Character target;
    public int healAmount;
    public int lifeHealed;

    public CharacterHealedEventData(Character source, Character target, int healAmount, int lifeHealed)
    {
        this.source = source;
        this.target = target;
        this.healAmount = healAmount;
        this.lifeHealed = lifeHealed;
    }

    public override string ToString()
    {
        return target.name + " healed for  " + lifeHealed + " from " + target.name;
    }
}
