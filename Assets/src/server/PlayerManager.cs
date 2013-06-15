using UnityEngine;
using System.Collections;
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
    /// Applies a force to a player
    /// </summary>
    /// <param name="force">The force to apply</param>
    /// <param name="guid">The guid of the player</param>
    [RPC]
    void AddForce(string guid, Vector3 force)
    {
        player.rigidbody.AddForce(force);
        Debug.Log("1 - magnitude: " + player.rigidbody.velocity.magnitude);
        if (player.rigidbody.velocity.sqrMagnitude > AmApplication.MAX_VELOCITY_MAGNITUDE * AmApplication.MAX_VELOCITY_MAGNITUDE)
        {
            Vector3 v = player.rigidbody.velocity;
            v.Normalize();
            player.rigidbody.velocity = v * AmApplication.MAX_VELOCITY_MAGNITUDE;
            //Debug.Log("2 - magnitude: " + player.rigidbody.velocity.magnitude);
            //player.rigidbody.velocity *= AmApplication.MAX_VELOCITY_MAGNITUDE;
            //Debug.Log("3 - magnitude: " + player.rigidbody.velocity.magnitude);
        }
    }
}
