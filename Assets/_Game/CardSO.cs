using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace _Game
{
    [CreateAssetMenu(fileName = "cardData", menuName = "ScriptableObjects/CardData", order = 1)]
    public class CardSO : SerializedScriptableObject
    {
        [OdinSerialize] public Dictionary<CardType, Sprite> CardData;

        [Button]
        private void Validate()
        {
            CardData ??= new Dictionary<CardType, Sprite>();
            foreach (var cardType in Enum.GetValues(typeof(CardType)).Cast<CardType>())
            {
                if(CardData.ContainsKey(cardType)) continue;
                CardData.Add(cardType, null);
            }
        }

        public Sprite this[CardType cardType] => CardData[cardType];
    }
}

