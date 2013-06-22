using UnityEngine;
using System.Collections;
using Amucuga;

public class PowerUpManager : MonoBehaviour {

    /// <summary>
    /// The power up type
    /// </summary>
    public PowerUp powerUp;

	/// <summary>
	/// Initialize the powerup
	/// </summary>
	void Start () {
        int powerUpType = (int)(Random.value * 5.9999f);
        switch (powerUpType)
        {
            case 0:
                powerUp = new FlightPowerUp();
                break;
            case 1:
                powerUp = new SuperMassPowerUp();
                break;
            case 2:
                powerUp = new ColorPowerUp();
                break;
            case 3:
                powerUp = new StrengthPowerUp();
                break;
            case 4:
                powerUp = new SpeedPowerUp();
                break;
            case 5:
                powerUp = new BlowPowerUp();
                break;
            case 6:
                // Disabled for now
                powerUp = new MassDislocationPowerUp();
                break;
            default:
                throw new System.NotImplementedException("PowerUp " + powerUpType + " not implemented");
        }
        gameObject.renderer.material.color = powerUp.PowerUpColor;
	}

    /// <summary>
    /// Collision detection
    /// </summary>
    void OnTriggerEnter(Collider collider)
    {
        GameObject.Destroy(gameObject, AmApplication.GAMEOBJECT_DESTROY_DELAY);
    }
}