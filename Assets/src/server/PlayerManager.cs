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
        if (Network.peerType != NetworkPeerType.Server)
            return;

        // checks if the player is Dead
        CheckRespawn();
        CenterCamera();
        // reposition players
        // WARNING: HIGH SPERIMENTAL
        //RepositionPlayers ();
    }

    /// <summary>
    /// Adds a force to the camera for moving it to the mass center of the world
    /// </summary>
    private void CenterCamera()
    {
        float xMass = PlayersMassCenter().x;
        float xDisplacement = xMass - camera.transform.position.x;

        // Moves the camera to the center of mass if there is enough displacement from that point
        if (Mathf.Abs(xDisplacement) > AmApplication.MAX_CAMERA_DISPLACEMENT_FROM_MASS_CENTER)
        {
            camera.rigidbody.AddForce(new Vector3(xDisplacement * 2 - Mathf.Sign(xDisplacement) * AmApplication.MAX_CAMERA_DISPLACEMENT_FROM_MASS_CENTER, 0, 0));
        }

        // Adds a sort of friction (avoid spring behaviour)
        camera.rigidbody.AddForce(-camera.rigidbody.velocity * 2);
    }

    /// <summary>
    /// Returns the mass center of all the players
    /// </summary>
    private Vector3 PlayersMassCenter()
    {
        Vector3 massCenter = Vector3.zero;
        float totalMass = 0;
        
        foreach(KeyValuePair<string, ConnectedPlayer> p in _players)
        {
            ConnectedPlayer player = p.Value;
            float pmass = player.rigidbody.mass;
            Vector3 pcenter = player.rigidbody.worldCenterOfMass;
            massCenter = (totalMass * massCenter + pmass * pcenter) / (totalMass + pmass);
            totalMass += pmass;
        }

        return massCenter;
    }

    /// <summary>
    /// Fixed update is called once per frame
    /// </summary>
    void FixedUpdate()
    {
        // Blocks client execution
        if (Network.isClient)
            return;

        // Limits the player speed
        foreach (KeyValuePair<string, ConnectedPlayer> p in _players)
        {
            ConnectedPlayer player = p.Value;

            // Limits the player's velocity
            if (player.Cube.rigidbody.velocity.sqrMagnitude > player.MaxVelocityMagnitude * player.MaxVelocityMagnitude)
            {
                Vector3 v = player.Cube.rigidbody.velocity;
                v.Normalize();
                player.Cube.rigidbody.velocity = v * player.MaxVelocityMagnitude;
            }
        }
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
        float xSpawn = SpawnPoint.x;
        if (Mathf.Abs(_players[guid].Cube.transform.position.x - xSpawn) > AmApplication.MAX_PLAYER_DISPLACEMENT_FROM_SPAWN)
            return true;
        return false;
    }

    /// <summary>
    /// Applies a force to a player
    /// </summary>
    /// <param name="force">The force to apply</param>
    /// <param name="guid">The guid of the player</param>
    [RPC]
    void AddForce(string guid, Vector3 force)
    {
        ConnectedPlayer player = _players[guid];
        player.Cube.rigidbody.AddForce(new Vector3(force.x,  player.CanFly ? force.y : 0, force.z) * player.ForceMultiplier);
    }
}