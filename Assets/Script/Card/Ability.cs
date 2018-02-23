using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

    public EventID eventID { get; private set; }
    public abstract void Invoke(EventData eventData);
    public abstract override string ToString();

    protected Ability(EventID eventID)
    {
        this.eventID = eventID;
        SetEnabled(true);
    }

    public void SetEnabled(bool enabled)  //TODO not DRY at all, this switch statement will be monumental once all the triggers are put in place
    {
        switch (eventID)
        {
            case EventID.CharacterDied:
                if (enabled)
                {
                    GameManager.gameManager.OnCharacterDeath += Invoke;
                }
                else
                {
                    GameManager.gameManager.OnCharacterDeath -= Invoke;
                }
                break;
            case EventID.CharacterDamaged:
                if (enabled)
                {
                    GameManager.gameManager.OnCharacterDamaged += Invoke;
                }
                else
                {
                    GameManager.gameManager.OnCharacterDamaged -= Invoke;
                }
                break;
        }
    }

    public class HealSelf : Ability {

        private Character self;
        private int healAmount;

        public HealSelf(EventID eventID, Character self, int healAmount) : base(eventID)
        {
            this.self = self;
            this.healAmount = healAmount;
        }

        public override void Invoke(EventData eventData)
        {
            GameManager.gameManager.HealCharacters(self, new Character[] { self }, healAmount);
        }

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }
    }
}
