using ModalWindows;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class NetworkWindowTemp : ModalWindow
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
        }

        private void StartClient()
        {
        }
        
        private void BackToMainMenu()
        {
            Hide();
        }
    }
}