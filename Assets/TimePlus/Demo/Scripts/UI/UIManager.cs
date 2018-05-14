// UIManager.cs
// Author: Tedo Pranowo
// This class contains UIManager class. This class handles updating the Health and StatusEffect
// user interface

using UnityEngine;
using UnityEngine.UI;
using TimePlus;

namespace Demo
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HealthUI m_healthUI = null;
        [SerializeField] private StatusEffectUI m_statusEffectUI = null;

        // Use this for initialization
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            //[TODO]: Do not update health every frame
            UpdateHealth();
        }

        public void UpdateHealth()
        {
            m_healthUI.Update();
            m_statusEffectUI.Update();
        }
    }

    [System.Serializable]
    class HealthUI
    {
        [SerializeField] private Image m_image = null;
        [SerializeField] private Text m_curHealthText = null;
        [SerializeField] private Text m_maxHealthText = null;

        public void Update()
        {
            //Get the player character
            Character playerCharacter = PlayerController.instance ? PlayerController.instance.character : null;

            //Get the character health
            int curHealth = playerCharacter ? playerCharacter.health : 0;
            int maxHealth = playerCharacter ? playerCharacter.maxHealth : 0;

            //Update the health UI
            m_curHealthText.text = curHealth.ToString();
            m_maxHealthText.text = maxHealth.ToString();
            m_image.fillAmount = (float)curHealth / maxHealth;
        }
    }

    [System.Serializable]
    class StatusEffectUI
    {
        [SerializeField] private Image[] m_images = null;

        public void Update()
        {
            StatusEffect[] statusEffects = PlayerController.instance.character.statusEffects;
            int i = 0;

            //Clear all the images
            foreach (Image image in m_images)
            {
                image.color = Color.clear;
            }

            //Update all status effect images
            foreach (StatusEffect statusEffect in statusEffects)
            {
                m_images[i].sprite = statusEffect.icon;
                m_images[i].color = Color.white;
                ++i;
            }
        }
    }
}