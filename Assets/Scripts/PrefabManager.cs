using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UNO;
using UnityEngine.UIElements;
public class PrefabManager : MonoBehaviour
{
    private static PrefabManager instance;
    public static PrefabManager Instance { get => instance; set => instance = value; }
    public GameObject DeckPrefab { get => deckPrefab; }

    public CardObj cardPrefab;
    public GameObject deckPrefab;
    public GameObject collectionRingPrefab;

    public Dictionary<string, Material> cardMaterials;
    public List<Material> materials;

    public CanvasGroup canvas;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        DeckPrefab.GetComponent<MeshRenderer>().material = GetMaterials(CardColor.Blue, CardGroup.Action, CardType.DrawFour, true);
    }
    public Material GetMaterials(CardColor color, CardGroup group, CardType type, bool deck = false)
    {
        if (deck)
            return materials[materials.Count - 1];
        Material mat = null;
        int i;
        int j = 0;
        switch (color)
        {
            case CardColor.Red:
                j = 2;
                break;
            case CardColor.Blue:
                j = 0;
                break;
            case CardColor.Yellow:
                j = 3;
                break;
            case CardColor.Green:
                j = 1;
                break;
        }
        switch (group)
        {
            case CardGroup.Action:
                i = 40;
                switch (type)
                {
                    case CardType.DrawTwo:
                        mat = materials[i + j * 3];
                        break;
                    case CardType.Reverse:
                        mat = materials[i + 1 + j * 3];
                        break;
                    case CardType.Skip:
                        mat = materials[i + 2 + j * 3];
                        break;
                }
                break;
            case CardGroup.Number:
                i = (int)type;
                mat = materials[i + j*10];
                break;
            case CardGroup.Wild:
                i = 52;
                switch (type)
                {
                    case CardType.DrawFour:
                        mat = materials[i + 1];
                        break;
                    case CardType.WildCard:
                        mat = materials[i];
                        break;
                }
                break;
        }
        return mat;
    }
}