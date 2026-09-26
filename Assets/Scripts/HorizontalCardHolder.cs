using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField] private int startingHandSize = 5;
    public List<Card> cards;

    [Header("Deck System")]
    [SerializeField] private List<CardDataSO> startingDeck = new List<CardDataSO>();
    public List<CardDataSO> drawPile = new List<CardDataSO>();
    public List<CardDataSO> discardPile = new List<CardDataSO>();
    
    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    // State Machine
    void Start()
    {
        rect = GetComponent<RectTransform>();
        cards = new List<Card>();

        drawPile = new List<CardDataSO>(startingDeck);

        ShuffleDeck();

        for (int i = 0; i < startingHandSize; i++)
        {
            DrawCard();
        }

        StartCoroutine(Frame());

        IEnumerator Frame()
        {
            yield return new WaitForSecondsRealtime(.1f);
            UpdateVisualIndexes();
        }
    }

    public void DrawCard()
    {
        if (drawPile.Count == 0)
        {
            if (discardPile.Count > 0)
            {
                ShuffleDiscardIntoDraw();
            }
            else
            {
                return; 
            }
        }

        CardDataSO drawnCardData = drawPile[0];
        drawPile.RemoveAt(0);

        GameObject newSlot = Instantiate(slotPrefab, transform);
        Card newCard = newSlot.GetComponentInChildren<Card>();
        
        // Inject the data into the newly spawned card
        newCard.Setup(drawnCardData);
        
        SetupCardListeners(newCard, cards.Count);
        cards.Add(newCard);
        
        UpdateVisualIndexes();
    }

    public void DiscardCard(Card cardToDiscard)
    {
        if (cards.Contains(cardToDiscard))
        {
            cards.Remove(cardToDiscard);
            
            // Re-add the card's data to the discard pile
            discardPile.Add(cardToDiscard.cardData); 
            
            RemoveCardListeners(cardToDiscard);
            Destroy(cardToDiscard.transform.parent.gameObject);
            
            UpdateVisualIndexes();
        }
    }

    public void ShuffleDiscardIntoDraw()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < drawPile.Count; i++)
        {
            CardDataSO temp = drawPile[i];
            int randomIndex = UnityEngine.Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }
    }

    private void SetupCardListeners(Card card, int index)
    {
        card.PointerEnterEvent.AddListener(CardPointerEnter);
        card.PointerExitEvent.AddListener(CardPointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
        card.name = index.ToString();
    }

    private void RemoveCardListeners(Card card)
    {
        card.PointerEnterEvent.RemoveListener(CardPointerEnter);
        card.PointerExitEvent.RemoveListener(CardPointerExit);
        card.BeginDragEvent.RemoveListener(BeginDrag);
        card.EndDragEvent.RemoveListener(EndDrag);
    }

    private void UpdateVisualIndexes()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].cardVisual != null)
                cards[i].cardVisual.UpdateIndex(transform.childCount);
        }
    }

    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }


    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0,selectedCard.selectionOffset,0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;

    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (hoveredCard != null)
            {
                DiscardCard(hoveredCard);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (Card card in cards)
            {
                card.Deselect();
            }
        }

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < cards.Count; i++)
        {

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        //Updated Visual Indexes
        UpdateVisualIndexes();
    }

}