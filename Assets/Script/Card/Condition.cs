using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition {

    public abstract List<Character> Validate(List<Character> targets);

    public class MinHealth : Condition
    {
        public int minHealth = 0;

        public MinHealth(int minHealth)
        {
            this.minHealth = minHealth;
        }

        public override List<Character> Validate(List<Character> targets)
        {
            return targets.FindAll(t => t.health >= minHealth);
        }
    }
}
