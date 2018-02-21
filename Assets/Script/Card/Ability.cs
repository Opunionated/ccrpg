using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

    public Event trigger;
    public abstract void Invoke(EventData eventData);
    public abstract override string ToString();

    protected Ability(event e)
    {
        //Simple test line for Github
        e += Invoke;
    }

    public class GainLife : Ability
    {
        public GainLife(event abilityEvent) : base(abilityEvent)
        {

        }

        public override void Invoke(EventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }
    }
}
