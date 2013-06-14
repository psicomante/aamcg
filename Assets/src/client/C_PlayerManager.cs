using UnityEngine;
using System.Collections;

/// <summary>
/// The client PlayerManager
/// </summary>
public class C_PlayerManager : MonoBehaviour
{

    /// <summary>
    /// Initializer the PlayerManager
    /// </summary>
    void Start() 
    {
        //Blocks server execution
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

        //Input management
        if (Input.GetKey("space"))
            networkView.RPC("AddForce", RPCMode.Server, Vector3.up * 20);
    }
}
