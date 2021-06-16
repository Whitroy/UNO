using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UNO.Multiplayer
{
    public class UILobby : MonoBehaviour
    {
        private static UILobby _instance;

        [Header("Host Join")]
        [SerializeField] private TMP_InputField joinMatchInput = null;
        [SerializeField] List<Selectable> lobbySelectables = new List<Selectable>();
        [SerializeField] Canvas lobbyCanvas = null;
        [SerializeField] Canvas searchCanvas = null;

        private bool searching = false;

        [Header("Lobby")]
        [SerializeField] private Transform UIPlayerParent = null;
        [SerializeField] private GameObject UIPlayerPrefab = null;
        [SerializeField] private TextMeshProUGUI matchIDText = null;
        [SerializeField] private GameObject beginGameButton = null;

        private GameObject localPlayerLobbyUI;

        public static UILobby Instance { get => _instance; private set => _instance = value; }

        private void Start()
        {
            Instance = this;
        }

        public void HostPublic()
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            Player.LocalPlayer.HostGame(true);
        }

        public void HostPrivate()
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            Player.LocalPlayer.HostGame(false);
        }

        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.LocalPlayer);
                matchIDText.text = matchID;
                beginGameButton.SetActive(true);
            }
            else
            {
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }


        public void Join()
        {
            lobbySelectables.ForEach(x => x.interactable = false);
            Player.LocalPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }

        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.LocalPlayer);
                matchIDText.text = matchID;
            }
            else
            {
                lobbySelectables.ForEach(x => x.interactable = true);
            }


        }

        public void DisconnectGame()
        {
            if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
            Player.LocalPlayer.DisconnectGame();

            lobbyCanvas.enabled = false;
            lobbySelectables.ForEach(x => x.interactable = true);
            beginGameButton.SetActive(false);
        }

        public GameObject SpawnPlayerUIPrefab(Player player)
        {
            GameObject newPlayerUI = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newPlayerUI.GetComponent<UIPlayer>().SetPlayer(player);
            newPlayerUI.transform.SetSiblingIndex(player.PlayerIndex - 1);

            return newPlayerUI;
        }


        public void BeginGame()
        {
            Player.LocalPlayer.BeginGame();
        }

        public void SearchGame()
        {
            StartCoroutine(Searching());
        }

        public void CancelSearchGame()
        {
            searching = false;
        }

        public void SearchGameSuccess(bool success, string matchID)
        {
            if (success)
            {
                searchCanvas.enabled = false;
                searching = false;
                JoinSuccess(success, matchID);
            }
        }

        IEnumerator Searching()
        {
            searchCanvas.enabled = true;
            searching = true;

            float searchInteravl = 1;
            float currentTime = 1;
            while (searching)
            {
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    currentTime = searchInteravl;
                    Player.LocalPlayer.SearchGame();
                }
                yield return null;
            }
            searchCanvas.enabled = false;
        }
    }
}