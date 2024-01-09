using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DC.Tools;
using UnityEngine.UI;

namespace TicTacToe
{
    public class TicTacToeGridManager : MonoBehaviour
    {

        public int gridRow = 3;
        public int gridColumn = 3;

        private GridLayoutGroup m_gridLayoutGroup;

        public GameObject parentGameObject;

        public GameObject gridPrefab;

        [HideInInspector]public int totalItems;

        private int defaultSpacing = 120;

        public int GetSpacing
        {
            get
            {
                if (totalItems == 9)
                    return defaultSpacing;
                else if (totalItems > 9 && totalItems <= 16)
                    return defaultSpacing / 2;
                else if (totalItems > 16 && totalItems <= 25)
                    return defaultSpacing / 3;

                return 50;
            }

        }

        public GameManager m_GameManager;

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
        }

        public void GenerateGrid()
        {
            if (m_gridLayoutGroup != null)
            {
                for (int i = 0; i < totalItems; i++)
                {
                    GameObject prefab =Instantiate(gridPrefab, parentGameObject.transform);
                    prefab.GetComponent<GridSpace>().m_gridIdInInt = i;
                    prefab.GetComponent<GridSpace>().m_gridIdInString = i.ToString();
                    prefab.GetComponent<GridSpace>().RegisterToGameManager();
                }

                Vector2 spacingVector = new Vector2(GetSpacing, GetSpacing);
                m_gridLayoutGroup.spacing = spacingVector;

                m_GameManager.SetGameControllerReferenceOnButton();
            }
        }

    }
}