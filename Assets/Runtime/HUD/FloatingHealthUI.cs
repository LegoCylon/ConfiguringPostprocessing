using System.Collections;
using Gamekit2D;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Gray
{
    public class FloatingHealthUI : MonoBehaviour
    {
        public Damageable representedDamageable;
        public GameObject healthIconPrefab;
        public HorizontalLayoutGroup healthLayoutGroup;
        public PostProcessVolume postProcessVolume;

        protected Grayscale m_Grayscale;
        protected Animator[] m_HealthIconAnimators;

        protected readonly int m_HashActivePara = Animator.StringToHash ("Active");
        protected readonly int m_HashInactiveState = Animator.StringToHash ("Inactive");

        IEnumerator Start ()
        {
            if(representedDamageable == null || healthLayoutGroup == null)
                yield break;

            yield return null;

            m_HealthIconAnimators = new Animator[representedDamageable.startingHealth];

            for (int i = 0; i < representedDamageable.startingHealth; i++)
            {
                GameObject healthIcon = Instantiate (healthIconPrefab);
                healthIcon.transform.SetParent (healthLayoutGroup.transform);
                m_HealthIconAnimators[i] = healthIcon.GetComponent<Animator> ();

                if (representedDamageable.CurrentHealth < i + 1)
                {
                    m_HealthIconAnimators[i].Play (m_HashInactiveState);
                    m_HealthIconAnimators[i].SetBool (m_HashActivePara, false);
                }
            }

            if (postProcessVolume != null && postProcessVolume.profile != null)
            {
                postProcessVolume.profile.TryGetSettings(outSetting: out m_Grayscale);
            }
        }

        public void ChangeHitPointUI (Damageable damageable)
        {
            if (m_HealthIconAnimators != null)
            {
                for (int i = 0; i < m_HealthIconAnimators.Length; i++)
                {
                    m_HealthIconAnimators[i].SetBool(m_HashActivePara, damageable.CurrentHealth >= i + 1);
                }
            }

            if (m_Grayscale != null)
            {
                m_Grayscale.blend.value = 1.0f - (float)damageable.CurrentHealth / representedDamageable.startingHealth;
            }
        }
    }
}