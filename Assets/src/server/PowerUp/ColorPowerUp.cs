using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// Changes the player color
    /// </summary>
    public class ColorPowerUp : PowerUp
    {
        private Color backupColor;

        public ColorPowerUp()
            : base(20)
        {
            PowerUpColor = Color.magenta;
            IsCumulative = false;
        }

        /// <summary>
        /// Enables the powerup
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            backupColor = AttachedPlayer.renderer.material.color;
        }

        /// <summary>
        /// Disables the powerup
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.renderer.material.color = backupColor;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UpdatePowerUpEffect()
        {
            AttachedPlayer.renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }

}
