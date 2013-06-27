using UnityEngine;
using System.Collections;
using Amucuga;

/// <summary>
/// The client PlayerManager
/// </summary>
public class C_PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private GameObject _playerCube;

    /// <summary>
    /// Initializes the PlayerManager
    /// </summary>
    void Start ()
	{
		//Blocks wrong execution
		if (!Network.isClient)
			return;

        _playerCube = (GameObject)GameObject.Instantiate(playerPrefab);
        _playerCube.GetComponent<Rigidbody>().useGravity = false;
        _playerCube.GetComponent<BoxCollider>().enabled = false;
        Color color = new Color(Random.value, Random.value, Random.value);
        _playerCube.renderer.material.color = color;
		networkView.RPC ("AddPlayerName", RPCMode.Server, Network.player.guid, PlayerSettings.PlayerName);
        networkView.RPC("AddPlayerColor", RPCMode.Server, Network.player.guid, color.r, color.g, color.b);

		Debug.Log ("Start Client Player Manager");
	}

    /// <summary>
    /// Updates the PlayerManager
    /// </summary>
    void Update()
    {
        //Blocks the Server exectution
        if (!Network.isClient || AmApplication.CurrentMatchState != MatchState.MATCH)
            return;
    }

    /// <summary>
    /// Updates the Physics of the player
    /// </summary>
    void FixedUpdate ()
	{
		//Blocks the Server execution
		if (!Network.isClient || AmApplication.CurrentMatchState != MatchState.MATCH)
			return;

		//Input management
		if (Input.GetKey ("left"))
			networkView.RPC ("AddForce", RPCMode.Server, Network.player.guid, Vector3.left);
		if (Input.GetKey ("right"))
			networkView.RPC ("AddForce", RPCMode.Server, Network.player.guid, Vector3.right);
		if (Input.GetKey ("up"))
			networkView.RPC ("AddForce", RPCMode.Server, Network.player.guid, Vector3.forward);
		if (Input.GetKey ("down"))
			networkView.RPC ("AddForce", RPCMode.Server, Network.player.guid, Vector3.back);
		if (Input.GetKey ("space"))
			networkView.RPC ("AddForce", RPCMode.Server, Network.player.guid, Vector3.up);
		if (Input.acceleration.magnitude != 0)
			networkView.RPC ("AddForce", RPCMode.Server, Network.player.guid, new Vector3 (Input.acceleration.x, Input.acceleration.z, Input.acceleration.y) * 1.5f);
	}
}
