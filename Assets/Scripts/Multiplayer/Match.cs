using Mirror;
using UnityEngine;

namespace UNO.Multiplayer
{
    [System.Serializable]
    public class Match
    {
        private string matchID;
        private bool publicMatch;
        private bool inMatch;
        private bool matchFull;
        private SyncListGameObject players = new SyncListGameObject();

        public Match(){ }

        public Match(string matchID, bool publicMatch, GameObject player)
        {
            this.MatchFull = false;
            this.InMatch = false;
            this.MatchID = matchID;
            this.PublicMatch = publicMatch;
            this.Players.Add(player);
        }

        public string MatchID { get => matchID; set => matchID = value; }
        public bool PublicMatch { get => publicMatch; set => publicMatch = value; }
        public bool InMatch { get => inMatch; set => inMatch = value; }
        public bool MatchFull { get => matchFull; set => matchFull = value; }
        public SyncListGameObject Players { get => players; set => players = value; }
    }

    [System.Serializable]
    public class SyncListGameObject : SyncList<GameObject> { }

}
