using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TicTacToe
{
    public class AIManager : MonoBehaviour
    {

        private float delay;
        private float threshold = 5f;
        private int randomValue;
        private int m_TotalGridListCount = 0;

        public AIType m_AIType;
        public bool isPlayerMoving;
        public List<int> gridList = new List<int>();

        public static UnityAction OnRandomValueGenerated;

        public void TogglePlayerMove(bool value) => isPlayerMoving = value;

        public void ResetAllValues() => delay = 1;

        private void OnEnable()
        {
            TicTacToeGridManager.InitializeTotalGrid += InitializeTotalGrid;
            GameManager.OnGridSelected += UpdateTotalGridList;
            GameManager.OnGameRestarted += ResetGameGriddata;
        }

        private void OnDisable()
        {
            TicTacToeGridManager.InitializeTotalGrid -= InitializeTotalGrid;
            GameManager.OnGridSelected -= UpdateTotalGridList;
            GameManager.OnGameRestarted -= ResetGameGriddata;
        }

        private void InitializeTotalGrid(int totalListCount)
        {
            m_TotalGridListCount = totalListCount;
            for (int i = 0; i < totalListCount; i++) 
                gridList.Add(i);
        
        }

        private void UpdateTotalGridList(int gridId)
        {
            if(gridList.Count>0 && gridList.Contains(gridId))
                gridList.Remove(gridId);
        }

        private void ResetGameGriddata()
        {
            if(gridList.Count>0)
                gridList.Clear();

            for (int i = 0;i< m_TotalGridListCount; i++)
            {
                gridList.Add(i);
            }
        }


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
        public int GetRandomGridValue()
        {
            int value = Random.Range(0, gridList.Count);
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
