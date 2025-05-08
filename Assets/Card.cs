using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite closeCard;
    [SerializeField] private Color hintColor;
    [SerializeField] private float flipDuration = 0.2f;
    [SerializeField] private float hintDuration = 0.2f;

    public static event Action<Card> OnOpenCard; 

    private CardInfo _cardInfo;
    private Color _defaultColor;
    
    private Vector3 faceFlip = new(0f, 90f, 0f);
    private Vector3 faceUp = new(0f, 180f, 0f);
    private Vector3 faceDown = new(0f, 0f, 0f);
    
    public int CardID => _cardInfo.CardID;

    public CardType CardType => _cardInfo.CardType;
    
    public bool isProgress;
    public bool isRemove;

    private void OnEnable()
    {
        CloseCard().Complete(true);
        _defaultColor = cardImage.color;
    }

    public void Setup(CardInfo cardInfo)
    {
        _cardInfo = cardInfo;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isProgress || isRemove) return;
        OnOpenCard?.Invoke(this);
    }
    
    public Tween OpenCard()
    {
        if (isProgress) return DOTween.Sequence();
        isProgress = true;
        transform.DOComplete(true);
        return DOTween.Sequence(transform)
            .Append(cardImage.transform.DOLocalRotate(faceFlip, flipDuration * 0.5f))
            .AppendCallback(() =>
            {
                cardImage.sprite = _cardInfo.CardImage;
            })
            .Append(cardImage.transform.DOLocalRotate(faceUp, flipDuration * 0.5f))
            .AppendCallback(() => isProgress = false);
    }
    
    public Tween CloseCard()
    {
        if (isProgress) DOTween.Sequence();
        isProgress = true;
        transform.DOComplete(true);
        return DOTween.Sequence(transform)
            .Append(cardImage.transform.DOLocalRotate(faceFlip, flipDuration * 0.5f))
            .AppendCallback(() =>
            {
                cardImage.sprite = closeCard;
            })
            .Append(cardImage.transform.DOLocalRotate(faceDown, flipDuration * 0.5f))
            .AppendCallback(() =>
            {
                isProgress = false;
            });
    }

    public Tween Match()
    {
        isRemove = true;
        return cardImage.transform.DOScale(Vector3.one * 0.02f, 0.22f).SetEase(Ease.InQuad)
            .OnComplete((() =>
            {
                cardImage.gameObject.SetActive(false);
            }));
    }

    public Tween Hint()
    {
        return DOTween.Sequence(transform)
            .Append(cardImage.DOColor(hintColor, hintDuration))
            .Append(cardImage.DOColor(_defaultColor, hintDuration))
            .SetEase(Ease.InOutQuad)
            .SetLoops(8, LoopType.Yoyo);
    }
}

public class CardInfo
{
    public int CardID;
    public CardType CardType;
    public Sprite CardImage;
}

public enum CardType
{
    A = 0, 
    B, 
    C,
    D,
    E, 
    F,
    G,
    H,
    K, 
    T
}
