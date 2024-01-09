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

        public int totalItems;

        public int defaultSpacing = 120;

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

        private void Start()
        {
            m_gridLayoutGroup = parentGameObject.GetComponent<GridLayoutGroup>();
            totalItems = gridRow * gridColumn;
        }

        [Button("GenerateGrid")]
        public void GenerateGrid()
        {
            if (m_gridLayoutGroup != null)
            {
                for (int i = 0; i < totalItems; i++)
                {
                    Instantiate(gridPrefab, parentGameObject.transform);
                }

                Vector2 spacingVector = new Vector2(GetSpacing, GetSpacing);
                m_gridLayoutGroup.spacing = spacingVector;
            }
        }

    }
}