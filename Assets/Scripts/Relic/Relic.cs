using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Relics
{
    public class Relic : MonoBehaviour
    {
        private SpriteRenderer m_spRender;
        private Collider2D m_col;
        private AudioSource m_aSrc;

        [SerializeField, Expandable]
        private Scriptable_RelicBase m_data;
        [SerializeField]
        private RelicRarity m_relicRarity;

        private void Awake()
        {
            m_spRender = GetComponent<SpriteRenderer>();
            m_col = GetComponent<Collider2D>();
            m_aSrc = GetComponent<AudioSource>();
        }

        public void SetSpawnData(RelicRarity relicRarity, Scriptable_RelicBase data)
        {
            m_data = data;
            m_relicRarity = relicRarity;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!m_data)
            {
                Debug.LogError("Missing data on " + name);
                m_spRender.color = Color.red;
                return;
            }

            //equip to player
            if (collision.CompareTag("Player"))
                m_data.ApplyToPlayer(collision.gameObject);

            //delete self
            Destroy(gameObject);
        }
    }
}