// PlayerController.cs
// Author: Tedo Pranowo
// This file contains PlayerController class. This class convert user input into character controls

using UnityEngine;

namespace Demo
{
    public class PlayerController : Controller
    {
        //---------------------------------------------------------------------------------------------
        // Singleton
        //---------------------------------------------------------------------------------------------
        private static PlayerController s_instance = null;
        public static PlayerController instance
        {
            set
            {
                Debug.Assert(value != null);

                if (s_instance != null)
                {
                    Debug.LogWarning("Singleton object cannot be instatiated twice!");
                    Destroy(value);
                    return;
                }

                s_instance = value;
            }
            get
            {
                return s_instance;
            }
        }

        //---------------------------------------------------------------------------------------------
        // Unity Monobehavior
        //---------------------------------------------------------------------------------------------
        private void Awake()
        {
            instance = this;
        }
        private void Update()
        {
            UpdateMovement();
        }

        //---------------------------------------------------------------------------------------------
        // Functions
        //---------------------------------------------------------------------------------------------
        public void UpdateMovement()
        {
            float xMovPos;
            float yMovPos;
            xMovPos = Input.GetAxisRaw("Horizontal") * character.moveSpeed;
            yMovPos = Input.GetAxisRaw("Vertical") * character.moveSpeed;


            Vector2 moveVector = new Vector2(xMovPos, yMovPos);
            character.Move(moveVector);
        }
    }

}