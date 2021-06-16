using UnityEngine;
using Mirror;

namespace UNO.Multiplayer
{
    public class AutoHostClient : MonoBehaviour
    {
        [SerializeField] private NetworkManager networkManager = null;

        void Start()
        {
            if (!Application.isBatchMode)
            { //Headless build
                Debug.Log($"=== Client Build ===");
                networkManager.StartClient();
            }
            else
            {
                Debug.Log($"=== Server Build ===");
            }
        }

        public void JoinLocal()
        {
            networkManager.networkAddress = "localhost";
            networkManager.StartClient();
        }

    }
}