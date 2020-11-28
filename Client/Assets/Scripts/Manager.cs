using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Network Info")]
    public int port = 14242;
    public string server = "127.0.0.1";
    public string gameName = "game";

    void Awake()
    {
        Debug.Log("Starting game manager");
        StaticManager.InitializeGameManager(port, server, gameName);
    }

    void OnApplicationQuit()
    {
        StaticManager.NetworkManager.SendDisconnect();
    }
}
