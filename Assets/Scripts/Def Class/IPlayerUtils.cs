using System.Collections.Generic;
using UnityEngine;

namespace UNO
{
    public interface IPlayerUtils
    {
        Transform GetTransform();
        bool IsMyTurn { get; set; }
        bool IsturnCompleted { get; set; }
        bool CanShowCard { get; set; }
        int CardLeft { get; set; }
        int MyId { get;}
        string PlayerName { get;}
        List<CardObj> MyCards { get; }
        Stack<Card> StackOfCard { get; }
        bool CanChoose { get; set; }

        void Intialize(int cardLeft, int id, string name);
    }
}
