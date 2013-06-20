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
    public GameObject lightPrefab;

    /// <summary>
    /// The list of connected players
    /// </summary>
    private SortedList<string, ConnectedPlayer> _players;

    /// <summary>
    /// Gets the spawn point position
    /// </summary>
    public Vector3 SpawnPoint
    {
        get
        {
            Vector3 playerSpawnPoint = GameObject.Find(AmApplication.GAMEOBJECT_MAP_GENERATOR_NAME).GetComponent<MapGenerator>().PlayerSpawnPoint;
            playerSpawnPoint.y = Network.connections.Length;
            return playerSpawnPoint;
        }
    }

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
        GameObject.Instantiate(lightPrefab);
        _players = new SortedList<string, ConnectedPlayer>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        //Blocks client execution
        if (Network.isClient)
            return;

        // checks if the player is Dead
        CheckRespawn();

        // reposition players
        // WARNING: HIGH SPERIMENTAL
        //RepositionPlayers ();
    }

    /// <summary>
    /// Handles the PlayerConnected event
    /// </summary>
    /// <param name="np">The NetworkPlayer</param>
    void OnPlayerConnected(NetworkPlayer np)
    {
        print("Player from " + np.ipAddress + " connected");
        print("Connections " + Network.connections.Length);
        GameObject playerCube = (GameObject)GameObject.Instantiate(playerPrefab, SpawnPoint, Quaternion.identity);
        playerCube.renderer.material.color = new Color(Random.value, Random.value, Random.value);
        ConnectedPlayer cp = playerCube.GetComponent<ConnectedPlayer>();
        cp.NPlayer = np;
        _players.Add(np.guid, cp);
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
    /// Checks the respawn.
    /// </summary>
    void CheckRespawn()
    {
        foreach (KeyValuePair<string, ConnectedPlayer> p in _players)
        {
            string guid = p.Value.NPlayer.guid;
            if (IsDead(guid))
                ResetPlayer(guid);
        }

    }

    /// <summary>
    /// Respawn the specified Player.
    /// </summary>
    /// <param name='guid'>
    /// GUID.
    /// </param>
    void ResetPlayer(string guid)
    {
        GameObject cube = _players[guid].Cube;
        cube.transform.position = SpawnPoint;
        cube.rigidbody.velocity = Vector3.zero;
        cube.transform.rotation = Quaternion.identity;
        cube.transform.localRotation = Quaternion.identity;
        cube.rigidbody.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Determines whether this instance is dead the specified guid.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is dead the specified guid; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='guid'>
    /// If set to <c>true</c> GUID.
    /// </param>
    bool IsDead(string guid)
    {
        if (_players[guid].Cube.transform.position.y > AmApplication.MAX_PLAYABLE_AREA_Y || _players[guid].Cube.transform.position.y < -1 * AmApplication.MAX_PLAYABLE_AREA_Y)
            return true;
        return false;
    }

    /// <summary>
    /// Repositions the players HIGH SPERIMENTAL CODE. Must refactoring based on input
    /// </summary>
    void RepositionPlayers()
    {
        foreach (KeyValuePair<string, ConnectedPlayer> p in _players)
        {
            float x = p.Value.Cube.transform.rotation.x;
            float y = p.Value.Cube.transform.rotation.y;
            float z = p.Value.Cube.transform.rotation.z;
            float w = p.Value.Cube.transform.rotation.w;
            p.Value.Cube.transform.rotation = new Quaternion(x, y, 0, 0);
        }
    }


    /// <summary>
    /// Applies a force to a player
    /// </summary>
    /// <param name="force">The force to apply</param>
    /// <param name="guid">The guid of the player</param>
    [RPC]
    void AddForce(string guid, Vector3 force)
    {
        _players[guid].Cube.rigidbody.AddForce(new Vector3(force.x, 0, force.z));

        // Limits the player's velocity
        if (_players[guid].Cube.rigidbody.velocity.sqrMagnitude > AmApplication.MAX_VELOCITY_MAGNITUDE * AmApplication.MAX_VELOCITY_MAGNITUDE)
        {
            Vector3 v = _players[guid].Cube.rigidbody.velocity;
            v.Normalize();
            _players[guid].Cube.rigidbody.velocity = v * AmApplication.MAX_VELOCITY_MAGNITUDE;
        }
    }
}