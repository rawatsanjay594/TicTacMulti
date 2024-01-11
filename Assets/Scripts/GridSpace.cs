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

        public string CurrentOccupant => m_GridAcquiredBy;

        public int GetGridCellId => m_gridIdInInt;

        private void OnEnable() { }

        private void OnDisable() => UnRegisterToGameManager();
        

        private void Start()
        {
            m_ScoreManager = FindObjectOfType<TicTacToeScoreManager>();
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
                playerSide = m_ScoreManager.GetPlayerSide(GameConstants.K_CurrentPlayerName);
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

        public void RegisterToGameManager()
        {
            GameManager.RegisterGridBase(this);
        }

        public void UnRegisterToGameManager()
        {
            GameManager.UnRegisterGridBase(this);
        }
    }


}
