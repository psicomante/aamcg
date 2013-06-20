using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// Increases the player dimensions
    /// </summary>
    public class BlowPowerUp : PowerUp
    {
        private const float DIMENSIONS_INCREMENT = 1f;

        public BlowPowerUp()
            : base(15)
        {
            PowerUpColor = Color.green;
            IsCumulative = true;
            Name = "Big QB";
        }

        /// <summary>
        /// Enables the powerup
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            AttachedPlayer.Cube.transform.localScale += new Vector3(DIMENSIONS_INCREMENT, DIMENSIONS_INCREMENT, DIMENSIONS_INCREMENT);
        }

        /// <summary>
        /// Disables the powerup
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.Cube.transform.localScale -= new Vector3(DIMENSIONS_INCREMENT, DIMENSIONS_INCREMENT, DIMENSIONS_INCREMENT);
        }
    }

}