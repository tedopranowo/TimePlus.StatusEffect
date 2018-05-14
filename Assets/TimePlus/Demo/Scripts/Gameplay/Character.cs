// Character.cs
// Author: Tedo Pranowo
// This files contains Character class. This class defines the basic behaviour of a character

using UnityEngine;
using TimePlus;

namespace Demo
{
    [System.Serializable]
    public class Character : MonoBehaviour
    {
        //---------------------------------------------------------------------------------------------
        // Variables
        //---------------------------------------------------------------------------------------------
        [SerializeField]
        private int m_maxHealth = 100;
        private int m_health;

        [SerializeField]
        private float m_movementSpeed;

        private SpriteRenderer m_spriteRenderer;
        private Color m_originalColor;
        private bool m_colorChange;
        private float m_colorChangeTimer = 0.0f;
        private const float k_hitColorDuration = 0.25f;

        private StatusEffectHandler m_statusEffectHandler;
        private Animator m_anim;
        private Rigidbody2D m_rigidBody;
        //---------------------------------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------------------------------

        public int health
        {
            get { return m_health; }
        }

        public int maxHealth
        {
            get { return m_maxHealth; }
        }

        public float moveSpeed
        {
            get { return m_movementSpeed; }
            set { m_movementSpeed = value; }
        }

        public StatusEffect[] statusEffects
        {
            get
            {
                return GetComponent<StatusEffectHandler>().statusEffects;
            }
        }

        //---------------------------------------------------------------------------------------------
        // Unity Overrides
        //---------------------------------------------------------------------------------------------
        private void Awake()
        {
            m_health = m_maxHealth;
            m_anim = GetComponent<Animator>();
            m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            m_originalColor = m_spriteRenderer.color;
            m_rigidBody = GetComponent<Rigidbody2D>();
            m_statusEffectHandler = GetComponent<StatusEffectHandler>();
        }


        private void Update()
        {
            OnHitColorTick();
        }

        //---------------------------------------------------------------------------------------------
        // Functions
        //---------------------------------------------------------------------------------------------
        public void ApplyStatusEffect(StatusEffect statusEffect)
        {
            m_statusEffectHandler.Apply(statusEffect);
        }

        public void Move(Vector2 direction)
        {
            m_rigidBody.velocity = new Vector3(direction.x, direction.y, 0);


            if (direction.x > 0)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (direction.x < 0)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);

            }
            if (m_anim != null)
            {
                m_anim.SetBool("isRunning", direction != Vector2.zero);
            }

        }

        public void Heal(int heal)
        {
            m_health += heal;
            if (m_health > m_maxHealth)
            {
                m_health = m_maxHealth;
            }
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("Taking " + damage + " damage");
            m_health -= damage;
            OnHitColor(Color.red);
            if (m_health <= 0)
                Die();
        }

        private void OnHitColor(Color color)
        {
            m_spriteRenderer.color = color;
            m_colorChange = true;
        }

        private void OnHitColorTick()
        {
            if (m_colorChange)
            {
                if (m_colorChangeTimer < k_hitColorDuration)
                {
                    m_colorChangeTimer += Time.deltaTime;
                }
                else
                {
                    m_spriteRenderer.color = m_originalColor;
                    m_colorChangeTimer = 0;
                    m_colorChange = false;
                }
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }

}