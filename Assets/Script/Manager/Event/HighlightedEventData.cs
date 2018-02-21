using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHighlightedEventData {

    public Character character;

    public CharacterHighlightedEventData (Character character)
    {
        this.character = character;
    }

    public override string ToString()
    {
        return "Character " + character.name + " Highlighted";
    }
}
