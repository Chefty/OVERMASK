using System;
using client;
using client.dto;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    [SerializeField] private TextMeshProUGUI blueName;
    [SerializeField] private TextMeshProUGUI blueScore;
    [SerializeField] private TextMeshProUGUI redName;
    [SerializeField] private TextMeshProUGUI redScore;
    [SerializeField] private Button backToLobbyButton;

    private void Awake()
    {
        screen.SetActive(false);
        
        backToLobbyButton.onClick.AddListener(OnBackToLobbyClicked);
    }

    private void Start()
    {
        Client.Instance.OnGameOver.AddListener(OnGameOver);
    }

    private void OnGameOver(GameOverDto gameOverDto)
    {
        Client.Instance.OnGameOver.RemoveListener(OnGameOver);
        
        blueName.text = Game.Instance.Round.GetPlayerBy(PlayerFaction.Blue).Name;
        blueScore.text = gameOverDto.BlueScore.ToString();
        
        redName.text = Game.Instance.Round.GetPlayerBy(PlayerFaction.Red).Name;
        redScore.text = gameOverDto.RedScore.ToString();
        
        screen.SetActive(true);
        Client.Instance.Dispose();
    }
    
    private void OnBackToLobbyClicked()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
