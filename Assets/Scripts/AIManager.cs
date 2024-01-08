using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe
{
    public class AIManager : MonoBehaviour
    {

        private float delay;
        private float threshold = 2f;
        private int randomValue;

        public AIType m_AIType;
        public bool isPlayerMoving;

        public static UnityAction OnRandomValueGenerated;

        public void TogglePlayerMove(bool value) => isPlayerMoving = value;

        public void ResetAllValues() => delay = 1;

        private void Update()
        {
            if (!isPlayerMoving)
            {
                delay += delay * Time.deltaTime;

                if(delay >= threshold) 
                {
                    OnRandomValueGenerated?.Invoke();
                }
            }
        }

        /// <summary>
        /// Need to implement AI value based on available grid and closest to completion
        /// </summary>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public int GetRandomGridValue(int maxCount)
        {
            int value = Random.Range(0, maxCount);
            return value;
        }

    }

    public enum AIType
    {
        Easy,
        Medium,
        Hard
    }
}
