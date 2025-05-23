using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Spring")]
    public class Spring : MonoBehaviour, IEntityContact
    {
        public float force = 25f;
        public AudioClip[] clips; // List of possible sound effects

        protected AudioSource m_audio;
        protected Collider m_collider;

        /// <summary>
        /// Applies spring force to a given Player.
        /// </summary>
        /// <param name="player">The Player you want to apply force to.</param>
        public void ApplyForce(Player player)
        {
            if (player.verticalVelocity.y <= 0)
            {
                PlayRandomClip();
                player.verticalVelocity = Vector3.up * force;
            }
        }

        /// <summary>
        /// Plays a random audio clip from the list.
        /// </summary>
        protected void PlayRandomClip()
        {
            if (clips != null && clips.Length > 0)
            {
                int index = Random.Range(0, clips.Length);
                m_audio.PlayOneShot(clips[index]);
            }
        }

        public void OnEntityContact(Entity entity)
        {
            if (!entity.CompareTag(GameTags.Player)) return;

            if (entity is Player player && player.isAlive &&
                BoundsHelper.IsBellowPoint(m_collider, entity.stepPosition))
            {
                ApplyForce(player);
                player.SetJumps(1);
                player.ResetAirSpins();
                player.ResetAirDash();
                player.states.Change<FallPlayerState>();
            }
        }

        protected virtual void Start()
        {
            tag = GameTags.Spring;
            m_collider = GetComponent<Collider>();

            if (!TryGetComponent(out m_audio))
            {
                m_audio = gameObject.AddComponent<AudioSource>();
            }
        }
    }
}