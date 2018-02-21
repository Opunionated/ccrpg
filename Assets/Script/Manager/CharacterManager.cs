using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {

    private const int SLOTS = 7;
    public List<Character> characters = new List<Character>();
    private Character[] currentTargets;
    public Character[] validTargets = new Character[SLOTS];
    public GameObject[] slots = new GameObject[SLOTS];

    //Initialise CharacterManager
    public void Init()
    {
        //Debug.Log("Character Manager Initialised!");
    }

    //Get player
    public Player GetPlayer()
    {
        return characters.Find(c => c is Player) as Player;
    }

    //Get all characters
    public List<Character> GetCharacters()
    {
        return characters;
    }

    //Set Valid Targets
    public void SetValidTargets(Character[] validTargets)
    {
        if (validTargets != null)
        {
            this.validTargets = validTargets;
        }
        else validTargets = new Character[0];
    }

    //Target a character
    public void SetCurrentTargets(Character[] currentTargets)
    {
        this.currentTargets = currentTargets;
    }

    //Get target
    public Character[] GetCurrentTargets()
    {
        return currentTargets;
    }
}
