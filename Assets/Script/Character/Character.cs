using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public int maxHealth = 5;
    public int health = 3;
    public int armour = 1;
    public int speed = 1;
    public bool dead = false; //Characters can be marked for death, without yet being removed from the scene by the 'Reaper'
    public Ability[] abilities = null;

    public void Start()
    {
        abilities = new Ability[] { new Ability.HealSelf(EventID.CharacterDied, this, 3) };
    }

    public void OnDisable()
    {
        RemoveAbilities();
    }

    public void OnDestroy()
    {
        RemoveAbilities();
    }

    private void RemoveAbilities()
    {
        foreach (Ability a in abilities)
        {
            a.SetEnabled(false);
        }
        abilities = null;
    }

    public int CalculateArmourDamage(int power)
    {
        return Mathf.Clamp(power, 0, armour);
    }

    public int CalculateDamage(int power)
    {
        return Mathf.Clamp(power - CalculateArmourDamage(power), 0, power); //TODO Magic number
    }

    public CharacterDamagedEventData TakeDamage(Character source, int power)
    {
        //Calculate Damages
        int armourDamage = CalculateArmourDamage(power);
        int damage = CalculateDamage(power);
        //Apply Damage
        armour -= armourDamage;
        health -= damage;
        //Mark dead
        if (health <= 0) dead = true;
        return new CharacterDamagedEventData(source, this, power, damage, dead);
    }

    public CharacterHealedEventData GainLife(Character source, int power)
    {
        //Calculate the amount of life to heal
        int healValue = Mathf.Max(new int[] { maxHealth - health, power });
        //Apply Heal
        health += healValue;
        return new CharacterHealedEventData(source, this, power, healValue);
    }
}
