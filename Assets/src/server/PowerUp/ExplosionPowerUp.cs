using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// The player gains more speed
    /// </summary>
    public class ExplosionPowerUp : PowerUp
    {
        /// <summary>
        /// Initializes the powerup
        /// </summary>
        public ExplosionPowerUp()
            : base(5)
        {
            PowerUpColor = Color.yellow;
            IsCumulative = true;
            Name = "XPlode";
        }

        /// <summary>
        /// Enables the powerup
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            AttachedPlayer.OnGenerateExplosion();
        }

        /// <summary>
        /// Disables the powerup
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            
        }
    }

}
