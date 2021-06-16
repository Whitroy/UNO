using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UNO;

public class AIPlayer : HumanPlayer
{
    private void Play()
    {
        

        StartCoroutine(drop());

        
       
    }

    IEnumerator drop()
    {
        int validCount = PlayerHelper.ShowValidCard(MatchManager.Instance.GetTopCard(), this);

        yield return new WaitForSeconds(2);

        Debug.Log($"<color=orange>{PlayerName} :- {validCount} </color>");
        if (validCount > 0)
        {
            ChooseAllValidOnes();
            ThrowCard();
        }
        else
        {
            Debug.Log($"<color=red>AI picked </color>");
            PickCard();
        }
        PlayerHelper.ResetValid(this);
    }

    private void ChooseAllValidOnes()
    {
        List<CardObj> tempCard = new List<CardObj>();
        for(int i = 0; i < CardLeft; i++)
        {
            if (MyCards[i].IsValid)
            {
                tempCard.Add(MyCards[i]);
            }
        }

        tempCard[0].IsSelected = true;
        ChooseCard();
        for(int i=1;i< tempCard.Count; i++)
        {
            if (tempCard[i].IsValid)
            {
                StackOfCard.Push(tempCard[i].Card);
                tempCard[i].IsValid = false;
            }
        }

    }

    private void Update()
    {
        if (IsMyTurn)
        {
            if (CanShowCard)
                CanChoose = true;
            if (CanChoose && Input.GetKeyDown(KeyCode.Space))
                Play();
        }
    }
}
