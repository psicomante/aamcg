using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// The player gains more speed
    /// </summary>
    public class SpeedPowerUp : PowerUp
    {
        private const float MAX_VELOCITY_MAGNITUDE_INCREMENT = 3f;

        /// <summary>
        /// Initializes the powerup
        /// </summary>
        public SpeedPowerUp()
            : base(20)
        {
            PowerUpColor = Color.yellow;
            IsCumulative = true;
            Name = "Beep Beep";
        }

        /// <summary>
        /// Enables the powerup
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            AttachedPlayer.MaxVelocityMagnitude += MAX_VELOCITY_MAGNITUDE_INCREMENT;
        }

        /// <summary>
        /// Disables the powerup
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.MaxVelocityMagnitude -= MAX_VELOCITY_MAGNITUDE_INCREMENT;
        }
    }

}
