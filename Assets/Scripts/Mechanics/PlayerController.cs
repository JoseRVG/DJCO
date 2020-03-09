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
        public bool onDoor;
        public bool DoorControl = false;
        public int DoorNum;
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
            RandomDoor ();
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
                    if (DoorControl) {
                        grades = grades - 1;
                        onDoor = false;
                        RandomDoor ();
                    }
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
                if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.Space)) {
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

            if (Input.GetKey (KeyCode.Q) && (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow))) {
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

        void RandomDoor () {
            int i = Random.Range (1, 44);
            switch (i) {
                case 1:
                    DoorNum = 001;
                    break;
                case 2:
                    DoorNum = 002;
                    break;
                case 3:
                    DoorNum = 003;
                    break;
                case 4:
                    DoorNum = 004;
                    break;
                case 5:
                    DoorNum = 005;
                    break;
                case 6:
                    DoorNum = 006;
                    break;
                case 7:
                    DoorNum = 007;
                    break;
                case 8:
                    DoorNum = 008;
                    break;
                case 9:
                    DoorNum = 101;
                    break;
                case 10:
                    DoorNum = 102;
                    break;
                case 11:
                    DoorNum = 103;
                    break;
                case 12:
                    DoorNum = 104;
                    break;
                case 13:
                    DoorNum = 105;
                    break;
                case 14:
                    DoorNum = 106;
                    break;
                case 15:
                    DoorNum = 107;
                    break;
                case 16:
                    DoorNum = 108;
                    break;
                case 17:
                    DoorNum = 109;
                    break;
                case 18:
                    DoorNum = 110;
                    break;
                case 19:
                    DoorNum = 111;
                    break;
                case 20:
                    DoorNum = 112;
                    break;
                case 21:
                    DoorNum = 113;
                    break;
                case 22:
                    DoorNum = 201;
                    break;
                case 23:
                    DoorNum = 202;
                    break;
                case 24:
                    DoorNum = 203;
                    break;
                case 25:
                    DoorNum = 204;
                    break;
                case 26:
                    DoorNum = 205;
                    break;
                case 27:
                    DoorNum = 206;
                    break;
                case 28:
                    DoorNum = 207;
                    break;
                case 29:
                    DoorNum = 208;
                    break;
                case 30:
                    DoorNum = 209;
                    break;
                case 31:
                    DoorNum = 210;
                    break;
                case 32:
                    DoorNum = 211;
                    break;
                case 33:
                    DoorNum = 301;
                    break;
                case 34:
                    DoorNum = 302;
                    break;
                case 35:
                    DoorNum = 303;
                    break;
                case 36:
                    DoorNum = 304;
                    break;
                case 37:
                    DoorNum = 305;
                    break;
                case 38:
                    DoorNum = 306;
                    break;
                case 39:
                    DoorNum = 307;
                    break;
                case 40:
                    DoorNum = 308;
                    break;
                case 41:
                    DoorNum = 309;
                    break;
                case 42:
                    DoorNum = 310;
                    break;
                case 43:
                    DoorNum = 311;
                    break;
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