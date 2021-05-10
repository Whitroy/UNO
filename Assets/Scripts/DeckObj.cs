using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UNO;
public class DeckObj : MonoBehaviour
{
    private Deck deck;

    public Deck Deck { get => deck;}

    private void Awake()
    {
        deck = new Deck();
    }

    public void Distribute(int numOfCards,List<IPlayer> players)
    {
        Deck.Distribute(numOfCards, players);
    }

    void Start()
    {
    }
    void Update()
    {
        
    }
}
