using System;
using System.Collections.Generic;
using System.Linq;
using _Game;
using ChuongCustom;
using UnityEngine;
using Random = System.Random;

public class Spawner : Singleton<Spawner>
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private CardSO cardData;

    public List<Card> Cards { get; private set; }

    public List<CardType> Spawn(int amountType)
    {
        var listType = GetRandomListType(amountType);
        Cards = new();

        int id = 0;
        foreach (var cardType in listType)
        {
            var card = Instantiate(cardPrefab, transform);
            card.Setup(new CardInfo()
            {
                CardID = id,
                CardType = cardType,
                CardImage = cardData[cardType]
            });
            Cards.Add(card);
            id++;
        }

        return listType;
    }

    private List<CardType> GetRandomListType(int amount)
    {
        int count = 0;
        var list = new List<CardType>();
        foreach (var cardType in Enum.GetValues(typeof(CardType)).Cast<CardType>())
        {
            if(count >= amount) break;
            list.Add(cardType);
            list.Add(cardType);
        }

        Shuffle(list);
        Shuffle(list);
        return list;
    }

    public static void Shuffle<T>(List<T> list)
    {
        var rng = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
