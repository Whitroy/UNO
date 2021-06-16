using Mirror;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace UNO.Multiplayer
{
    public class MatchMaker : NetworkBehaviour
    {
        private static MatchMaker instance = null;
        public static MatchMaker Instance { get => instance; private set => instance = value; }

        private SyncDictionary<string, Match> matches = new SyncDictionary<string, Match>();

        [SerializeField] private int maxMatchPlayers = 4;

        private void Start()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        public bool HostGame(string _matchID,GameObject _player,bool publicMatch,out int playerIndex)
        {
            playerIndex = -1;

            if (!matches.ContainsKey(_matchID)){


                Match newMatch = new Match(_matchID, publicMatch, _player);

                matches.Add(_matchID, newMatch);

                Debug.Log($"<color=green>Match Created</color>");

                _player.GetComponent<Player>().CurrentMatch = newMatch;

                playerIndex = 1;
                return true;

            }
             Debug.Log($"<color=red>Match ID already exists</color>");
             return false;
        }


        public bool JoinGame(string _matchID,GameObject _player,out int playerIndex)
        {
            playerIndex = -1;

            if (matches.ContainsKey(_matchID))
            {
                Match match = matches[_matchID];
                
                if(!match.InMatch && !match.MatchFull) 
                {
                    match.Players.Add(_player);
                    _player.GetComponent<Player>().CurrentMatch = match;
                    playerIndex = match.Players.Count;

                    match.MatchFull = match.Players.Count == maxMatchPlayers;

                    Debug.Log($"<color=green>Match Joined</color>");

                    return true;
                }

                Debug.Log($"<color=yellow> Match is either full or going on</color>");
            }

            Debug.Log($"<color=red>Match ID does not exist</color>");
            return false;
        }

        public bool SearchGame(GameObject _player,out int playerIndex,out string matchID)
        {
            playerIndex = -1;
            matchID = "";

            foreach (var match in matches)
            {
                Debug.Log($"<color=yellow>Checking match{match.Key} | inMatch {match.Value.InMatch}" +
                    $"| MatchFull {match.Value.MatchFull}| publicMatch {match.Value.PublicMatch} </color>");

                if(!match.Value.InMatch && !match.Value.MatchFull && match.Value.PublicMatch)
                {
                    if(JoinGame(match.Key,_player,out playerIndex))
                    {
                        matchID = match.Key;
                        return true;
                    }
                }
            }

            return false;
        }


        public void BeginGame(string _matchID)
        {
            Match match = matches[_matchID];
            if (match == null)
                return;

            match.InMatch = true;
            foreach(var player in match.Players)
            {
                player.GetComponent<Player>().StartGame();
            }

        }

        public static string GetRandomMatchID()
        {
            string _id = string.Empty;

            for(int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);
                if (random < 26)
                {
                    _id += (char)(random + 65);
                }
                else
                {
                    _id += (random - 26).ToString();
                }
            }

            Debug.Log($"<color=yellow>Random Match ID : {_id}</color>");
            return _id;
        }

        public void PlayerDisconnected(Player player,string _matchID)
        {
            Match match = matches[_matchID];
            int playerIndex = match.Players.IndexOf(player.gameObject);
            match.Players.RemoveAt(playerIndex);
            Debug.Log($"<color=yellow>Player disconnected from match {_matchID}</color>");

            if(match.Players.Count == 0)
            {
                Debug.Log($"<color=yellow>No more players in Match. Terminating {_matchID} </color>");

                matches.Remove(_matchID);
            }
        }

    }

    public static class MatchExtensions
    {
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            return new Guid(hashBytes);
        }
    }
}
