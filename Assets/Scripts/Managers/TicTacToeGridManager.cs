using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DC.Tools;
using UnityEngine.UI;
using UnityEngine.Events;
using TicTacToe.Grid;
using TicTacToe.Constants;

namespace TicTacToe.Grid
{
    public class TicTacToeGridManager : MonoBehaviour
    {
        [SerializeField] private int gridRow = 3;
        [SerializeField] private int gridColumn = 3;
        [HideInInspector] public int totalItems;

        private GridLayoutGroup m_gridLayoutGroup;

        public GameObject parentGameObject;
        public GameObject gridPrefab;

        private int m_DefaultSpacing = 120;
        public int GetSpacing
        {
            get
            {
                if (totalItems == 9)
                    return m_DefaultSpacing;
                else if (totalItems > 9 && totalItems <= 16)
                    return m_DefaultSpacing / 2;
                else if (totalItems > 16 && totalItems <= 25)
                    return m_DefaultSpacing / 3;

                return 50;
            }
        }

        public static UnityAction<int> InitializeTotalGrid;

        private GameManager m_GameManager;

        private void OnEnable()
        {
            GameManager.OnGameInitialized += GenerateGrid;
        }

        private void OnDisable()
        {
            GameManager.OnGameInitialized -= GenerateGrid;
        }

        private void Start()
        {
            m_GameManager = FindObjectOfType<GameManager>();
            m_gridLayoutGroup = parentGameObject.GetComponent<GridLayoutGroup>();
            totalItems = gridRow * gridColumn;

            GameConstants.rowSize = gridRow;
            GameConstants.columnSize = gridColumn;

            InitializeTotalGrid?.Invoke(totalItems);
        }

        public void GenerateGrid()
        {
            if (m_gridLayoutGroup != null)
            {
                for (int i = 0; i < totalItems; i++)
                {
                    GameObject prefab =Instantiate(gridPrefab, parentGameObject.transform);
                    prefab.GetComponent<GridCell>().m_gridIdInInt = i;
                    prefab.GetComponent<GridCell>().m_gridIdInString = i.ToString();
                    prefab.GetComponent<GridCell>().RegisterToGameManager();
                }

                Vector2 spacingVector = new Vector2(GetSpacing, GetSpacing);
                m_gridLayoutGroup.spacing = spacingVector;

                m_GameManager.SetGameControllerReferenceOnButton();
            }
        }

    }
}