using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void Start()
    {
        SceneManager.LoadScene(sceneName);
    }
}
