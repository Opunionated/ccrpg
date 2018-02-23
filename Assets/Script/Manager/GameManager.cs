using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Singleton Instance
    public static GameManager gameManager { get; private set; }
    //Card Manager
    private static CardManager cardManager;
    //Character Manager
    private static CharacterManager characterManager;
    //The Prefab for our cards
    public GameObject cardPrefab;
    //Actions
    public delegate void OnGamePhaseAction();
    public delegate void OnCardMovedAction(CardMovedEventData eventData);
    public delegate void OnCardPlayedAction(CardActionEventData eventData);
    public delegate void OnCharacterHighlightedAction(CharacterHighlightedEventData eventData);
    public delegate void OnCharacterDamagedAction(CharacterDamagedEventData eventData);
    public delegate void OnCharacterHealedAction(CharacterHealedEventData eventData);
    public delegate void OnCharacterStateAction(CharacterStateEventData eventData);
    //Events
    public event OnGamePhaseAction OnMatchBegin;
    public event OnGamePhaseAction OnPlayerIdle;
    public event OnGamePhaseAction OnReaper;
    public event OnCharacterStateAction OnCharacterDeath;
    public event OnCardMovedAction OnCardDrawn;
    public event OnCardMovedAction OnCardDiscard;
    public event OnCardMovedAction OnCardMoved;
    public event OnCardPlayedAction OnCardPrepared;
    public event OnCardPlayedAction OnCardPlayed;
    public event OnCardPlayedAction OnCardUnprepared;
    public event OnCharacterHighlightedAction OnCharacterHighlighted;
    public event OnCharacterDamagedAction OnCharacterDamaged;
    public event OnCharacterHealedAction OnCharacterHealed;

    public void Awake()
    {
        //Setup Singleton reference (to make the GameManager globally accessible)
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        //Get Character Manager
        characterManager = gameObject.GetComponent<CharacterManager>();
        //Initialise CharacterManager
        characterManager.Init();
        //Get Card Manager
        cardManager = gameObject.GetComponent<CardManager>();
        //Initialise CardManager
        cardManager.Init();

        BeginMatch(); //TODO Refactor
    }

    public void BeginMatch()
    {
        //Shuffle deck
        cardManager.ShuffleDeck();
        //Draw your hand
        for (int i = 0; i < cardManager.preferredHandSize; i++)
        {
            if (cardManager.DrawCard() == null) break;
        }
        if (OnMatchBegin != null) { OnMatchBegin(); }
    }

    private void Idle()
    {
        Reap();
        if (OnReaper != null) { OnReaper(); }
        if (OnPlayerIdle != null) { OnPlayerIdle(); }
    }

    private void Reap()
    {
        List<Character> characters = characterManager.GetCharacters();
        for (int i = characters.Count - 1; i >= 0; i--)
        {
            Character c = characters[i];
            if (c.dead)
            {
                Destroy(c.gameObject);
                if (OnCharacterDeath != null) { OnCharacterDeath(new CharacterStateEventData(c)); }
                characterManager.GetCharacters().Remove(c);
                //TODO remove character from character list?
            }
        }
    }

    public void DrawCard()
    {
        CardMovedEventData eventData = cardManager.DrawCard();
        if (OnCardDrawn != null) { OnCardDrawn(eventData); }
    }

    public void DiscardCard(Card card)
    {
        CardMovedEventData eventData = cardManager.RemoveCard(card);
        if (OnCardDiscard != null) { OnCardDiscard(eventData); }
    }

    public void PrepareCard(Card card)
    {
        //Get valid targets
        Character[] validTargets = card.Prepare(characterManager.GetCharacters());
        if (validTargets == null)
        {
            Debug.Log("Card conditions have not been met!");
            return;
        }
        //Highlight Targets
        characterManager.SetValidTargets(validTargets);
        //Prepare card
        cardManager.SetPreparedCard(card);
        //characterManager.HighlightCharacters(card.Prepare(characterManager.GetCharacters()));
        if (OnCardPrepared != null) OnCardPrepared(new CardActionEventData(card, validTargets));
    }

    public void UnprepareCard(Card card)
    {
        //Remove Highlights
        cardManager.SetPreparedCard(null);
        //characterManager.HighlightCharacters(null);
        if (OnCardUnprepared != null) OnCardUnprepared(new CardActionEventData(card, null));
        Idle();
    }

    public bool PlayCard(Card card)
    {
        //Get Targets, returns null if card is invalid
        Character[] targets = characterManager.GetCurrentTargets();
        //Play card if valid
        if (card.targetType == TargetType.None || targets != null)
        {
            card.Play(targets);
            CardMovedEventData eventData = cardManager.RemoveCard(card);
            if (OnCardPlayed != null) OnCardPlayed(new CardActionEventData(card, targets));
            if (OnCardMoved != null) OnCardMoved(eventData);
            UnprepareCard(card);
            return true;
        }
        else
        {
            UnprepareCard(card);
            return false;
        }
    }

    public void TargetCharacter(Character target)
    {
        characterManager.SetCurrentTargets(new Character[]{ target });
    }

    public void ClearTargetCharacter()
    {
        characterManager.SetCurrentTargets(null);
    }

    public void DamageCharacters(Character source, Character[] targets, int power, DamageType damageType)
    {
        if (targets == null || targets.Length <= 0) return; //TODO debug
        foreach (Character c in targets)
        {
            CharacterDamagedEventData eventData = c.TakeDamage(source, power);
            if (OnCharacterDamaged != null) OnCharacterDamaged(eventData);
        }
    }

    public void HealCharacters(Character source, Character[] targets, int power)
    {
        if (targets == null || targets.Length <= 0) return; //TODO debug
        foreach (Character c in targets)
        {
            CharacterHealedEventData eventData = c.GainLife(source, power);
            if (OnCharacterHealed != null) OnCharacterHealed(eventData);
        }
    }

    public Player GetPlayer()
    {
        return characterManager.GetPlayer();
    }

    public List<Card> GetHand()
    {
        return cardManager.GetHand();
    }
}
