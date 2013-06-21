using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// Changes the mass center of the player
    /// </summary>
    public class MassDislocationPowerUp : PowerUp
    {
        private float _xMassSpeed;
        private float _yMassSpeed;
        private float _zMassSpeed;
        private Color _backupColor;

        /// <summary>
        /// Initializes the powerup
        /// </summary>
        public MassDislocationPowerUp()
            :base(30)
        {
            Name = "Drunk man";
            PowerUpColor = Color.red;
            IsCumulative = false;
        }

        /// <summary>
        /// Enables the powerup
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            _xMassSpeed = Random.value / 100f;
            _yMassSpeed = Random.value / 100f;
            _zMassSpeed = Random.value / 100f;
            _backupColor = AttachedPlayer.renderer.material.color;
        }

        /// <summary>
        /// Disables the powerup
        /// </summary>
        protected override void DisablePowerUpEffect()
        {
            AttachedPlayer.rigidbody.centerOfMass = Vector3.zero;
            AttachedPlayer.renderer.material.color = _backupColor;
        }

        /// <summary>
        /// Updates the powerup
        /// </summary>
        protected override void UpdatePowerUpEffect()
        {
            Vector3 centerOfMass = AttachedPlayer.rigidbody.centerOfMass;
            centerOfMass += new Vector3(_xMassSpeed, _yMassSpeed, _zMassSpeed);
            
            // Limits x component of center of mass and invert x component moovement
            if (centerOfMass.x > 0.5f)
            {
                centerOfMass = new Vector3(0.5f, centerOfMass.y, centerOfMass.z);
                _xMassSpeed *= -1;
            }
            else if (centerOfMass.z < -0.5f)
            {
                centerOfMass = new Vector3(-0.5f, centerOfMass.y, centerOfMass.z);
                _xMassSpeed *= -1;
            }

            // Limits y component of center of mass
            if (centerOfMass.y > 0.5f)
            {
                centerOfMass = new Vector3(centerOfMass.x, 0.5f, centerOfMass.z);
                _yMassSpeed *= -1;
            }
            else if (centerOfMass.y < -0.5f)
            {
                centerOfMass = new Vector3(centerOfMass.x, -0.5f, centerOfMass.z);
                _yMassSpeed *= -1;
            }
            
            // Limits z component of center of mass
            if (centerOfMass.z > 0.5f)
            {
                centerOfMass = new Vector3(centerOfMass.x, centerOfMass.y, 0.5f);
                _zMassSpeed *= -1;
            }
            else if (centerOfMass.z < -0.5f)
            {
                centerOfMass = new Vector3(centerOfMass.x, centerOfMass.y, -0.5f);
                _zMassSpeed *= -1;
            }
            AttachedPlayer.rigidbody.centerOfMass = centerOfMass;
            Vector3 color = centerOfMass + new Vector3(0.5f, 0.5f, 0.5f);
            AttachedPlayer.renderer.material.color = new Color(color.x, color.y, color.z);
        }
    }

}