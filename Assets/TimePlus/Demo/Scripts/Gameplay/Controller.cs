// Controller.cs
// Author: Tedo Pranowo
// This class contains Controller class. This is a base class inheritted by classes which
// controls Character class (Player / AI)

using UnityEngine;

namespace Demo
{
    [RequireComponent(typeof(Character))]
    public abstract class Controller : MonoBehaviour
    {
        private Character m_character;
        public Character character
        {
            set
            {
                m_character = value;
            }
            get
            {
                if (m_character == null)
                    m_character = GetComponent<Character>();

                return m_character;
            }
        }
    }

}