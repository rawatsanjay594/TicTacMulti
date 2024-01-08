using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class GridSpace : GridBase, IGridData
    {
        public string m_GridAcquiredBy;
        private GameManager m_gameManager;

        public string OccupiedBy => m_GridAcquiredBy;

        public string GetGridId => m_gridId;

        private void OnEnable()
        {
            GameManager.OnGameInitialized += RegisterToGameManager;
        }

        private void OnDisable()
        {
            GameManager.OnGameInitialized -= RegisterToGameManager;
            UnRegisterToGameManager();
        }


        public void SetSpace()
        {
            if (m_gameManager.playerMove)
            {
                m_ButtonText.text = m_gameManager.CurrentPlayerSide;
                m_Button.interactable = false;
                m_gameManager.EndTurn();
            }
        }

        public void SetGameControllerReference(GameManager manager ) => m_gameManager = manager;

        public override void SetDelegate(IGridData gridDelegate)
        {
            GridDelegate = gridDelegate;
        }

        private void RegisterToGameManager()
        {
            GameManager.RegisterGridBase(this);
        }

        private void UnRegisterToGameManager()
        {
            GameManager.UnRegisterGridBase(this);
        }
    }


}
