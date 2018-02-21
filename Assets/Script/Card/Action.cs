using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action {

    public abstract void Invoke(Character[] targets);
    public abstract override string ToString();

    //Damage Effect
    public class Damage : Action {

        public Character source;
        public int power;
        public DamageType damageType;

        public Damage(Character source, int power, DamageType damageType)
        {
            this.source = source;
            this.power = power;
            this.damageType = damageType;
        }

        public override void Invoke(Character[] targets)
        {
            GameManager.gameManager.DamageCharacters(source, targets, power, damageType);
        }

        public override string ToString()
        {
            return "Action: Attack from " + source + " for " + power + " damage";
        }
    }

    //Draw Effect
    public class Draw : Action
    {
        public Draw() {}

        public override void Invoke(Character[] targets)
        {
            GameManager.gameManager.DrawCard();
        }

        public override string ToString()
        {
            return "Action: Draw a card";
        }
    }
}
