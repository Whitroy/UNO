using System;
using UnityEngine;
using TMPro;

namespace UNO.Multiplayer
{
    public class UIPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text = null;
        private Player player;
        public void SetPlayer(Player player)
        {
            this.player = player;
            text.text = "Player" + player.PlayerIndex.ToString();
        }
    }
}