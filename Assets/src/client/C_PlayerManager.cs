using UnityEngine;
using System;
using System.Collections;
using Amucuga;

/// <summary>
/// The client PlayerManager
/// </summary>
public class C_PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;

    /// <summary>
    /// The current player
    /// </summary>
    private ConnectedPlayer _player;

    /// <summary>
    /// Initializes the PlayerManager
    /// </summary>
    void Start ()
	{
		//Blocks wrong execution
		if (!Network.isClient)
			return;

        GameObject _playerCube = (GameObject)GameObject.Instantiate(playerPrefab);
        _playerCube.GetComponent<Rigidbody>().useGravity = false;
        _playerCube.GetComponent<BoxCollider>().enabled = false;
        Color color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        _playerCube.renderer.material.color = color;
		networkView.RPC ("AddPlayerName", RPCMode.Server, Network.player.guid, PlayerSettings.PlayerName);
        networkView.RPC("AddPlayerColor", RPCMode.Server, Network.player.guid, color.r, color.g, color.b);
        _player = _playerCube.GetComponent<ConnectedPlayer>();
        Camera.main.transform.position = new Vector3(0, 5, -5);
		Debug.Log ("Start Client Player Manager");
	}

    /// <summary>
    /// Updates the PlayerManager
    /// </summary>
    void Update()
    {
        //Blocks the Server exectution
        if (!Network.isClient)
            return;

        AmApplication.MatchCountDown -= Time.deltaTime;
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

    /// <summary>
    /// Updates the client match status
    /// </summary>
    /// <param name="MatchCountDown">The match count down</param>
    /// <param name="State">The current match state</param>
    [RPC]
    void UpdateMatchStatus(float MatchCountDown, string State)
    {
        AmApplication.MatchCountDown = MatchCountDown;
        AmApplication.CurrentMatchState = (MatchState)System.Enum.Parse(typeof(MatchState), State);
    }

    /// <summary>
    /// Tells the client that a new powerup has just been collected
    /// </summary>
    /// <param name="PowerUpType">A string representing the type of the powerup</param>
    [RPC]
    void NewPowerUpCollected(string PowerUpType)
    {
        Type type = Type.GetType(PowerUpType);
        PowerUp newPowerUp = (PowerUp)Activator.CreateInstance(type);
        _player.AddPowerUp(newPowerUp);
        newPowerUp.CollectedByPlayer(_player);
    }
}
