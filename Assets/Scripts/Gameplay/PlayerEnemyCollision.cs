using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay {

    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision> {
        public EnemyController enemy;
        public PlayerController player;

        PlatformerModel model = Simulation.GetModel<PlatformerModel> ();

        public override void Execute () {
            var willHurtEnemy = player.Bounds.center.y >= enemy.Bounds.max.y;

            if (willHurtEnemy) {
                var enemyHealth = enemy.GetComponent<Health> ();
                if (enemyHealth != null) {
                    enemyHealth.Decrement ();
                    if (!enemyHealth.IsAlive) {
                        Schedule<EnemyDeath> ().enemy = enemy;
                        player.Bounce (2);
                        player.collision = true;
                    } else {
                        player.Bounce (7);
                        player.collision = true;
                    }
                } else {
                    Schedule<EnemyDeath> ().enemy = enemy;
                    player.Bounce (2);
                    player.collision = true;
                }
            } else if (player.Bounds.max.x >= enemy.Bounds.max.x || player.Bounds.min.x <= enemy.Bounds.min.x || player.Bounds.max.x >= enemy.Bounds.min.x || player.Bounds.min.y <= enemy.Bounds.min.y || player.Bounds.max.y >= enemy.Bounds.min.y) {
                if (player.health.currentHP == 1)
                    Schedule<PlayerDeath> ();
                else {
                    player.health.Decrement ();
                    player.collision = true;
                }
            }
        }
    }
}