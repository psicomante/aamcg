using UnityEngine;
using System.Collections;
using Amucuga;

public class TileManager : MonoBehaviour {
	
	private float _remainingTime = 0;
	private bool _touched;
	private ConnectedPlayer _touchedBy;

	// Use this for initialization
	void Start () {
		_remainingTime = AmApplication.TILE_TOUCHEDBY_TIMEOUT;
	}
	
	/// <summary>
	/// Updates the tiles
	/// </summary>
	public void Update ()
	{

		Color currentTileColor = gameObject.renderer.material.color;
		
		// increment the current color to white (tile color decay)
		if (_touched && gameObject.renderer.material.color != Color.white) {
			currentTileColor.r += AmApplication.TILE_COLOR_DECAY;
			currentTileColor.g += AmApplication.TILE_COLOR_DECAY;
			currentTileColor.b += AmApplication.TILE_COLOR_DECAY;
			gameObject.renderer.material.color = currentTileColor;
		} else if (_touched && gameObject.renderer.material.color == Color.white) {
			_touched = false;
			_touchedBy = null;
		}
		
	
	}

	/// <summary>
	/// Collision detection
	/// </summary>
	public void OnCollisionEnter (Collision collision)
	{
		
		// hey, you touched me!
		_touched = true;
		_touchedBy = collision.gameObject.GetComponent<ConnectedPlayer> ();
		
		//update the color
		Color tileColor = gameObject.renderer.material.color;
		if (tileColor != Color.red)
			gameObject.renderer.material.color = collision.gameObject.renderer.material.color;
	}	
	
}
