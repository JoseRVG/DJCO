﻿using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics {
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent (typeof (AnimationController), typeof (Collider2D))]
    public class EnemyController : MonoBehaviour {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;
        public bool onLadder = false;
        public Bounds Bounds => _collider.bounds;

        // public bool onLadder=true;

        void Awake () {
            control = GetComponent<AnimationController> ();
            _collider = GetComponent<Collider2D> ();
            _audio = GetComponent<AudioSource> ();
            spriteRenderer = GetComponent<SpriteRenderer> ();
        }

        void OnCollisionEnter2D (Collision2D collision) {
            var player = collision.gameObject.GetComponent<PlayerController> ();
            if (player != null) {
                var ev = Schedule<PlayerEnemyCollision> ();
                ev.player = player;
                ev.enemy = this;
                player.health.currentHP = player.health.currentHP - 25;
            }
        }

        void Update () {
            if (path != null) {
                if (mover == null) mover = path.CreateMover (control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp (mover.Position.x - transform.position.x, -1, 1);
            }
            if (onLadder) {
                GetComponent<Rigidbody2D> ().gravityScale = 0f;
                control.maxSpeed = control.maxSpeed * 0.5f;
            } else if (!onLadder) {
                GetComponent<Rigidbody2D> ().gravityScale = 1f;
            }
            transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
        }

    }
}