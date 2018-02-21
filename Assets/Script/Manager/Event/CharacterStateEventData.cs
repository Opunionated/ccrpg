using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateEventData : MonoBehaviour {

    public Character character;

    public CharacterStateEventData(Character character)
    {
        this.character = character;
    }
}
