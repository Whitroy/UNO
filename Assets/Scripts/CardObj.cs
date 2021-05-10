using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UNO;
using TMPro;
public class CardObj : MonoBehaviour
{
    private Card card;
    private bool isValid;
    private Material deckMat;
    private Material cardMat;
    private void Start()
    {
        if (card == null)
            Debug.LogError("Card is not being intialized!");
        gameObject.name = card.ToString();
        deckMat = PrefabManager.Instance.GetMaterials(card.Color, card.Group, card.Type, true);
        cardMat = PrefabManager.Instance.GetMaterials(card.Color, card.Group, card.Type);
        GetComponent<MeshRenderer>().material = cardMat;
    }
    public Card Card { get => card; set => card = value; }
    public bool IsValid { get => isValid; set => isValid = value; }
    public bool IsSelected { get => card.IsSelected; set =>card.IsSelected = value;}

    private void OnMouseDown()
    {
        if (isValid)
        {
            card.IsSelected = !IsSelected;
        }
    }

    public void ChangeUI(bool isInDeck = false)
    {
        if (!isInDeck)
        {
            GetComponent<MeshRenderer>().material = cardMat;
        }
        else
        {
            GetComponent<MeshRenderer>().material = deckMat;
        }
    }
}
