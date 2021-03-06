using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// Adds extra mass to the player
    /// </summary>
    public class SuperMassPowerUp : PowerUp
    {
        private const float MASS_MODIFIER = 0.3f;

        public SuperMassPowerUp()
            : base(15) 
        {
            PowerUpColor = Color.black;
            IsCumulative = true;
            Name = "Anvil";
        }

        /// <summary>
        /// Enables the power up effect
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            AttachedPlayer.GetComponent<Rigidbody>().mass += 1;
        }

        /// <summary>
        /// Disables the power up effect
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.GetComponent<Rigidbody>().mass -= 1;
        }
    }
}