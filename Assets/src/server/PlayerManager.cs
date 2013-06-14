using UnityEngine;
using System.Collections;

/// <summary>
/// The server PlayerManager
/// </summary>
public class PlayerManager : MonoBehaviour
{

    //Prefabs
    public GameObject playerPrefab;
    public GameObject planePrefab;
    public GameObject lightPrefab;

    /// <summary>
    /// The player connected
    /// </summary>
    /// TODO: allow multiplayer
    public GameObject player;

    /// <summary>
    /// Intialize the PlayerManager
    /// </summary>
    void Start()
    {
        //Blocks client execution
        if (Network.isClient)
            return;

        Debug.Log("Start Server Player Manager");

        //Initializes the scene objects
        GameObject.Instantiate(planePrefab);
        GameObject.Instantiate(lightPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        //Blocks client execution
        if (Network.isClient)
            return;
    }

    /// <summary>
    /// Handles the PlayerConnected event
    /// </summary>
    void OnPlayerConnected()
    {
        player = (GameObject)GameObject.Instantiate(playerPrefab);
    }

    /// <summary>
    /// Adds a force to a player
    /// </summary>
    /// <param name="force"></param>
    [RPC]
    void AddForce(Vector3 force)
    {
        player.rigidbody.AddForce(force);
    }
}
