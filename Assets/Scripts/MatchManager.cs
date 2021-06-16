using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UNO;
public class MatchManager : MonoBehaviour
{
    private int totalNumberOfPlayers;
    private int startingNumberOfCards;
    private DeckObj deck;
    private List<IPlayer> players;
    private int currentPlayer;
    private const float xDist = 6.0f; // 6
    private bool isreverse;
    private int intialCards;
    private int penaltyCards;
    private bool abilityEffects;
    private bool colorPanel;
    public void AddTwoCards()
    {
        AbilityEffects = false;
        int nextPlayer = isreverse ? currentPlayer - 1 == -1 ? totalNumberOfPlayers -1 : currentPlayer - 1 
            : currentPlayer + 1 == totalNumberOfPlayers ? 0:currentPlayer + 1;
        
        //int valid = players[nextPlayer].CountValid(stackOfCards.Peek());
        int valid = PlayerHelper.CountValid(stackOfCards.Peek(), players[nextPlayer]);
        int final = stackOfCards.Count;
        penaltyCards += (final - intialCards) * 2;
        if (valid < 1)
        {
            while (penaltyCards > 0)
            {
                //players[nextPlayer].AddCard(GetNewCard());
                PlayerHelper.AddCard(GetNewCard(), players[nextPlayer]);
                penaltyCards--;
            }
            //players[nextPlayer].Arrange();
            PlayerHelper.Arrange(players[nextPlayer]);
            penaltyCards = 0;
            SkipPlayer();
            AbilityEffects = true;
        }
        
    }

    public void ReverseGame()
    {
        int diff = stackOfCards.Count - intialCards;
        while (diff > 0)
        {
            isreverse = !isreverse;
            diff--;
        }
        AbilityEffects = true;
    }

    public void SkipPlayer()
    {
        int diff = stackOfCards.Count - intialCards;
        while (diff > 0)
        {
            if (isreverse)
            {
                currentPlayer--;
                if (currentPlayer == -1)
                    currentPlayer = totalNumberOfPlayers - 1;
            }
            else
            {
                currentPlayer++;
                if (currentPlayer == totalNumberOfPlayers)
                    currentPlayer = 0;
            }
            diff--;
        }
        AbilityEffects = true;
    }

    public void AddDrawFour()
    {
        int nextPlayer = isreverse ? currentPlayer - 1 == -1 ? totalNumberOfPlayers - 1 : currentPlayer - 1
            : currentPlayer + 1 == totalNumberOfPlayers ? 0 : currentPlayer + 1;
        //int valid = players[nextPlayer].CountValid(stackOfCards.Peek());
        int valid = PlayerHelper.CountValid(stackOfCards.Peek(), players[nextPlayer]);
        int final = stackOfCards.Count;
        penaltyCards += (final - intialCards) * 4;
        AbilityEffects = false;
        if (valid < 1)
        {
            while (penaltyCards > 0)
            {
                //players[nextPlayer].AddCard(GetNewCard());
                PlayerHelper.AddCard(GetNewCard(), players[nextPlayer]);
                penaltyCards--;
            }
            //players[nextPlayer].Arrange();
            PlayerHelper.Arrange(players[nextPlayer]);
            penaltyCards = 0;
            //players[currentPlayer].ChooseColor();
            PlayerHelper.ChooseColor();
            SkipPlayer();
            AbilityEffects = true;
        }
    }

    public void WildCard()
    {
        //players[currentPlayer].ChooseColor();
        PlayerHelper.ChooseColor();
        AbilityEffects = true;
    }

    private const float yDist = 3.5f; // 3.5
    private Vector3 deckPos = new Vector3(-4.2f, 1.5f,-0.15f);
    private int countActive;

    private bool isRunning;
    private Stack<Card> stackOfCards;
    private List<MeshRenderer> ringCards;
    public Material mat;

    private static MatchManager _instance;
    public static MatchManager Instance { get => _instance; }
    public bool AbilityEffects { get => abilityEffects; set => abilityEffects = value; }
    public Card GetTopCard()
    {
        if (stackOfCards.Count == 0)
            return null;
        return stackOfCards.Peek();
    }

    public Card GetNewCard()
    {
        return deck.Deck.GetTopCard();
    }

    private void Intialize()
    {
        totalNumberOfPlayers = 4;
        startingNumberOfCards = 8;
        currentPlayer = 0;
        players = new List<IPlayer>();

        //Creating a board
        GameObject ring = Instantiate<GameObject>(PrefabManager.Instance.collectionRingPrefab);
        ringCards = new List<MeshRenderer>();
        for(int i = 0; i < ring.transform.childCount; i++)
        {
            ringCards.Add(ring.transform.GetChild(i).GetComponent<MeshRenderer>());
        }


        for (int i = 0; i < totalNumberOfPlayers; i++)
        {
            GameObject player = new GameObject();
            IPlayer currentPlayer =i==0 ? player.AddComponent<HumanPlayer>() : player.AddComponent<AIPlayer>();
            Vector3 pos = i % 2 == 0 ? new Vector3(transform.position.x,
                transform.position.y + i < 2 ? -yDist : yDist) : new Vector3(transform.position.x + i < 2 ? -xDist : xDist,
                transform.position.y);
            player.transform.position = pos;
            player.transform.SetParent(this.transform);
            player.name = "Player" + (i);
            currentPlayer.Intialize(startingNumberOfCards, i + 1, "Player" + i + 1);
            Quaternion rot = Quaternion.Euler(Vector3.forward * (i%2 ==0? 0f:-180f));
            player.transform.rotation = rot;
            players.Add(currentPlayer);
        }

        //Creating Deck GameObj
        GameObject d = Instantiate<GameObject>(PrefabManager.Instance.DeckPrefab);
        deck = d.AddComponent<DeckObj>();
        d.transform.SetParent(this.transform);
        Vector3 posi = this.transform.position + deckPos;
        deck.transform.position = posi;
        deck.name = "Deck";
        deck.Distribute(startingNumberOfCards, players);

        for (int i = 0; i < totalNumberOfPlayers; i++)
        {
            //players[i].Arrange();
            PlayerHelper.Arrange(players[i]);
        }

        stackOfCards = new Stack<Card>();
    }

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance.gameObject);

        _instance = this;
    }

    void startMatch()
    {
        HideColorPanel();
        isRunning = true;
        currentPlayer = 0;
        Card firstCard = deck.Deck.GetTopCard();
        while (firstCard.Group != CardGroup.Number)
            firstCard = deck.Deck.GetTopCard();
        stackOfCards.Push(firstCard);
        ringCards[0].material = PrefabManager.Instance.GetMaterials(firstCard.Color, firstCard.Group, firstCard.Type);
        ringCards[0].gameObject.SetActive(true);
        countActive++;
    }

    private void Start()
    {
        Intialize();
        startMatch();
    }

    private void GameLoop()
    {
        if (currentPlayer >= totalNumberOfPlayers)
            currentPlayer = 0;
        else if (currentPlayer < 0)
            currentPlayer = totalNumberOfPlayers - 1;

        if (!players[currentPlayer].IsMyTurn)
        {
            Debug.Log(stackOfCards.Peek().ToString());
            players[currentPlayer].IsMyTurn = true;
            players[currentPlayer].CanShowCard = true;
            intialCards = stackOfCards.Count;
        }

        if (players[currentPlayer].IsturnCompleted)
        {
            Debug.Log($"<color=yellow> {players[currentPlayer].PlayerName} </color>");
            players[currentPlayer].IsMyTurn = false;
            players[currentPlayer].IsturnCompleted = false;
            players[currentPlayer].CanShowCard = false;
            stackOfCards.Peek().ability();
            if (isreverse)
                currentPlayer--;
            else
                currentPlayer++;
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            if(!colorPanel)
                GameLoop();
        }
    }

    public void AddCardInGameStack(Card card)
    {
        stackOfCards.Push(card);
        if (countActive < ringCards.Count)
        {
            ringCards[countActive].material = PrefabManager.Instance.GetMaterials(card.Color, card.Group, card.Type);
            ringCards[countActive].gameObject.SetActive(true);
            countActive++;
        }
        else
        {
            countActive = ringCards.Count;
            changeRingMaterialsOrder(PrefabManager.Instance.GetMaterials(card.Color, card.Group, card.Type));
        }
    }

    private void changeRingMaterialsOrder(Material mat)
    {
        for(int i = 0; i < countActive-1; i++)
        {
            ringCards[i].material = ringCards[i + 1].material;
        }
        ringCards[countActive - 1].material = mat;
        Vector3 rot = ringCards[countActive - 1].transform.rotation.eulerAngles;
        rot.x = Random.Range(100f, 150f);
        ringCards[countActive - 1].gameObject.transform.rotation = Quaternion.Euler(rot);
    }

    public void ShowColorPanel()
    {
        PrefabManager.Instance.canvas.alpha = 1;
        PrefabManager.Instance.canvas.interactable = true;
        colorPanel = true;
    }

    public void HideColorPanel()
    {
        PrefabManager.Instance.canvas.alpha = 0;
        PrefabManager.Instance.canvas.interactable = false;
        colorPanel = false;
    }
}
