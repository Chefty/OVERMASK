using System;
using System.Threading;
using System.Threading.Tasks;
using client.dto;
using Engine;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using UnityEngine.Events;
using NativeWebSocket;

namespace client
{
    public class Client
    {
        public readonly UnityEvent OnOpponentDisconnected = new();

        public readonly UnityEvent<GameStartDto> OnOpponentFound = new();
        public readonly UnityEvent<RequestCardDto> OnCardRequested = new();
        public readonly UnityEvent<EndRoundDto> OnRoundEnded = new();
        public readonly UnityEvent<GameOverDto> OnGameOver = new();
        public readonly UnityEvent<DealInitialCardsDto> OnDealInitialCardsDto = new();
        
        private readonly TimeSpan pingTimeSpan = new(0, 0, 15);
        private CancellationTokenSource pingCancellationTokenSource;

        private WebSocket ws;
        public static Client Instance { get; } = new();

        public GameStartDto GameDto { get; set; }

        public async void ConnectToServer(string url, PlayerDto playerDto, Action onConnected)
        {
            Debug.Log($"Trying to connect to {url}");
            
            // DEVELOPMENT ONLY: Skip SSL certificate validation for Railway
            // Remove this in production!
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            System.Net.ServicePointManager.ServerCertificateValidationCallback = 
                (sender, certificate, chain, errors) => true;
            #endif
            
            ws = new WebSocket(url);

            ws.OnOpen += () =>
            {
                Debug.Log($"WebSocket OnOpen triggered. State: {ws.State}");
                SendMessage(new MessageDto("Connection", playerDto));
                _ = StartServerPingRoutine(); // Fire-and-forget task
                Debug.Log("Connected to WebSocket server");
                onConnected.Invoke();
            };

            ws.OnMessage += (bytes) =>
            {
                Debug.Log($"Received message: {bytes.Length} bytes");
                ReadMessage(bytes);
            };

            ws.OnError += (errorMsg) =>
            {
                Debug.LogError($"WebSocket Error: {errorMsg}");
            };

            ws.OnClose += (e) =>
            {
                Debug.LogWarning($"Disconnected from WebSocket server. Code: {e}, State: {ws.State}");
                pingCancellationTokenSource?.Cancel();
            };

            try
            {
                await ws.Connect();
                Debug.Log($"Connect() completed. State: {ws.State}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to connect: {ex.Message}\nStack: {ex.StackTrace}");
            }
        }
        
        // Add Update method to dispatch messages
        public void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            ws?.DispatchMessageQueue();
#endif
        }

        private async Task StartServerPingRoutine()
        {
            // NativeWebSocket doesn't support manual ping, but the connection stays alive
            // Keep the routine for future custom ping implementation if needed
            pingCancellationTokenSource = new CancellationTokenSource();
            while (!pingCancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(pingTimeSpan, pingCancellationTokenSource.Token);
                // Send a keep-alive message instead
                if (ws.State == WebSocketState.Open)
                {
                    SendMessage(new MessageDto("Ping"));
                }
            }
        }

        private void ReadMessage(byte[] data)
        {
            using (var ms = new CustomMemoryStream(data))
            {
                var type = ms.ReadString();

                switch (type)
                {
                    case "OpponentFound":
                        GameDto = new GameStartDto();
                        GameDto.ReadFromStream(ms);
                        CardsService.Instance.InjectCardsData(GameDto.CardsData);
                        Callback(() => { OnOpponentFound.Invoke(GameDto); });
                        break;
                    case "OpponentDisconnected":
                        Callback(() => { OnOpponentDisconnected.Invoke(); });
                        break;
                    case "DealInitialCards":
                        var dicdto = new DealInitialCardsDto();
                        dicdto.ReadFromStream(ms);
                        Callback(() => { OnDealInitialCardsDto.Invoke(dicdto); });
                        break;
                    case "RequestCard":
                        var crdto = new RequestCardDto();
                        crdto.ReadFromStream(ms);
                        Callback(() => { OnCardRequested.Invoke(crdto);});
                        break;
                    case "EndRound":
                        var erdto = new EndRoundDto();
                        erdto.ReadFromStream(ms);
                        Callback(() => { OnRoundEnded.Invoke(erdto);});
                        break;
                    case "GameOver":
                        var godto = new GameOverDto(ms);
                        Callback(() => { OnGameOver.Invoke(godto);});
                        break;
                }
            }
        }

        private void Callback(Action action)
        {
            UnityMainThreadDispatcher.Instance.Enqueue(action);
        }

        public async void SendMessage(MessageDto message)
        {
            if (ws == null || ws.State != WebSocketState.Open)
            {
                Debug.LogWarning("Cannot send message: WebSocket is not open");
                return;
            }

            using (var ms = new CustomMemoryStream())
            {
                message.WriteToStream(ms);
                var data = ms.ToArray();
                await ws.Send(data);
            }
        }

        public async void Dispose()
        {
            if (ws != null && ws.State == WebSocketState.Open)
            {
                await ws.Close();
            }
        }

        public void CreateOrJoinRoom()
        {
            SendMessage(new MessageDto("CreateOrJoinRoom"));
        }

        public void Reset()
        {
            OnOpponentDisconnected.RemoveAllListeners();
            OnOpponentFound.RemoveAllListeners();
            OnCardRequested.RemoveAllListeners();
            OnRoundEnded.RemoveAllListeners();
            OnGameOver.RemoveAllListeners();
            OnDealInitialCardsDto.RemoveAllListeners();
        }
    }
}