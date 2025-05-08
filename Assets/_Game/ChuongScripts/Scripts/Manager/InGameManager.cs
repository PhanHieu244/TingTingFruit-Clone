
using System.Collections.Generic;
using System.Linq;
using _Game.ChuongScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChuongCustom
{
    public class InGameManager : Singleton<InGameManager>
    {
        [SerializeField] public int PriceToPrice = 1;

        private Card waitCard;

        private List<CardType> _cardAvailable;

        private void Start()
        {
            ScoreManager.Reset();
            var amount = Data.Player.Coin switch
            {
                1 => 4,
                2 => 6,
                3 => 8,
                _ => 10,
            };
            _cardAvailable = Spawner.Instance.Spawn(amount).DistinctBy((type => type)).ToList();
            Card.OnOpenCard += OnOpenCard;
            TargetFill.Instance.SetTarget(_cardAvailable.Count);
        }

        private void OnDestroy()
        {
            transform.DOKill();
            Card.OnOpenCard -= OnOpenCard;
        }

        #region WINLOSE

        [Button]
        public void Win()
        {
            Manager.ScreenManager.OpenScreen(ScreenType.Result);
            //todo:
        }

        [Button]
        public void Lose()
        {
            Manager.ScreenManager.OpenScreen(ScreenType.Lose);
            //todo:
        }

        [Button]
        public void BeforeLose()
        {
            Manager.ScreenManager.OpenScreen(ScreenType.BeforeLose);
            //todo:
        }

        public void Retry()
        {
            //retry
            //todo:
        }

        public void Continue()
        {
            //continue

            //todo:
        }

        #endregion

        public void OnOpenCard(Card card)
        {
            if (waitCard == null)
            {
                OpenAndWait(card);
                return;
            }

            if (card.CardID == waitCard.CardID)
            {
                CloseCard(card);
                return;
            }
            
            if (waitCard.CardType == card.CardType)
            {
                Match(waitCard, card);
                return;
            }
            
            UnMatch(waitCard, card);
        }

        public void OpenAndWait(Card card)
        {
            waitCard = card;
            card.OpenCard();
        }
        
        public void CloseCard(Card card)
        {
            waitCard = null;
            card.CloseCard();
        }

        
        private void Match(Card firstCard, Card secondCard)
        {
            waitCard = null;
            _cardAvailable.Remove(firstCard.CardType);
            DOTween.Sequence(transform)
                .Append(secondCard.OpenCard())
                .Append(firstCard.Match())
                .Join(secondCard.Match())
                .Append(UpdateProgress())
                .OnComplete(CheckWin);
        }

        public Tween UpdateProgress()
        {
            return TargetFill.Instance.SetProgress(_cardAvailable.Count);
        }
        
        public void CheckWin()
        {
            if (_cardAvailable.Count > 0) return;
            Data.Player.Coin++;
            Win();
        }

        private void UnMatch(Card firstCard, Card secondCard)
        {
            waitCard = null;
            DOTween.Sequence(transform)
                .Append(secondCard.OpenCard())
                .AppendInterval(0.5f)
                .Append(secondCard.CloseCard())
                .Join(firstCard.CloseCard());
        }

        public void ShowHint()
        {
            if (_cardAvailable.Count <= 0) return;
            var random = Random.Range(0, _cardAvailable.Count);
            var cardTypeHint = _cardAvailable[random];
            var cards = Spawner.Instance.Cards.Where(card => card.CardType == cardTypeHint);
            foreach (var card in cards)
            {
                card.Hint();
            }
        }
    }
}