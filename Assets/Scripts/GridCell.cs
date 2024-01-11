using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.DataClass;
using TicTacToe.Constants;

namespace TicTacToe.Grid
{
    public class GridCell : GridCellBase, IGridData
    {
        public string m_GridAcquiredBy;    
        public string CurrentOccupant => m_GridAcquiredBy;

        public int GetGridCellId => m_gridIdInInt;

        private GameManager m_gameManager;
        private TicTacToeScoreManager m_ScoreManager;

        private void OnEnable() { }

        private void OnDisable() => UnRegisterToGameManager();
        
        private void Start()
        {
            m_ScoreManager = FindObjectOfType<TicTacToeScoreManager>();
        }

        public void PopulateData()
        {
            if (m_gameManager.gameType == GamePlayType.AI)            
                PlayerMove();            
            else if (m_gameManager.gameType == GamePlayType.Mutiplayer)
                 MultiplayerMove();            
        }

        private void PlayerMove()
        {
            if (m_gameManager.playerMove)
            {
                playerSide = m_gameManager.CurrentPlayerSide;

                m_ButtonText.text = playerSide;
                m_Button.interactable = false;
                m_gameManager.EndTurn(m_gridIdInInt, playerSide);
            }
        }

        private void MultiplayerMove()
        {
            playerSide = m_ScoreManager.GetPlayerSide(GameConstants.K_CurrentPlayerName);
            m_ButtonText.text = playerSide;
            m_Button.interactable = false;
            m_gameManager.EndTurn(m_gridIdInInt, playerSide);
        }

        public void SetGameControllerReference(GameManager manager ) => m_gameManager = manager;

        public override void SetDelegate(IGridData gridDelegate)
        {
            GridDelegate = gridDelegate;
        }

        public void RegisterToGameManager() => GameManager.RegisterGridBase(this);       

        public void UnRegisterToGameManager() => GameManager.UnRegisterGridBase(this);
    }


}
