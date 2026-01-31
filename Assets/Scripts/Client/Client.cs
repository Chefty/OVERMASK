using System;
using System.Threading;
using System.Threading.Tasks;
using client.dto;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

namespace client
{
    public class Client
    {
        public readonly UnityEvent OnOpponentDisconnected = new();

        public readonly UnityEvent<GameStartDto> OnOpponentFound = new();
        public readonly UnityEvent<RequestCardDto> OnCardRequested = new();
        public readonly UnityEvent<EndRoundDto> OnRoundEnded = new();
        
        private readonly TimeSpan pingTimeSpan = new(0, 0, 15);
        private CancellationTokenSource pingCancellationTokenSource;

        private WebSocket ws;
        public static Client Instance { get; } = new();

        public GameStartDto GameDto { get; set; }

        public void ConnectToServer(string url, PlayerDto playerDto, Action onConnected)
        {
            Debug.Log($"Trying to connect to {url}");
            ws = new WebSocket(url);

            ws.OnOpen += (sender, e) =>
            {
                SendMessage(new MessageDto("Connection", playerDto));
                StartServerPingRoutine();

                Debug.Log("Connected to WebSocket server");
                onConnected.Invoke();
            };

            ws.OnMessage += ReadMessage;

            ws.OnClose += (sender, e) =>
            {
                Debug.Log("Disconnected from WebSocket server");
                pingCancellationTokenSource?.Cancel();
                pingCancellationTokenSource?.Dispose();
            };

            // var sslProtocolHack = (SslProtocols)(SslProtocolsHack.Tls12 | SslProtocolsHack.Tls11 | SslProtocolsHack.Tls);
            // ws.SslConfiguration.EnabledSslProtocols = sslProtocolHack;
            ws.Connect();
        }

        private async Task StartServerPingRoutine()
        {
            pingCancellationTokenSource = new CancellationTokenSource();
            while (!pingCancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(pingTimeSpan, pingCancellationTokenSource.Token);
                ws?.Ping();
            }
        }

        private void ReadMessage(object sender, MessageEventArgs message)
        {
            using (var ms = new CustomMemoryStream(message.RawData))
            {
                var type = ms.ReadString();

                switch (type)
                {
                    case "OpponentFound":
                        GameDto = new GameStartDto();
                        GameDto.ReadFromStream(ms);
                        Callback(() => OnOpponentFound.Invoke(GameDto));
                        break;
                    case "OpponentDisconnected":
                        Callback(() => { OnOpponentDisconnected.Invoke(); });
                        break;
                    case "CardData":
                        //todo: get card data dto
                        SendMessage(new MessageDto("Ready"));
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
                }
            }
        }

        private void Callback(Action action)
        {
            UnityMainThreadDispatcher.Instance.Enqueue(action);
        }

        public void SendMessage(MessageDto message)
        {
            using (var ms = new CustomMemoryStream())
            {
                message.WriteToStream(ms);
                var data = ms.ToArray();
                ws.Send(data);
            }
        }

        public void Dispose()
        {
            ws.Close();
        }

        public void CreateOrJoinRoom()
        {
            SendMessage(new MessageDto("CreateOrJoinRoom"));
        }

        [Flags]
        private enum SslProtocolsHack
        {
            Tls = 192,
            Tls11 = 768,
            Tls12 = 3072
        }
    }
}