using System;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Core;
using Platformer.Model;
using UnityEngine.UI;

namespace Platformer.Mechanics {
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour {
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public Slider myHealthBar;
        public Slider myStaminaBar;
        public int maxHP;
        public int maxStamina;
        private int regeneration = 1;
        [SerializeField]
        private GameObject gameOverUI;
        private Timer timer;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => currentHP > 0;

        public int currentHP;
        public int currentStamina;
        public Animator anim;

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>

        void Start () {
            timer = FindObjectOfType<Timer> ();
        }

        public void Increment () {
            currentHP += regeneration;
            if (currentHP > maxHP)
                currentHP = maxHP;

            myHealthBar.value = currentHP;
        }

        public void IncrementStamina () {
            currentStamina += regeneration;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            myStaminaBar.value = currentStamina;
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement () {
            currentHP = Mathf.Clamp (currentHP - 1, 0, maxHP);
            myHealthBar.value = currentHP;
            if (currentHP == 0) {
                var ev = Schedule<HealthIsZero> ();
                ev.health = this;
                timer.timeStart = 0;
                EndGame ();
            }
        }

        public void DecrementStamina () {
            currentStamina = Mathf.Clamp (currentStamina - 1, 0, maxStamina);
            myStaminaBar.value = currentStamina;
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die () {
            while (currentHP > 0)
                Decrement ();
        }

        public void EndGame () {
            Debug.Log ("END");
            anim.SetBool ("End", true);
            PlatformerModel model = Simulation.GetModel<PlatformerModel> ();
            var player = model.player;
            player.controlEnabled = false;
        }

        void Awake () {
            myHealthBar.maxValue = maxHP;
            myStaminaBar.maxValue = maxStamina;
            currentHP = maxHP;
            currentStamina = maxStamina;
        }
    }
}