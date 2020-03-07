using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics {
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;
        public bool onDoor = false;
        public int grades = 5;
        private float waitHP = 5f;
        private float lastRegenHP = 1f / 5f;

        private float wait = 1f / 5f;
        private float lastRegen = 1f / 5f;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel> ();

        public Bounds Bounds => collider2d.bounds;

        void Awake () {
            health = GetComponent<Health> ();
            audioSource = GetComponent<AudioSource> ();
            collider2d = GetComponent<Collider2D> ();
            spriteRenderer = GetComponent<SpriteRenderer> ();
            animator = GetComponent<Animator> ();
        }

        protected override void Update () {
            if (controlEnabled) {
                move.x = Input.GetAxis ("Horizontal");
                move.y = Input.GetAxis ("Vertical");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown ("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp ("Jump")) {
                    stopJump = true;
                    Schedule<PlayerStopJump> ().player = this;
                }
            } else {
                move.x = 0;
            }
            if (onDoor) {
                if (Input.GetKeyDown (KeyCode.E)) {
                    grades = grades - 1;
                    print (grades);
                }
            } else if (!onDoor) {

            }

            if (health.currentHP < health.maxHP)
                if ((Time.time - waitHP) > lastRegenHP) {
                    health.Increment ();
                    lastRegenHP = Time.time;
                }

            if (health.currentStamina < health.maxStamina)
                if ((Time.time - wait) > lastRegen) {
                    health.IncrementStamina ();
                    lastRegen = Time.time;
                }

            UpdateJumpState ();
            base.Update ();
        }

        void UpdateJumpState () {
            jump = false;
            switch (jumpState) {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded) {
                        Schedule<PlayerJumped> ().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded) {
                        Schedule<PlayerLanded> ().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity () {
            if (jump && IsGrounded) {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            } else if (stopJump) {
                stopJump = false;
                if (velocity.y > 0) {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;


            if (onLadder) {
                gravityModifier = 0f;
                if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space )) {
                    velocity.y = 2f;
                    jumpState = JumpState.InFlight;
                    if (Input.GetKeyDown (KeyCode.Q)) {
                        targetVelocity = move * (maxSpeed * 2);

                    } else {
                        targetVelocity = move * maxSpeed;
                    }
                } else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S))
                    gravityModifier = 1f;
            } else if (!onLadder) {
                jumpState = JumpState.Grounded;
                gravityModifier = 1f;
            }

            animator.SetBool ("grounded", IsGrounded);
            animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

            if (Input.GetKey(KeyCode.Q)) {
                if (health.currentStamina > 0) {
                    targetVelocity = move * (maxSpeed * 2);
                    health.DecrementStamina ();
                } else if (health.currentHP > 0) {
                    targetVelocity = move * (maxSpeed * 2);
                    health.Decrement ();
                } else {
                    targetVelocity = move * maxSpeed;
                }
            } else {
                targetVelocity = move * maxSpeed;
            }

        }

        public enum JumpState {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}