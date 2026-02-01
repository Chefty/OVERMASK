using System;
using System.Collections;
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
        private const string IP_TO_CONNECT = "ws://localhost:8080";

        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField ip;
        [SerializeField] private Button playButton;
        [SerializeField] private GameObject loadingPanel;

        private void Start()
        {
            playButton.onClick.AddListener(ConnectToServer);
        }

        private void ConnectToServer()
        {
            loadingPanel.SetActive(true);
            var connectDto = new PlayerDto(username.text);
            Client.Instance.ConnectToServer(ip.text ?? IP_TO_CONNECT, connectDto, OnClientConnected);
        }

        private void OnClientConnected()
        {
            Client.Instance.OnOpponentFound.AddListener(OnMatchStart);
            Client.Instance.OnCardRequested.AddListener(requestCardDto =>
            {
                StartCoroutine(WaitAndCall(() =>
                {
                    Client.Instance.SendMessage(new MessageDto("ChooseCard", new ChooseCardDto(1)));
                }));
            });
            Client.Instance.CreateOrJoinRoom();
        }

        private IEnumerator WaitAndCall(Action action)
        {
            yield return new WaitForSecondsRealtime(1f);
            action();
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