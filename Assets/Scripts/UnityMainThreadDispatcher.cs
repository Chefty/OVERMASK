using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using client;

namespace PimDeWitte.UnityMainThreadDispatcher
{
    /// Author: Pim de Witte (pimdewitte.com) and contributors, https://github.com/PimDeWitte/UnityMainThreadDispatcher
    /// <summary>
    /// A thread-safe class which holds a queue with actions to execute on the next Update() method. It can be used to make calls to the main thread for
    /// things such as UI Manipulation in Unity. It was developed for use in combination with the Firebase Unity plugin, which uses separate threads for event handling
    /// </summary>
    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        public static UnityMainThreadDispatcher Instance { get; private set; }

        private static readonly Queue<Action> _executionQueue = new Queue<Action>();

        private void Awake()
        {
            if (Instance != null)
                return;

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            // Dispatch WebSocket messages from NativeWebSocket queue
            Client.Instance.Update();
            
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(Action action)
        {
            _executionQueue.Enqueue(action);
        }

        public void WaitAndCall(Action callback, float seconds)
        {
            StartCoroutine(InternalWaitAndCallback(callback, seconds));
        }

        private IEnumerator InternalWaitAndCallback(Action callback, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            callback();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void OnApplicationQuit()
        {
            Client.Instance.Dispose();
        }
    }
}