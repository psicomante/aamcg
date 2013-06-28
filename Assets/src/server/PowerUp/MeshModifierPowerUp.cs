using UnityEngine;
using System.Collections;

namespace Amucuga
{
	/// <summary>
	/// Change the mesh attached to player
	/// </summary>
	public class MeshModifierPowerUp : PowerUp
	{
		private const float DIMENSIONS_INCREMENT = 1f;
		private Mesh _oldMesh;

		public MeshModifierPowerUp ()
            : base(30)
		{
			PowerUpColor = Color.red;
			IsCumulative = false;
			Name = "Shapeshifter";
		}

		/// <summary>
		/// Enables the powerup
		/// </summary>
		protected override void EnablePowerUpEffect ()
		{
			// save the old mesh
			_oldMesh = AttachedPlayer.Cube.GetComponent<MeshFilter> ().mesh;
			
			// take the new mesh from ConnectedPlayer instance (once for every player)
			AttachedPlayer.Cube.GetComponent<MeshFilter> ().mesh = AttachedPlayer.Cube.GetComponent<ConnectedPlayer> ().SphereMesh;
			
			// adding the correct collider
			AttachedPlayer.Cube.GetComponent<SphereCollider> ().enabled = true;
			
			// disable the old boxcollider
			AttachedPlayer.Cube.GetComponent<BoxCollider> ().enabled = false;
		}

		/// <summary>
		/// Disables the powerup
		/// </summary>
		protected override void DisablePowerUpEffect ()
		{
			AttachedPlayer.Cube.GetComponent<MeshFilter> ().mesh = _oldMesh;
			AttachedPlayer.Cube.GetComponent<BoxCollider> ().enabled = true;
			AttachedPlayer.Cube.GetComponent<SphereCollider> ().enabled = false;
		}
	}

}