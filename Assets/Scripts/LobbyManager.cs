using client;
using client.dto;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PimDeWitte.UnityMainThreadDispatcher
{
    public class LobbyManager : MonoBehaviour
    {
        private const string IpToConnect = "wss://ggj-2026-production-8567.up.railway.app";

        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField ip;
        [SerializeField] private Button playButton;
        [SerializeField] private GameObject loadingPanel;

        private void Start()
        {
            playButton.onClick.AddListener(ConnectToServer);
        }

        private void Update()
        {
            Client.Instance.Update();
        }

        private void ConnectToServer()
        {
            loadingPanel.SetActive(true);
            var connectDto = new PlayerDto(username.text);
            var address = string.IsNullOrEmpty(ip.text) ? IpToConnect : ip.text;
            Client.Instance.ConnectToServer(address, connectDto, OnClientConnected);
        }

        private void OnClientConnected()
        {
            Client.Instance.OnOpponentFound.AddListener(OnMatchStart);
            Client.Instance.CreateOrJoinRoom();
        }

        public void OnMatchStart(GameStartDto dto)
        {
            loadingPanel.SetActive(false);
            Client.Instance.OnOpponentFound.RemoveListener(OnMatchStart);
            Debug.LogWarning("Match started");
            SceneManager.LoadScene("GameScene");
        }
    }
}