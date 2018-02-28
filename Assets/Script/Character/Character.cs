using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// (Hugh) If this is the main character page then I would recommend some changes
public class Character : MonoBehaviour {
// (Hugh) These stats are fine
    public int maxHealth = 3;
    public int health = 3;
    public int armour = 1;
    public int speed = 1;
    public bool dead = false; //Characters can be marked for death, without yet being removed from the scene by the 'Reaper'
    
// (Hugh) This may just be me, but I beleive that these calculations should be done in a seperate script the character page 
//  should only have the character stats with other scripts calling upon this page to change things like health and damage calculations.
//  Also by seperating these calculations we can eaily call upon them for monster attacks, traps, event dmg so on and so forth.
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
        
  //(Hugh) This could live here, but again i feel it would be beter as a seperate script to apply to all monsters and ecncounters we code.  
        //Mark dead
        if (health <= 0) dead = true;
        return new CharacterDamagedEventData(source, this, power, damage, dead);
    }
}
