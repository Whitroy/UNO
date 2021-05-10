using System.Collections.Generic;
using UnityEngine;
namespace UNO
{
    public class HumanPlayer : MonoBehaviour, IPlayer
    {
        private bool _isMyTurn;
        private bool _isturnCompleted;
        private bool _canShowCard;
        private int _cardLeft;
        private int _myId;
        private string _playerName;
        private List<CardObj> _myCards;
        private Stack<Card> _stackOfCard;
        private bool _canChoose;

        public bool IsMyTurn { get => _isMyTurn; set => _isMyTurn = value; }
        public bool IsturnCompleted { get => _isturnCompleted; set => _isturnCompleted = value; }
        public bool CanShowCard { get => _canShowCard; set => _canShowCard = value; }
        public int CardLeft { get => _cardLeft; set => _cardLeft = value; }
        public int MyId { get => _myId; private set => _myId = value; }
        public string PlayerName { get => _playerName; private set => _playerName = value; }
        public List<CardObj> MyCards { get => _myCards; private  set => _myCards= value; }
        public Stack<Card> StackOfCard { get => _stackOfCard; private  set => _stackOfCard= value; }
        public bool CanChoose { get => _canChoose; set => _canChoose= value; }
        public void Caught()
        {
            
        }

        public void ChooseCard()
        {
            for (int i = 0; i < CardLeft; i++)
            {
                if (MyCards[i].IsValid && MyCards[i].Card.IsSelected)
                {
                    StackOfCard.Push(MyCards[i].Card);
                    MyCards[i].IsValid = false;
                    MyCards[i].gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    PlayerHelper.ResetValid(this);
                    CanChoose = false;
                    PlayerHelper.ShowValidCard(StackOfCard.Peek(),this);
                    break;
                }
            }
        }

        public Transform GetTransform()
        {
            return this.transform;
        }

        public void Intialize(int cardLeft, int id, string name)
        {
            this.IsMyTurn = false;
            this.CardLeft = cardLeft;
            this.MyId = id;
            this.PlayerName = name;
            this.MyCards = new List<CardObj>();
            this.StackOfCard = new Stack<Card>();
        }

        public void PickCard()
        {
            PlayerHelper.ResetValid(this);
            if (MyCards.Count > 15)
                return;
            PlayerHelper.AddCard(MatchManager.Instance.GetNewCard(),this);
            CardLeft++;
            CanChoose = false;
            CanShowCard = false;
            PlayerHelper.Arrange(this);
            IsturnCompleted = true;
        }

        public void Shout()
        {

        }

        public void ShowCard()
        {
            PlayerHelper.ResetValid(this);
            Card topCard = MatchManager.Instance.GetTopCard();
            PlayerHelper.ShowValidCard(topCard,this);
            CanShowCard = false;
        }

        public void ThrowCard()
        {
            PlayerHelper.RemoveCard(this);
            PlayerHelper.ResetValid(this);
            CanChoose = false;
            CanShowCard = false;
            PlayerHelper.Arrange(this);
            IsturnCompleted = true;
        }

        
        void Update()
        {
            if (IsMyTurn)
            {
                if (CanShowCard)
                    ShowCard();
                if (CanChoose)
                    ChooseCard();
                if (Input.GetKeyDown(KeyCode.T) && StackOfCard.Count > 0)
                    ThrowCard();
                if (Input.GetKeyDown(KeyCode.P))
                    PickCard();
            }
        }
    }
}