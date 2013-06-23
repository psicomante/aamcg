using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// The force added to the player is incremented
    /// </summary>
    public class StrengthPowerUp : PowerUp
    {
        private const float FORCE_MULTIPLIER_INCREMENT = 4f;

        /// <summary>
        /// Initializes the powerup
        /// </summary>
        public StrengthPowerUp()
            : base(20)
        {
            PowerUpColor = Color.gray;
            IsCumulative = true;
            Name = "Hulk";
        }

        protected override void EnablePowerUpEffect()
        {
            AttachedPlayer.ForceMultiplier += FORCE_MULTIPLIER_INCREMENT;
        }

        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.ForceMultiplier -= FORCE_MULTIPLIER_INCREMENT;
        }
    }
}
