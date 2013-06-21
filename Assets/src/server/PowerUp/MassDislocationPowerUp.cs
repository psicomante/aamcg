using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// Changes the mass center of the player
    /// </summary>
    public class MassDislocationPowerUp : PowerUp
    {
        /// <summary>
        /// Initializes the powerup
        /// </summary>
        public MassDislocationPowerUp()
            :base(15)
        {
            Name = "Drunk man";
            PowerUpColor = Color.white;
            IsCumulative = false;
        }

        /// <summary>
        /// Enables the powerup
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            
        }

        /// <summary>
        /// Disables the powerup
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.rigidbody.centerOfMass = Vector3.zero;
        }

        /// <summary>
        /// Updates the powerup
        /// </summary>
        protected override void UpdatePowerUpEffect()
        {
            AttachedPlayer.rigidbody.centerOfMass += new Vector3(Random.value * 0.01f, Random.value * 0.01f, Random.value * 0.01f);
        }
    }

}