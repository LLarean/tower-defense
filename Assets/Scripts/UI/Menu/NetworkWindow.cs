using ModalWindows;
// using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class NetworkWindow : ModalWindow
    {
        [SerializeField] private Button _server;
        [SerializeField] private Button _host;
        [SerializeField] private Button _client;
        [SerializeField] private Button _back;
        
        private void Start()
        {
            _server.onClick.AddListener(StartServer);
            _host.onClick.AddListener(StartHost);
            _client.onClick.AddListener(StartClient);
            _back.onClick.AddListener(BackToMainMenu);
        }

        private void StartServer()
        {
            // NetworkManager.Singleton.StartServer();
        }

        private void StartHost()
        {
            // NetworkManager.Singleton.StartHost();
        }

        private void StartClient()
        {
            // NetworkManager.Singleton.StartClient();
        }
        
        private void BackToMainMenu()
        {
            Hide();
        }
    }
}