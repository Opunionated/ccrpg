using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

    public Trigger trigger;
    public abstract void Invoke();
    public abstract override string ToString();

    public enum Trigger { DamageTaken, DamageDealt, Deathrattle }; //Grow list

    protected Ability(Trigger t)
    {
        switch (t)
        {
            case (Trigger.DamageTaken):
                break;
            case (Trigger.DamageDealt):
                break;
            case (Trigger.Deathrattle):
                break;
        }
    }

    public class GainLife : Ability
    {
        public GainLife(Trigger t) : base(t)
        {

        }

        public override void Invoke()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }
    }
}
