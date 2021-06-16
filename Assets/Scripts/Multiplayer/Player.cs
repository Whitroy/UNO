using System;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace UNO.Multiplayer
{
    public class Player : NetworkBehaviour
    {
        // fields
        private static Player localPlayer = null;

        [SerializeField] private GameObject playerLoobyUI = null;

        private NetworkMatchChecker networkMatchChecker = null;

        //Newtork Variables
        [SyncVar] private string matchID = string.Empty;
        [SyncVar] private Match currentMatch = null;
        [SyncVar] private int playerIndex = -1;

        
        //Properties
        public static Player LocalPlayer { get => localPlayer; private  set => localPlayer = value; }
        public Match CurrentMatch { get => currentMatch; set => currentMatch = value; }
        public int PlayerIndex { get => playerIndex; private set => playerIndex = value; }



        //Methods

        private void Awake()
        {
            networkMatchChecker = GetComponent<NetworkMatchChecker>();
        }


        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                localPlayer = this;
            }
            else
            {
                Debug.Log($"<color=yellow>Spawning other player UI Prefab</color>");
                playerLoobyUI = UILobby.Instance.SpawnPlayerUIPrefab(this);
            }
        }


        public override void OnStopClient()
        {
            Debug.Log($"<color=red>Client Stopped</color>");
            ClientDisconnect();
        }

        public override void OnStopServer()
        {
            Debug.Log($"<color=red>Client Stopped on server</color>");
            ServerDisconnect();
        }


        //Host Match

        public void HostGame(bool publicMatch)
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID, publicMatch);
        }

        [Command]
        private void CmdHostGame(string _matchID, bool publicMatch)
        {
            matchID = _matchID;
            if (MatchMaker.Instance.HostGame(_matchID, gameObject, publicMatch, out playerIndex))
            {
                Debug.Log($"<color=green>Game hosted Successfully.</color>");
                networkMatchChecker.matchId = matchID.ToGuid();
                TargetHostGame(true, _matchID, PlayerIndex);

            }
            else
            {
                Debug.Log($"<color=red>Game hosted failed</color>");
                TargetHostGame(false, _matchID, PlayerIndex);

            }
        }

        [TargetRpc]
        private void TargetHostGame(bool success, string _matchID, int _playerIndex)
        {
            PlayerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.Instance.HostSuccess(success, _matchID);
        }

        //Join Match

        public void JoinGame(string _inputID)
        {
            CmdJoinGame(_inputID);
        }

        [Command]
        private void CmdJoinGame(string _matchID)
        {
            matchID = _matchID;
            if (MatchMaker.Instance.JoinGame(_matchID, gameObject, out playerIndex))
            {
                Debug.Log($"<color=green>Game hosted Successfully.</color>");
                networkMatchChecker.matchId = matchID.ToGuid();
                TargetJoinGame(true, _matchID, PlayerIndex);

            }
            else
            {
                Debug.Log($"<color=red>Game hosted failed</color>");
                TargetJoinGame(false, _matchID, PlayerIndex);

            }
        }

        [TargetRpc]
        private void TargetJoinGame(bool success, string _matchID, int _playerIndex)
        {
            PlayerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.Instance.JoinSuccess(success, _matchID);

        }

        public void DisconnectGame()
        {
            CmdDisconnectGame();
        }

        [Command]
        private void CmdDisconnectGame()
        {
            ServerDisconnect();
        }

        private void ServerDisconnect()
        {
            MatchMaker.Instance.PlayerDisconnected(this, matchID);
            RpcDisconnectGame();
            networkMatchChecker.matchId = string.Empty.ToGuid();
        }

        [ClientRpc]
        private void RpcDisconnectGame()
        {
            ClientDisconnect();
        }

        private void ClientDisconnect()
        {
            if(playerLoobyUI != null)
            {
                Destroy(playerLoobyUI);
            }
        }

        public void SearchGame()
        {
            CmdSearchGame();
        }

        [Command]
        private void CmdSearchGame()
        {
            if(MatchMaker.Instance.SearchGame(gameObject,out playerIndex,out matchID))
            {
                Debug.Log($"<color=green>Game Found Successfully</color>");
                networkMatchChecker.matchId = matchID.ToGuid();
                TargetSearchGame(true, matchID, PlayerIndex);

            }
            else
            {
                Debug.Log($"<color=red>Game Search Failed</color>");
                TargetSearchGame(false, matchID, PlayerIndex);

            }
        }

        [TargetRpc]
        private void TargetSearchGame(bool success, string _matchID, int _playerIndex)
        {
            PlayerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID} | {success}");
            UILobby.Instance.SearchGameSuccess(success, _matchID);

        }

        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        void CmdBeginGame()
        {
            MatchMaker.Instance.BeginGame(matchID);
            Debug.Log($"<color=red>Game Beginning</color>");
        }


        public void StartGame()//Server
        {
            TargetBeginGame();
        }

        [TargetRpc]
        void TargetBeginGame()
        {
            Debug.Log($"MatchID: {matchID} | Beginning");
            //Additively load game scene
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }

    }
}