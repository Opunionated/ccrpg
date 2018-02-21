using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {

    public string name;
    public string description;
    //public Sprite art;
    public TargetType targetType;
    public Condition[] conditions { get; set; }
    public Action[] actions { get; set; }
    public CardLocation location;

    public Card(string name, string description, TargetType targetType, Condition[] conditions, Action[] actions, CardLocation location = CardLocation.Deck)
    {
        this.name = name;
        this.description = description;
        this.targetType = targetType;
        this.conditions = conditions;
        this.actions = actions;
        this.location = location;
    }

    public Character[] Prepare(List<Character> targets)
    {
        List<Character> validTargets = new List<Character>();
        //Filter out targetType
        switch (targetType) //TODO implement all target types
        {
            case TargetType.None:
                //Action requires no target, therefore it is valid but the list is empty
                return validTargets.ToArray();
            case TargetType.Enemy:
                validTargets = targets.FindAll(t => t is Enemy);
                break;
            case TargetType.Ally:
                validTargets = targets.FindAll(t => t is Player); //TODO Implement Allies?
                break;
            default:
                break;
        }
        //Ignore 'dead' minions
        validTargets = validTargets.FindAll(t => !t.dead);
        //Validate targets by conditions
        foreach (Condition c in conditions)
        {
            validTargets = c.Validate(validTargets);
            //If any condition returns null, card is not even preparable!
            if (validTargets == null) return null;
        }
        //Return valid targets
        //If valid targets == empty, card may still be playable (such as a Draw Action) 
        return validTargets.ToArray();
    }

    public void Play(Character[] targets)
    {
        foreach (Action a in actions)
        {
            a.Invoke(targets);
            //Debug.Log(e);
        }
    }
}
