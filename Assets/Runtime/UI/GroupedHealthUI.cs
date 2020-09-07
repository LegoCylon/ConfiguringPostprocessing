using System.Collections;
using Gamekit2D;
using UnityEngine;
using UnityEngine.UI;

namespace ConfiguringPostprocessing
{
    public class GroupedHealthUI : MonoBehaviour
    {
        public Damageable representedDamageable;
        public GameObject healthIconPrefab;
        public LayoutGroup layoutGroup;

        protected Animator[] m_HealthIconAnimators;

        protected readonly int m_HashActivePara = Animator.StringToHash ("Active");
        protected readonly int m_HashInactiveState = Animator.StringToHash ("Inactive");

        IEnumerator Start ()
        {
            if(representedDamageable == null || layoutGroup == null)
                yield break;

            yield return null;

            m_HealthIconAnimators = new Animator[representedDamageable.startingHealth];

            for (int i = 0; i < representedDamageable.startingHealth; i++)
            {
                GameObject healthIcon = Instantiate (healthIconPrefab, layoutGroup.transform);
                m_HealthIconAnimators[i] = healthIcon.GetComponent<Animator> ();

                if (representedDamageable.CurrentHealth < i + 1)
                {
                    m_HealthIconAnimators[i].Play (m_HashInactiveState);
                    m_HealthIconAnimators[i].SetBool (m_HashActivePara, false);
                }
            }
        }

        public void ChangeHitPointUI (Damageable damageable)
        {
            if(m_HealthIconAnimators == null)
                return;

            for (int i = 0; i < m_HealthIconAnimators.Length; i++)
            {
                m_HealthIconAnimators[i].SetBool(m_HashActivePara, damageable.CurrentHealth >= i + 1);
            }
        }
    }
}