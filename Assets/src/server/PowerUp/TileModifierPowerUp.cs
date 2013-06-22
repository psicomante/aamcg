using UnityEngine;
using System.Collections;

namespace Amucuga
{
    /// <summary>
    /// Increases the player dimensions
    /// </summary>
    public class TileModifierPowerUp : PowerUp
    {
        private enum ModifierType
        {
            TILE_WIDTH_INCREMENT,
            TILE_DEPTH_INCREMENT,
            TILE_WIDTH_DECREMENT,
            TILE_DEPTH_DECREMENT
        }

        private const float DIMENSIONS_INCREMENT = 0.5f;
        private const float DURATION = 15;
        private float _transitionTime;
        private float _step;
        private float _totalModified;
        private ModifierType _type;

        public TileModifierPowerUp()
            : base(DURATION)
        {
            PowerUpColor = Color.white;
            IsCumulative = true;
            Name = "The Matrix";
        }

        /// <summary>
        /// Enables the powerup
        /// </summary>
        protected override void EnablePowerUpEffect()
        {
            _transitionTime = DURATION / 6f;
            _step = DIMENSIONS_INCREMENT / _transitionTime;
            _totalModified = 0;
            Debug.Log(_step);
            switch(Random.Range(0,3))
            {
                case 0:
                    _type = ModifierType.TILE_DEPTH_DECREMENT;
                    break;
                case 1:
                    _type = ModifierType.TILE_DEPTH_INCREMENT;
                    break;
                case 2:
                    _type = ModifierType.TILE_WIDTH_DECREMENT;
                    break;
                case 3:
                    _type = ModifierType.TILE_WIDTH_INCREMENT;
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }

        protected override void UpdatePowerUpEffect()
        {
            // Final transition
            if (CountDown < _transitionTime * 1.2f)
            {
                if (_totalModified > 0)
                {
                    float currentIncrement = Mathf.Min(_totalModified, _step * Time.deltaTime);
                    switch (_type)
                    {
                        case ModifierType.TILE_WIDTH_INCREMENT:
                            AmApplication.MapTileWidth -= currentIncrement;
                            break;
                        case ModifierType.TILE_DEPTH_INCREMENT:
                            AmApplication.MapTileDepth -= currentIncrement;
                            break;
                        case ModifierType.TILE_WIDTH_DECREMENT:
                            AmApplication.MapTileWidth += currentIncrement;
                            break;
                        case ModifierType.TILE_DEPTH_DECREMENT:
                            AmApplication.MapTileDepth += currentIncrement;
                            break;
                        default:
                            throw new System.NotImplementedException();
                    }
                    _totalModified -= currentIncrement;
                }
            }
            // Initial transition
            else if (_totalModified < DIMENSIONS_INCREMENT)
            {
                float currentIncrement = Mathf.Min(DIMENSIONS_INCREMENT - _totalModified, _step * Time.deltaTime);
                switch (_type)
                {
                    case ModifierType.TILE_WIDTH_INCREMENT:
                        AmApplication.MapTileWidth += currentIncrement;
                        break;
                    case ModifierType.TILE_DEPTH_INCREMENT:
                        AmApplication.MapTileDepth += currentIncrement;
                        break;
                    case ModifierType.TILE_WIDTH_DECREMENT:
                        AmApplication.MapTileWidth -= currentIncrement;
                        break;
                    case ModifierType.TILE_DEPTH_DECREMENT:
                        AmApplication.MapTileDepth -= currentIncrement;
                        break;
                    default:
                        throw new System.NotImplementedException();
                }
                _totalModified += currentIncrement;
            }

            
        }

        /// <summary>
        /// Disables the powerup
        /// </summary>
        protected override void DisablePowerUpEffect()
        {

        }
    }

}