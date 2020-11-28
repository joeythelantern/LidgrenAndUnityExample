using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Networking;

public class StaticManager
{
    public static string LocalPlayerID { get; set; }
    public static Client NetworkManager { get; set; }
    public static Dictionary<string, GameObject> Players { get; set; }

    public static void InitializeGameManager(int port, string server, string game)
    {
        Debug.Log("Starting game manager.");

        LocalPlayerID = "";
        NetworkManager = new Client(port, server, game);
        Players = new Dictionary<string, GameObject>();
    }
}
