using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ColorPanel : MonoBehaviour
{
    public void OnButtonClick()
    {
        if(EventSystem.current.currentSelectedGameObject.GetComponent<ColorButton>() != null)
            EventSystem.current.currentSelectedGameObject.GetComponent<ColorButton>().SetColor();
        MatchManager.Instance.HideColorPanel();
    }
}
