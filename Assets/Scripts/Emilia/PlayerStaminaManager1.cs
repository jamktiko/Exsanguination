using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiliaScripts
{
    public class PlayerStaminaManager : MonoBehaviour
    {

        private int currentStamina;
        private int maxStamina = 100;

        void Awake()
        {
            currentStamina = maxStamina;
            Debug.Log("Updated Player Health to MAX: " + currentStamina);
        }

        /// <summary>
        /// Returns the  current player stamina
        /// </summary>
        /// <returns>Int</returns>
        public int CurrentPlayerStamina()
        {
            return currentStamina;
        }


        /// <summary>
        /// Updates the player stamina. Input a negative number for less stamina and a positive for more.
        /// </summary>
        /// <param name="staminaNumber">Int</param>
        public void UpdatePlayerStamina(int staminaNumber)
        {
            if (staminaNumber != 0 && currentStamina > 0 && currentStamina <= maxStamina)
            {
                currentStamina += staminaNumber;
                if (currentStamina >= maxStamina) {
                    currentStamina = maxStamina;
                }
                Debug.Log("Updating player stamina with modifier: " + staminaNumber);
            }
            else if (currentStamina <= 0)
            {
                //cannot do shit
            }
            else if (currentStamina > maxStamina) // avoid overheal
            {
                currentStamina = maxStamina;
                Debug.Log("Current stamina over max, setting to max stamina: " + currentStamina);
            }

            Debug.Log("Current Player Stamina: " + currentStamina);
        }

    }
}