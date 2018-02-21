using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Character character;
    public GameObject highlightDecal;
    public GameObject targetDecal;
    public bool highlighted { get; private set; }
    public bool targetted { get; private set; }

    public void Start()
    {
        GameManager.gameManager.OnCardPrepared += Highlight;
        GameManager.gameManager.OnCardUnprepared += Reset;

        GameManager.gameManager.OnCardPrepared += Refresh;
        GameManager.gameManager.OnCardUnprepared += Refresh;
    }

    public void OnDisable()
    {
        GameManager.gameManager.OnCardPrepared -= Highlight;
        GameManager.gameManager.OnCardUnprepared -= Reset;

        GameManager.gameManager.OnCardPrepared -= Refresh;
        GameManager.gameManager.OnCardUnprepared -= Refresh;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Target(true);
        if (targetted)
        {
            GameManager.gameManager.TargetCharacter(character);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Target(false);
        GameManager.gameManager.ClearTargetCharacter();
    }

    public void Reset(CardActionEventData eventData)
    {
        highlighted = targetted = false;
        Refresh();
    }

    public void Highlight(CardActionEventData eventData)
    {
        if (character && Array.Exists(eventData.validTargets, c => c == character)) //TODO refactor
        {
            highlighted = true;
        }
    }

    public void Target(bool toggle)
    {
        if (toggle && highlighted == true && character)
        {
            targetted = true;
        }
        else
        {
            targetted = false;
        }
        Refresh();
    }

    public void Refresh()
    {
        highlightDecal.SetActive(false);
        targetDecal.SetActive(false);

        if (targetted)
        {
            targetDecal.SetActive(targetted = true);
        }
        else if (highlighted)
        {
            highlightDecal.SetActive(highlighted = true);
        }
    }

    public void Refresh(CardActionEventData eventData)
    {
        Refresh();
    }
}
