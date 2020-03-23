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
        public AudioClip sprintAudio;
        public AudioClip audioHealth;
        public AudioClip audioTimer;
        public AudioClip audioTimerRunningOut;
        public AudioSource audioSource;
        public AudioSource audioBoost;
        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        private float waitHP = 5f;
        private float lastRegenHP = 1f / 5f;
        private float wait = 1f / 5f;
        private float lastRegen = 1f / 5f;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>

        public JumpState jumpState = JumpState.Grounded;
        public Collider2D collider2d;
        public GameObject img;
        public GameObject img1;
        public Health health;

        private bool stopJump;
        public bool boostHealth = false;
        public bool boostTimer = false;
        public bool controlEnabled = true;
        public bool onDoor;
        public bool DoorControl = false;
        private bool ObstacleController = false;
        bool jump;
        public bool collision = false;

        public string DoorNum;

        public int ObstacleNum;
        public int grades = 5;

        private List<int> ObstacleUsed = new List<int> ();

        public GameObject obs1;
        public GameObject obs2;
        public GameObject obs3;
        public GameObject obs4;
        public GameObject obs5;
        public GameObject obs6;
        public GameObject obs7;
        public GameObject obs8;
        public GameObject obs9;
        public GameObject obs10;
        public GameObject obs11;
        public GameObject obs12;
        public GameObject obs13;
        public GameObject obs14;
        public GameObject obs15;
        public GameObject obs16;
        public GameObject obs17;
        public GameObject obs18;
        public GameObject obs19;
        public GameObject obs20;
        public GameObject obs21;
        public GameObject obs22;
        public GameObject obs23;

        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel> ();
        public Color originalColor;

        public Bounds Bounds => collider2d.bounds;

        void Awake () {
            health = GetComponent<Health> ();
            collider2d = GetComponent<Collider2D> ();
            spriteRenderer = GetComponent<SpriteRenderer> ();
            animator = GetComponent<Animator> ();
            var player = model.player;
            player.controlEnabled = false;
            RandomDoor ();
            originalColor = spriteRenderer.color;
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
                if (Input.GetKeyDown (KeyCode.E) || Input.GetButtonDown ("Fire3")) {
                    if (DoorControl) {
                        grades = grades - 1;
                        onDoor = false;
                        RandomDoor ();
                        RandomObstacle ();
                    }
                }
            } else if (!onDoor) {
                ObstacleController = false;
            }
            if (boostHealth) {
                audioBoost.clip = audioHealth;
                audioBoost.Play ();
                boostHealth = false;
            }
            if (boostTimer) {
                audioBoost.clip = audioTimer;
                audioBoost.Play ();
                boostTimer = false;
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

            if (grades == 0) {
                health.Victory ();
                animator.SetBool ("VictoryAnim", true);
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
                velocity.y = jumpTakeOffSpeed * model.jumpModifier / 1.5f;
                jump = false;
            } else if (stopJump) {
                stopJump = false;
                if (velocity.y > 0) {
                    velocity.y = velocity.y * model.jumpDeceleration / 1.5f;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            if (onLadder) {
                gravityModifier = 0f;
                if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.Space) || Input.GetAxisRaw ("Vertical") > 0) {
                    velocity.y = 5f;
                    jumpState = JumpState.InFlight;
                    if (Input.GetKeyDown (KeyCode.Q)) {
                        targetVelocity = move * (maxSpeed * 2);

                    } else {
                        targetVelocity = move * maxSpeed;
                    }
                } else if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S) || Input.GetAxisRaw ("Vertical") < 0) {
                    gravityModifier = 1f;
                    S_pressed = true;
                } else if (Input.GetKeyUp (KeyCode.DownArrow) || Input.GetKeyUp (KeyCode.S) || Input.GetAxisRaw ("Vertical") == 0) {
                    S_pressed = false;
                }
            } else if (!onLadder) {
                jumpState = JumpState.Grounded;
                gravityModifier = 1f;
            }

            animator.SetBool ("grounded", IsGrounded);
            animator.SetFloat ("velocityX", Mathf.Abs (velocity.x) / maxSpeed);
            if ((Input.GetKey (KeyCode.LeftShift) || Input.GetButtonDown ("Fire4")) && (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D) || Input.GetAxisRaw ("Horizontal") == 1 || Input.GetAxisRaw ("Horizontal") == -1)) {
                print ("q down");
                audioSource.clip = sprintAudio;
                audioSource.Play ();
            }
            if (Input.GetKeyUp (KeyCode.LeftShift) || Input.GetButtonUp ("Fire4")) {
                print ("q up");
                audioSource.Stop ();
            }
            if ((Input.GetKey (KeyCode.LeftShift) || Input.GetButton ("Fire4")) && (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D) || Input.GetAxisRaw ("Horizontal") == 1 || Input.GetAxisRaw ("Horizontal") == -1)) {
                print ("SPEED");
                if (health.currentStamina > 0) {
                    targetVelocity = move * (maxSpeed * 2);
                    health.DecrementStamina ();
                    audioSource.clip = sprintAudio;
                    audioSource.Play ();
                } else if (health.currentHP > 0) {
                    targetVelocity = move * (maxSpeed * 2);
                    health.Decrement ();
                    if (spriteRenderer.color==originalColor)
                    {
                        spriteRenderer.color = new Color(2,0,0);
                    }
                    else
                    {
                        spriteRenderer.color = originalColor;
                    }
                   // img.SetActive (true);
                } else {
                    targetVelocity = move * maxSpeed;
                   // img.SetActive (false);
                }
            } else if (collision) {
                targetVelocity = move * maxSpeed;
                spriteRenderer.color = new Color(2, 0, 0);
               // img1.SetActive (true);
                StartCoroutine (waiter ());
            } else {
                targetVelocity = move * maxSpeed;
                spriteRenderer.color = originalColor;
                //img1.SetActive (false);
               // img.SetActive (false);
            }

        }

        void RandomDoor () {
            int i = Random.Range (1, 44);
            switch (i) {
                case 1:
                    DoorNum = "001";
                    break;
                case 2:
                    DoorNum = "002";
                    break;
                case 3:
                    DoorNum = "003";
                    break;
                case 4:
                    DoorNum = "004";
                    break;
                case 5:
                    DoorNum = "005";
                    break;
                case 6:
                    DoorNum = "006";
                    break;
                case 7:
                    DoorNum = "007";
                    break;
                case 8:
                    DoorNum = "008";
                    break;
                case 9:
                    DoorNum = "101";
                    break;
                case 10:
                    DoorNum = "102";
                    break;
                case 11:
                    DoorNum = "103";
                    break;
                case 12:
                    DoorNum = "104";
                    break;
                case 13:
                    DoorNum = "105";
                    break;
                case 14:
                    DoorNum = "106";
                    break;
                case 15:
                    DoorNum = "107";
                    break;
                case 16:
                    DoorNum = "108";
                    break;
                case 17:
                    DoorNum = "109";
                    break;
                case 18:
                    DoorNum = "110";
                    break;
                case 19:
                    DoorNum = "111";
                    break;
                case 20:
                    DoorNum = "112";
                    break;
                case 21:
                    DoorNum = "113";
                    break;
                case 22:
                    DoorNum = "201";
                    break;
                case 23:
                    DoorNum = "202";
                    break;
                case 24:
                    DoorNum = "203";
                    break;
                case 25:
                    DoorNum = "204";
                    break;
                case 26:
                    DoorNum = "205";
                    break;
                case 27:
                    DoorNum = "206";
                    break;
                case 28:
                    DoorNum = "207";
                    break;
                case 29:
                    DoorNum = "208";
                    break;
                case 30:
                    DoorNum = "209";
                    break;
                case 31:
                    DoorNum = "210";
                    break;
                case 32:
                    DoorNum = "211";
                    break;
                case 33:
                    DoorNum = "301";
                    break;
                case 34:
                    DoorNum = "302";
                    break;
                case 35:
                    DoorNum = "303";
                    break;
                case 36:
                    DoorNum = "304";
                    break;
                case 37:
                    DoorNum = "305";
                    break;
                case 38:
                    DoorNum = "306";
                    break;
                case 39:
                    DoorNum = "307";
                    break;
                case 40:
                    DoorNum = "308";
                    break;
                case 41:
                    DoorNum = "309";
                    break;
                case 42:
                    DoorNum = "310";
                    break;
                case 43:
                    DoorNum = "311";
                    break;
            }
        }

        void RandomObstacle () {
            int i = Random.Range (1, 24);
            print ("size " + ObstacleUsed.Count);
            if (ObstacleUsed.Count != 0) {
                while (!ObstacleController) {
                    if (ObstacleUsed.Contains (i)) {
                        print ("Try again new trap number");
                        i = Random.Range (1, 24);
                    } else if (ObstacleUsed.Count == 23) {
                        ObstacleController = true;
                    } else {
                        ObstacleController = true;
                        print ("obs number " + i);
                        ObstacleUsed.Add (i);
                        obstacleChosen (i);
                    }
                }
            } else if (ObstacleUsed.Count == 0) {
                print ("obs number " + i);
                ObstacleUsed.Add (i);
                obstacleChosen (i);
            }
        }

        void obstacleChosen (int i) {
            switch (i) {
                case 1:
                    obs1.SetActive (true);

                    break;
                case 2:
                    obs2.SetActive (true);
                    break;
                case 3:
                    obs3.SetActive (true);
                    break;
                case 4:
                    obs4.SetActive (true);
                    break;
                case 5:
                    obs5.SetActive (true);
                    break;
                case 6:
                    obs6.SetActive (true);
                    break;
                case 7:
                    obs7.SetActive (true);
                    break;
                case 8:
                    obs8.SetActive (true);
                    break;
                case 9:
                    obs9.SetActive (true);
                    break;
                case 10:
                    obs10.SetActive (true);
                    break;
                case 11:
                    obs11.SetActive (true);
                    break;
                case 12:
                    obs12.SetActive (true);
                    break;
                case 13:
                    obs13.SetActive (true);
                    break;
                case 14:
                    obs14.SetActive (true);
                    break;
                case 15:
                    obs15.SetActive (true);
                    break;
                case 16:
                    obs16.SetActive (true);
                    break;
                case 17:
                    obs17.SetActive (true);
                    break;
                case 18:
                    obs18.SetActive (true);
                    break;
                case 19:
                    obs19.SetActive (true);
                    break;
                case 20:
                    obs20.SetActive (true);
                    break;
                case 21:
                    obs21.SetActive (true);
                    break;
                case 22:
                    obs22.SetActive (true);
                    break;
                case 23:
                    obs23.SetActive (true);
                    break;
            }
        }
        IEnumerator waiter () {
            yield return new WaitForSeconds (1);
            collision = false;
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