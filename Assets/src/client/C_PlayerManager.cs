using UnityEngine;
using System.Collections;
using Amucuga;

/// <summary>
/// The client PlayerManager
/// </summary>
public class C_PlayerManager : MonoBehaviour
{

    /// <summary>
    /// Initializes the PlayerManager
    /// </summary>
    void Start()
    {
        //Blocks the server execution
        if (Network.isServer)
            return;

        Debug.Log("Start Client Player Manager");
    }

    /// <summary>
    /// Updates the PlayerManager
    /// </summary>
    void Update()
    {
        //Blocks the Server exectution
        if (Network.isServer)
            return;
    }

    /// <summary>
    /// Updates the Physics of the player
    /// </summary>
    void FixedUpdate()
    {
        //Blocks the Server execution
        if (Network.isServer)
            return;

        //Input management
        if (Input.GetKey("left"))
            networkView.RPC("AddForce", RPCMode.Server, Network.player.guid, Vector3.left * AmApplication.FORCE_MULTIPLIER);
        if (Input.GetKey("right"))
            networkView.RPC("AddForce", RPCMode.Server, Network.player.guid, Vector3.right * AmApplication.FORCE_MULTIPLIER);
        if (Input.GetKey("up"))
            networkView.RPC("AddForce", RPCMode.Server, Network.player.guid, Vector3.forward * AmApplication.FORCE_MULTIPLIER);
        if (Input.GetKey("down"))
            networkView.RPC("AddForce", RPCMode.Server, Network.player.guid, Vector3.back * AmApplication.FORCE_MULTIPLIER);
        if (Input.GetKey("space"))
            networkView.RPC("AddForce", RPCMode.Server, Network.player.guid, Vector3.up * AmApplication.FORCE_MULTIPLIER);
        if (Input.acceleration.magnitude != 0)
            networkView.RPC("AddForce", RPCMode.Server, Network.player.guid, new Vector3(Input.acceleration.x, 0, Input.acceleration.y) * AmApplication.FORCE_MULTIPLIER * 2);
    }

}
