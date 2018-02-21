using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //UI References
    public Text cardName;
    public Image cardArt;
    public Text cardDescription;
    public Image cardBackground;
    //Arrow Reference
    public GameObject targetPointer;
    private static GameObject[] targetArrow; //TODO ugly
    //Card reference
    private Card card;
    //Position to return to
    private CardObject selectedCard;
    private int siblingIndex;
    private static bool preparing = false;
    private Vector2 restingPosition;
    private Vector2 defaultSize;
    private Vector2 activePosition;
    //Speed
    private const int SPEED = 2500; //TODO seems a bit high I guess?

    //Card creator
    public static GameObject Create(Card card, Transform parent, Vector2 position)
    {
        //Create new GameObject
        GameObject cardContainer = Instantiate(GameManager.gameManager.cardPrefab, Vector3.zero, Quaternion.identity);
        //Make the Card GameObject a child of the parent
        cardContainer.transform.SetParent(parent);
        //Card Object ref
        CardObject cardObject = cardContainer.GetComponent<CardObject>();
        //Set card reference to new card
        cardObject.SetCard(card);
        //Set Scale
        cardObject.transform.localScale = Vector2.one;
        //Set Position to Deck position
        cardObject.transform.position = position;
        //Set Position
        cardObject.SetPosition(position);
        //Instantiate TargetArrow if none exists
        if (targetArrow == null || targetArrow.Length == 0)
        {
            Transform holder = GameObject.Instantiate(new GameObject(), parent).transform;
            holder.name = "TargetArrow";
            holder.SetSiblingIndex(0);
            int n = 15; //Magic number
            targetArrow = new GameObject[n];
            for (int i = 0; i < n; i++)
            {
                targetArrow[i] = Instantiate(cardObject.targetPointer, holder);
                targetArrow[i].SetActive(false);
            }
        }
        return cardContainer;
    }

    public void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, activePosition, SPEED * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Highlight card image
        HighlightCard(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Unhighlight card image
        HighlightCard(false);
    }

    //when the card is clicked
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Check for Left click OnBeginDrag
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            preparing = true;
            ScaleCard(1); //TODO Magic number
            cardBackground.transform.localPosition = Vector2.zero; //TODO CODE DUPLICATION!!!
            GameManager.gameManager.PrepareCard(card);
        }
    }

    //while the card is clicked
    public void OnDrag(PointerEventData eventData)
    {
        var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Check for Left click OnDrag
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (card.targetType == TargetType.Ally || card.targetType == TargetType.Enemy)
            {
                float xP = Mathf.Lerp(restingPosition.x, eventData.position.x, 0.1f); //TODO magic numbers
                float yP = transform.parent.position.y + (Mathf.Clamp(eventData.position.y - transform.parent.position.y, 5, 100));
                SetPosition(new Vector2(xP, yP)); //TODO remove magic number
                //Create target
                DrawArrow(eventData.position);
            }
            else SetPosition(eventData.position);
        }
    }

    //when the card is un-clicked
    public void OnEndDrag(PointerEventData eventData)
    {
        preparing = false;
        //Check for Left click OnEndDrag
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Remove TargetArrow
            RemoveArrow();
            //Play card if possible
            if (transform.position.y >= restingPosition.y + 100)
            {
                if (GameManager.gameManager.PlayCard(card)) return;
            }
            //Return card to CardArray
            SetPosition(restingPosition);
            GameManager.gameManager.UnprepareCard(card);
        }
    }

    //Set card
    public void SetCard(Card card)
    {
        this.card = card;
        cardName.text = card.name;
        cardDescription.text = card.description;
        name = card.name;
    }

    //Get card
    public Card GetCard()
    {
        return card;
    }

    public void HighlightCard(bool toggle)
    {
        if (toggle && selectedCard == null)
        {
            if (!preparing)
            {
                siblingIndex = transform.GetSiblingIndex();
                ScaleCard(1.2f); //TODO Magic number
                cardBackground.transform.localPosition = Vector2.up * (cardBackground.rectTransform.rect.height * 0.1f); //TODO Magic number
                transform.SetAsLastSibling();
            }
            selectedCard = this;
        }
        else if (!toggle && selectedCard == this)
        {
            ScaleCard(1); //TODO Magic number
            cardBackground.transform.localPosition = Vector2.zero;
            selectedCard = null;
            if (transform.GetSiblingIndex() != siblingIndex) transform.SetSiblingIndex(siblingIndex);
        }
    }

    //Draw Arrow
    public void DrawArrow(Vector2 to)
    {
        //Get three Bezier points
        Vector2 p0 = transform.position;
        Vector2 p1 = new Vector2(transform.position.x, transform.position.y + ((to.y - transform.position.y) * 1.5f)); //TODO Magic Number(s)
        Vector3 p2 = to;

        //Bezier curve
        for (int i = 0, j = targetArrow.Length; i < j; i++)
        {
            float d = ((float)i / (j - 1));
            targetArrow[i].transform.position = Vector3.Lerp(Vector3.Lerp(p0, p1, d), Vector3.Lerp(p1, p2, d), d);
            targetArrow[i].SetActive(true);
        }
    }

    //Remove Arrow
    public void RemoveArrow()
    {
        if (targetArrow != null)
        {
            foreach (GameObject o in targetArrow)
            {
                o.SetActive(false);
            }
        }
    }

    //Private ScaleCard
    private void ScaleCard(float scalar)
    {
        transform.GetComponent<RectTransform>().localScale = new Vector2(scalar, scalar); //TODO reset
    }

    //Set ActivePosition to move to
    public void SetPosition(Vector2 activePostiion)
    {
        this.activePosition = activePostiion;
    }

    //Set RestingPosition
    public void SetRestingPosition(Vector2 restingPosition)
    {
        this.restingPosition = restingPosition;
    }
}
