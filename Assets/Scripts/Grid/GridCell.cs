using TicTacToe.DataClass;
using TicTacToe.Constants;
using Photon.Pun;
using UnityEngine;

namespace TicTacToe.Grid
{
    /// <summary>
    /// This script is attached to each grid cell this holds data for this grid cell
    /// </summary>
    public class GridCell : GridCellBase, IGridData
    {
        public string m_GridAcquiredBy;    
        public string CurrentOccupant => m_GridAcquiredBy;
        public int GetGridCellId => m_gridIdInInt;

        private GameManager m_gameManager;
        private TicTacToeScoreManager m_ScoreManager;

        private void OnEnable() { }

        private void OnDisable() => UnRegisterToGameManager();
        
        private void Start() => m_ScoreManager = FindObjectOfType<TicTacToeScoreManager>();

        /// <summary>
        /// Grid Cell registers themselves to the game manager so that game manager knows which all grid are currently in the board
        /// So If we want to take any action we have reference for each
        /// </summary>

        public void RegisterToGameManager() => GameManager.RegisterGridBase(this);

        public void UnRegisterToGameManager() => GameManager.UnRegisterGridBase(this);

        /// <summary>
        /// When someone clicks on this grid this data is populated based on the game type then further action is taken
        /// </summary>
        public void PopulateData()
        {
            if (m_gameManager.GamePlayType == GamePlayType.AI)            
                PlayerMove();            
            else if (m_gameManager.GamePlayType == GamePlayType.Mutiplayer)
                 MultiplayerMove();            
        }

        /// <summary>
        /// If this is of type against AI then the player move grabs data from manager to full
        /// </summary>
        private void PlayerMove()
        {
            if (m_gameManager.playerMove)
            {
                playerSide = m_gameManager.CurrentPlayerSide;
                m_GridAcquiredBy = PhotonNetwork.NickName;
                m_ButtonText.text = playerSide;
                m_Button.interactable = false;
               m_gameManager.EndTurn(m_gridIdInInt, playerSide,m_GridAcquiredBy);
            }
        }

        /// <summary>
        /// If this is of type against Multiplayer then the player move grabs data from manager to full
        /// </summary>
        private void MultiplayerMove()
        {
            playerSide = m_ScoreManager.GetPlayerSide(GameConstants.K_CurrentPlayerName);
            m_ButtonText.text = playerSide;
            m_Button.interactable = false;
            Debug.Log("User name set to this" + PhotonNetwork.NickName);

            m_gameManager.EndTurn(m_gridIdInInt, playerSide,PhotonNetwork.NickName);
        }

        /// <summary>
        /// The Game manager registers himself to particular grid so that he can have reference for that
        /// </summary>
        /// <param name="manager"></param>
        public void SetGameControllerReference(GameManager manager ) => m_gameManager = manager;

        /// <summary>
        /// The Delegate is passed here and it sets the game manager delegate So If anything happens
        /// on the grid it can inform the manager about it
        /// </summary>
        /// <param name="gridDelegate">This is where the delegate reference is passed to be assigned</param>

        public override void SetDelegate(IGridData gridDelegate)
        {
            GridDelegate = gridDelegate;
        }

    }


}
