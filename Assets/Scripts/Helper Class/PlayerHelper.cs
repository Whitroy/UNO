using System.Collections.Generic;
using UnityEngine;
namespace UNO
{
    public static class PlayerHelper
    {
        public static void Arrange(IPlayer player)
        {
            Transform playerTrans = player.GetTransform();
            int size = player.MyCards.Count;
            float gap = 0.45f;
            float z = -0.04f;
            if (player.MyCards.Count < 9)
            {
                gap = 0.45f;
                z = -0.04f;
            }
            else if (player.MyCards.Count < 10)
            {
                gap = 0.42f;
            }
            else if (player.MyCards.Count < 12)
            {
                gap = 0.4f;
                z = -0.035f;
            }
            int l = size / 2;
            Vector3 cardRot;
            float cardRotGap = 5f;
            float startingPos = playerTrans.position.x - l * gap;
            if (player.MyId == 1)
            {
                cardRot = new Vector3(90f + (l - 1) * cardRotGap, -90f, 90f);
            }
            else if (player.MyId == 2)
            {
                startingPos = playerTrans.position.y - l * gap;
                cardRot = new Vector3(180f + (l - 1) * cardRotGap, -90, -90);
            }
            else if (player.MyId == 3)
            {
                cardRot = new Vector3(90f - (l - 1) * cardRotGap, -90, -90);
            }
            else
            {
                startingPos = playerTrans.position.y - l * gap;
                cardRot = new Vector3(-180f - (l - 1) * cardRotGap, -90, 90);
            }
            for (int i = 0; i < size; i++)
            {
                Vector3 pos;
                Vector3 r = cardRot;
                if (player.MyId == 1 || player.MyId == 3)
                {
                    pos = new Vector3(startingPos + gap * i, 0f, z * (i + 1));
                    if (player.MyId == 1)
                        r.x -= cardRotGap * i;
                    else
                        r.x += cardRotGap * i;
                }
                else
                {
                    pos = new Vector3(player.MyId == 2 ? 1f : -1f, startingPos + gap * i, z * (i + 1));
                    if (player.MyId == 2)
                        r.x -= cardRotGap * i;
                    else
                        r.x += cardRotGap * i;
                }
                player.MyCards[i].transform.localPosition = pos;
                player.MyCards[i].transform.rotation = Quaternion.Euler(r);
            }
        }

        public static void AddCard(Card card, IPlayer player)
        {
            if (PrefabManager.Instance == null)
            {
                Debug.LogError("PrefabManager is not intialized!");
            }
            if (PrefabManager.Instance.cardPrefab == null)
            {
                Debug.LogError("card Prefab not found!");
            }
            CardObj obj = UnityEngine.GameObject.Instantiate<CardObj>(PrefabManager.Instance.cardPrefab);
            obj.Card = card;
            obj.transform.SetParent(player.GetTransform());
            player.MyCards.Add(obj);
        }

        public static void RemoveCard(IPlayer player)
        {
            Stack<Card> cards = player.StackOfCard;
            int i = player.CardLeft - 1;
            for (int j = 0; j < player.CardLeft; j++)
            {
                if (cards.Count != 0 && cards.Peek() == player.MyCards[j].Card)
                {
                    CardObj temp = player.MyCards[j];
                    player.MyCards[j] = player.MyCards[i];
                    player.MyCards[i] = temp;
                    j = 0;
                    i--;
                    cards.Pop();
                }
            }
            for (int j = player.CardLeft - 1; j > i; j--)
            {
                Debug.Log(j);
                CardObj card = player.MyCards[j];
                player.MyCards.RemoveAt(j);
                MatchManager.Instance.AddCardInGameStack(card.Card);
                UnityEngine.GameObject.Destroy(card.gameObject);
            }
            player.CardLeft = i + 1;
        }

        public static int CountValid(Card card, IPlayer player)
        {
            int countValid = 0;
            for (int i = 0; i < player.CardLeft; i++)
            {
                if (!player.MyCards[i].Card.IsSelected && CardChecker.CheckValidCard(card, player.MyCards[i].Card, player))
                {
                    countValid++;
                }
            }
            return countValid;
        }

        public static int ShowValidCard(Card card, IPlayer player)
        {
            int countValid = 0;
            for (int i = 0; i < player.CardLeft; i++)
            {
                if (!player.MyCards[i].Card.IsSelected && CardChecker.CheckValidCard(card, player.MyCards[i].Card, player))
                {
                    player.MyCards[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    player.MyCards[i].IsValid = true;
                    countValid++;
                }
            }
            player.CanChoose = countValid > 0 ? true : false;
            if (MatchManager.Instance.AbilityEffects)
                MatchManager.Instance.AbilityEffects = false;
            card.Color = card.Group == CardGroup.Wild ? CardColor.None : card.Color;
            return countValid;
        }

        public static void ResetValid(IPlayer player)
        {
            for (int i = 0; i < player.CardLeft; i++)
            {
                if (player.MyCards[i].IsValid && !player.MyCards[i].IsSelected)
                {
                    player.MyCards[i].gameObject.GetComponent<MeshRenderer>().material.color = MatchManager.Instance.mat.color;
                    player.MyCards[i].IsValid = false;
                }
            }
        }

        public static void ChooseColor()
        {
            MatchManager.Instance.ShowColorPanel();
        }

    }
}