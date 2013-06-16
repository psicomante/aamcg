using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Amucuga;

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
    /// The list of connected players
    /// </summary>
    private SortedList<string, ConnectedPlayer> _players;

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
        _players = new SortedList<string, ConnectedPlayer>();
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
    /// <param name="np">The NetworkPlayer</param>
    void OnPlayerConnected(NetworkPlayer np)
    {
        GameObject playerCube = (GameObject)GameObject.Instantiate(playerPrefab);
        playerCube.renderer.material.color = new Color(Random.value, Random.value, Random.value);
        _players.Add(np.guid, new ConnectedPlayer(playerCube, np));
    }

    /// <summary>
    /// Handles the PlayerDisconnected event
    /// </summary>
    /// <param name="np">The NetworkPlayer</param>
    void OnPlayerDisconnected(NetworkPlayer np)
    {
        _players[np.guid].Destroy();
        _players.Remove(np.guid);
    }

    /// <summary>
    /// Applies a force to a player
    /// </summary>
    /// <param name="force">The force to apply</param>
    /// <param name="guid">The guid of the player</param>
    [RPC]
    void AddForce(string guid, Vector3 force)
    {
        _players[guid].Cube.rigidbody.AddForce(force);

        // Limits the player's velocity
        if (_players[guid].Cube.rigidbody.velocity.sqrMagnitude > AmApplication.MAX_VELOCITY_MAGNITUDE * AmApplication.MAX_VELOCITY_MAGNITUDE)
        {
            Vector3 v = _players[guid].Cube.rigidbody.velocity;
            v.Normalize();
            _players[guid].Cube.rigidbody.velocity = v * AmApplication.MAX_VELOCITY_MAGNITUDE;
        }
    }
}
