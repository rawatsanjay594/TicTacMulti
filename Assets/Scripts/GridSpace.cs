using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class GridSpace : GridBase, IGridData
    {
        public string m_GridAcquiredBy;
        private GameManager m_gameManager;
        public TicTacToeScoreManager m_ScoreManager;

        public string OccupiedBy => m_GridAcquiredBy;

        public int GetGridId => m_gridIdInInt;

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
            if (m_gameManager.gameType == GamePlayType.AI)
            {
                if (m_gameManager.playerMove)
                {
                    playerSide = m_gameManager.CurrentPlayerSide;

                    m_ButtonText.text = playerSide;
                    m_Button.interactable = false;
                    m_gameManager.EndTurn(m_gridIdInInt, playerSide);
                }
            }
            else if (m_gameManager.gameType == GamePlayType.Mutiplayer)
            {
                playerSide = m_ScoreManager.GetPlayerSide(GameConstants.currentPlayerName);
                m_ButtonText.text = playerSide;
                m_Button.interactable = false;
                m_gameManager.EndTurn(m_gridIdInInt, playerSide);
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
