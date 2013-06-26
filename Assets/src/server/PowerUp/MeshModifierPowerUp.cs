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
            : base(15)
		{
			PowerUpColor = Color.red;
			IsCumulative = true;
			Name = "Shapeshifter";
		}

		/// <summary>
		/// Enables the powerup
		/// </summary>
		protected override void EnablePowerUpEffect ()
		{
			AttachedPlayer.Cube.GetComponent<MeshFilter>().mesh = GameObject.CreatePrimitive (PrimitiveType.Sphere).GetComponent<MeshFilter>().mesh;
		}

		/// <summary>
		/// Disables the powerup
		/// </summary>
		protected override void DisablePowerUpEffect ()
		{
			AttachedPlayer.Cube.GetComponent<MeshFilter> ().mesh = GameObject.CreatePrimitive (PrimitiveType.Cube).GetComponent<MeshFilter> ().mesh;
		}
	}

}