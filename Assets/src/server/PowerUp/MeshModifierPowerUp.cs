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
			AttachedPlayer.Cube.GetComponent<MeshFilter> ().mesh = GameObject.CreatePrimitive (PrimitiveType.Sphere).GetComponent<MeshFilter> ().mesh;
			AttachedPlayer.Cube.AddComponent<SphereCollider> ();
			AttachedPlayer.Cube.GetComponent<BoxCollider> ().enabled = false;
		}

		/// <summary>
		/// Disables the powerup
		/// </summary>
		protected override void DisablePowerUpEffect ()
		{
			AttachedPlayer.Cube.GetComponent<MeshFilter> ().mesh = GameObject.CreatePrimitive (PrimitiveType.Cube).GetComponent<MeshFilter> ().mesh;
			AttachedPlayer.Cube.GetComponent<BoxCollider> ().enabled = true;
		}
	}

}