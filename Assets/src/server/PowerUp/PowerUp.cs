using UnityEngine;
using System.Collections.Generic;


namespace Amucuga
{
    /// <summary>
    /// The current state of the powerup
    /// </summary>
    public enum PowerUpState
    {
        /// <summary>
        /// The powerup has been created, but it isn't yet attached to a player (it's still attached to the PowerUp prefab)
        /// </summary>
        CREATED,

        /// <summary>
        /// The powerup has been collected by a player, and it's attached to him.
        /// </summary>
        ATTACHED,

        /// <summary>
        /// The powerup has spent all his lifetime, and its effect is no longer active. It must be destroyed by the player
        /// </summary>
        DEAD
    }

    /// <summary>
    /// The PowerUp
    /// </summary>
    public abstract class PowerUp
    {
        /// <summary>
        /// The remainingTime of the PowerUp
        /// </summary>
        public float RemainingTime {get; private set;}

        /// <summary>
        /// The total lifeTime of the PowerUp
        /// </summary>
        private float _lifeTime;

        /// <summary>
        /// The player
        /// </summary>
        public ConnectedPlayer AttachedPlayer { get; private set; }

        /// <summary>
        /// The powerup state
        /// </summary>
        public PowerUpState State { get; private set; }

        /// <summary>
        /// The Color for the powerup
        /// </summary>
        public Color PowerUpColor { get; protected set; }

        /// <summary>
        /// Indicates wether or not the power up is cumulative
        /// </summary>
        public bool IsCumulative { get; protected set; }

        /// <summary>
        /// The powerup name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// The remaining life time of the powerup
        /// </summary>
        public float CountDown
        {
            get
            {
                return RemainingTime;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifeTime">The powerup lifeTime</param>
        public PowerUp(float lifeTime)
        {
            _lifeTime = lifeTime;
            RemainingTime = lifeTime;
            State = PowerUpState.CREATED;
        }

        /// <summary>
        /// Updates the powerup
        /// </summary>
        public void Update(float deltaTime)
        {
            if (State == PowerUpState.ATTACHED)
            {
                RemainingTime -= deltaTime;
                if (RemainingTime >= 0)
                    UpdatePowerUpEffect();
                else
                {
                    DisablePowerUpEffect();
                    State = PowerUpState.DEAD;
                }
            }
        }

        /// <summary>
        /// Attaches the powerUp to the player that just collected it.
        /// </summary>
        /// <param name="player">The player that collected the powerup</param>
        public void CollectedByPlayer(ConnectedPlayer player)
        {
            this.AttachedPlayer = player;
            State = PowerUpState.ATTACHED;
            EnablePowerUpEffect();
        }

        /// <summary>
        /// Enables the powerup effect.
        /// </summary>
        protected abstract void EnablePowerUpEffect();

        /// <summary>
        /// Disables the powerup effect to the player.
        /// </summary>
        protected abstract void DisablePowerUpEffect();

        /// <summary>
        /// Updates the powerup effect
        /// </summary>
        protected virtual void UpdatePowerUpEffect() { }

        /// <summary>
        /// Indicates wether or not the power ups are of the same type
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(PowerUp left, PowerUp right)
        {
            return left.GetType() == right.GetType();
        }

        /// <summary>
        /// Indicates wether or not the power ups aren't of the same type
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(PowerUp left, PowerUp right)
        {
            return left.GetType() != right.GetType();
        }

        /// <summary>
        /// Overrides Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is PowerUp)
                return this == obj;
            else
                return false;
        }

        /// <summary>
        /// Overrides GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Resets the current powerup
        /// </summary>
        public void Reset()
        {
            RemainingTime = _lifeTime;
        }

        /// <summary>
        /// Immediately cancels the effect of the powerup and terminates its life-cycle
        /// </summary>
        public virtual void TerminateImmediate()
        {
            DisablePowerUpEffect();
            RemainingTime = 0;
            State = PowerUpState.DEAD;
        }

        /// <summary>
        /// Serializes this powerup in a string
        /// </summary>
        public string AmSerialize()
        {
            string serialized = "Name:" + this.GetType().ToString();
            serialized += ";CountDown:" + this.CountDown;
            return serialized;
        }

        /// <summary>
        /// Serializes a collection of powerups
        /// </summary>
        /// <param name="powerups"></param>
        /// <returns></returns>
        public static string AmSerializePowerUpCollection(ICollection<PowerUp> powerups)
        {
            string serialized = null;
            foreach (PowerUp p in powerups)
            {
                if (serialized != null)
                    serialized += ",";
                else
                    serialized = "PowerUps[";
                serialized += p.AmSerialize();
            }
            serialized += "]";
            return serialized;
        }
    }
}