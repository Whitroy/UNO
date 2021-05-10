using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorButton : MonoBehaviour
{
    public UNO.CardColor color;

    public void SetColor()
    {
        MatchManager.Instance.GetTopCard().Color = color;
    }
}
