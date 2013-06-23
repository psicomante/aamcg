using UnityEngine;
using System.Collections;
using Amucuga;

public class TileManager : MonoBehaviour {
	
	private ConnectedPlayer _touchedBy;
	
	// records the touched state of tile
	public bool Touched { get; set; }
	
	/// <summary>
	/// The default tile material.
	/// </summary>
	public Material defaultTileMaterial;	
	
	/// <summary>
	/// Updates the tiles
	/// </summary>
	public void Update ()
	{
        //blocks wrong execution
        if (!Network.isServer || AmApplication.CurrentMatchState != MatchState.MATCH)
            return;

		Color currentTileColor = gameObject.renderer.material.color;
		
		// increment the current color to white (tile color decay)
		// if touched (by a player or spawner)
		if (Touched) {
			if (currentTileColor != AmApplication.DEFAULT_COLOR) {
				currentTileColor.r += AmApplication.TILE_COLOR_DECAY;
				currentTileColor.g += AmApplication.TILE_COLOR_DECAY;
				currentTileColor.b += AmApplication.TILE_COLOR_DECAY;
				currentTileColor.a = 0.5f;
				gameObject.renderer.material.color = currentTileColor;
				// color has been reset: reset touch variables too
			} else {
				gameObject.renderer.material = defaultTileMaterial;
				Touched = false;
				_touchedBy = null;				
			}
		}
		
	
	}

	/// <summary>
	/// Collision detection
	/// </summary>
	public void OnCollisionEnter (Collision collision)
	{
		// hey, you touched me!
		Touched = true;
		_touchedBy = collision.gameObject.GetComponent<ConnectedPlayer> ();
		
		//update the color
		Color tileColor = gameObject.renderer.material.color;
		if (tileColor != Color.red)
			gameObject.renderer.material.color = collision.gameObject.renderer.material.color;
	}	
	
}
