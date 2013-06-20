using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// Allows the player to fly
    /// </summary>
    public class FlightPowerUp : PowerUp
    {
        /// <summary>
        /// Initialize a new FlightPowerUp
        /// </summary>
        public FlightPowerUp()
            : base(30) 
        {
            PowerUpColor = Color.blue;
            IsCumulative = false;
            Name = "Iron Man";
        }

        /// <summary>
        /// Enables the powerup.
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            AttachedPlayer.CanFly = true;
        }

        /// <summary>
        /// Disables the powerup.
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.CanFly = false;
        }
    }
}