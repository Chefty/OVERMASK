using System;
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
        
        private const string LOCAL_USERNAME_KEY = "USERNAME";

        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField ip;
        [SerializeField] private Button joinRandomMatchButton;
        [SerializeField] private TMP_InputField roomCode;
        [SerializeField] private Button joinCodeMatchButton;
        [SerializeField] private Button createCodeMatchButton;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI gameCode;
        [SerializeField] private TextMeshProUGUI loadingMessage;

        private void Start()
        {
            joinRandomMatchButton.onClick.AddListener(() => ConnectToServer(CreateOrJoinRandomRoom));
            joinCodeMatchButton.onClick.AddListener(() =>
            {
                if (string.IsNullOrEmpty(roomCode.text) || roomCode.text.Length < 4)
                    return;
                
                ConnectToServer(JoinCodeRoom);
            });
            createCodeMatchButton.onClick.AddListener(() => ConnectToServer(CreateCodeRoom));
            username.text = PlayerPrefs.GetString(LOCAL_USERNAME_KEY, string.Empty);
            Client.Instance.Reset();
        }

        private void ConnectToServer(Action callback)
        {
            loadingMessage.text = "Loading...";
            gameCode.gameObject.SetActive(false);
            loadingPanel.SetActive(true);
            var connectDto = new PlayerDto(username.text);
            PlayerPrefs.SetString(LOCAL_USERNAME_KEY, username.text);
            var address = string.IsNullOrEmpty(ip.text) ? IpToConnect : ip.text;
            Client.Instance.ConnectToServer(address, connectDto, callback);
            //Client.Instance.ConnectToServer(address, connectDto, () => UnityMainThreadDispatcher.Instance.WaitAndCall(callback, 0.5f));
        }

        private void CreateOrJoinRandomRoom()
        {
            Client.Instance.OnOpponentFound.AddListener(OnMatchStart);
            loadingMessage.text = "Waiting for opponent to join...";
            Client.Instance.CreateOrJoinRoom();
        }
        
        private void CreateCodeRoom()
        {
            Client.Instance.OnSuccessCreatingRoomWithCode.AddListener(OnRoomWithCodeCreated);
            Client.Instance.CreateRoomWithCode();
        }

        private void OnRoomWithCodeCreated(ConnectToRoomWithCodeDto dto)
        {
            Client.Instance.OnSuccessCreatingRoomWithCode.RemoveListener(OnRoomWithCodeCreated);
            gameCode.text = $"Game code:\n{dto.RoomCode:D4}";
            gameCode.gameObject.SetActive(true);
            
            Client.Instance.OnOpponentFound.AddListener(OnMatchStart);
            loadingMessage.text = "Waiting for opponent to join...";
        }

        private void JoinCodeRoom()
        {
            Client.Instance.OnOpponentFound.AddListener(OnMatchStart);
            Client.Instance.OnFailedToJoinRoomWithCode.AddListener(OnFailedToJoinRoomWithCode);
            Client.Instance.JoinRoomWithCode(int.Parse(roomCode.text));
        }

        private void OnFailedToJoinRoomWithCode(ConnectToRoomWithCodeDto dto) 
        {
            Client.Instance.OnFailedToJoinRoomWithCode.RemoveListener(OnFailedToJoinRoomWithCode);
            Client.Instance.OnOpponentFound.RemoveListener(OnMatchStart);

            loadingMessage.text = $"Failed: {dto.Reason}";
            UnityMainThreadDispatcher.Instance.WaitAndCall(() =>
            {
                loadingPanel.SetActive(false);
            }, 3f);
        }

        public void OnMatchStart(GameStartDto dto)
        {
            Client.Instance.OnFailedToJoinRoomWithCode.RemoveListener(OnFailedToJoinRoomWithCode);
            loadingPanel.SetActive(false);
            Client.Instance.OnOpponentFound.RemoveListener(OnMatchStart);
            Debug.LogWarning("Match started");
            SceneManager.LoadScene("GameScene");
        }
    }
}