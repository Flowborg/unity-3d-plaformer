using System.Collections;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
    [AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Dash Powerup")]
    public class GetDash : Collectable
    {
        public override void Collect(Player player)
        {
            if (!m_vanished && !m_ghosting)
            {
                if (!hidden)
                {
                    Vanish();

                    if (particle != null)
                    {
                        particle.Play();
                    }
                }
                else
                {
                    StartCoroutine(QuickShowRoutine());
                    return;
                }

                // Start the custom routine instead of the base one
                StartCoroutine(DashCollectRoutine(player));
            }
        }

        private IEnumerator DashCollectRoutine(Player player)
        {
            var inputManager = player.GetComponent<PlayerInputManager>();
            if (inputManager != null)
            {
                inputManager.dashEnabled = true;
                Debug.Log("✅ Dash Unlocked (no star count)!");
            }

            // Play audio if available
            if (m_audio != null && clip != null)
            {
                m_audio.Stop();
                m_audio.PlayOneShot(clip);
            }

            // Optionally invoke your own event here (not onCollect!)
            yield return new WaitForSeconds(0.1f);
        }
    }
}