using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public int maxHealth = 3;
    public int health = 3;
    public int armour = 1;
    public int speed = 1;
    public bool dead = false; //Characters can be marked for death, without yet being removed from the scene by the 'Reaper'

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
}
