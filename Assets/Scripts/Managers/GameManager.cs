using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TicTacToe.DataClass;
using TicTacToe.Constants;
using TicTacToe.Grid;

namespace TicTacToe
{
    /// <summary>
    /// This is the main core class which handles all the game core loop core functionality 
    /// Along with callbacks when certain main event is occured i.e gamestarted , game restarted and game over
    /// </summary>
    public class GameManager : MonoBehaviour, IGridData
    {
        private int moveCount;
        public bool playerMove;
        
        public List<GridCell> gridList = new List<GridCell>();

        public Dictionary<string, GridCellBase> gridBaseDict = new Dictionary<string, GridCellBase>();

        public PlayerSideSelection playerSelectionForX;
        public PlayerSideSelection playerSelectionFor0;

        private GamePlayType gameType;
        public GamePlayType GamePlayType => gameType;

        private string m_CurrentPlayerSide;
        public string CurrentPlayerSide { get => m_CurrentPlayerSide; }

        private string m_OpponentPlayerSide;
        public string OpponentPlayerSide { get => m_OpponentPlayerSide; }

        public string CurrentOccupant => string.Empty;
        public int GetGridCellId => 0;

        public AIManager m_AIManager;
        public TicTacToeGridManager m_GridManager;

        private static GameManager s_Instance;

        public static UnityAction OnGameInitialized;
        public static UnityAction<string, string> OnPlayerSideSelected;
        public static UnityAction<int> OnGridSelected;
        public static UnityAction OnGameRestarted;

        private void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = this;
            }
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            InitGameManager();
            ToggleGameBoardInteractable(true);
            m_AIManager.TogglePlayerMove(true);
            UIManager.s_Instance.ToggleRestartStartButton(false);
        }

        /// <summary>
        /// When th game needs to be started this start game is called which then takes the reference of buttons to set the game type
        /// </summary>
        /// <param name="value">Game type which is passed so that the game type is set here</param>
        public void StartGame(int value)
        {
            gameType = (GamePlayType)value;
            UIManager.s_Instance.ToggleMenuPanel(false);
        }

        /// <summary>
        /// Initialization game manager initializes all the game aspects and which are required to start the game
        /// </summary>
        private void InitGameManager()
        {
            OnGameInitialized?.Invoke();
            ResetGameBoard();
            playerMove = true;
        }

        private void OnEnable()
        {
            AIManager.OnRandomValueGenerated += UpdateAIValue;
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable()
        {
            AIManager.OnRandomValueGenerated -= UpdateAIValue;
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        /// <summary>
        /// Register the grid base to the manager so that game manager has reference for all the grid
        /// </summary>
        /// <param name="gridbase"></param>

        public static void RegisterGridBase(GridCellBase gridbase) => s_Instance.InternalRegisterGridBase(gridbase);

        public void InternalRegisterGridBase(GridCellBase gridbase)
        {
            if (!gridList.Contains(gridbase as GridCell))
            {
                gridList.Add(gridbase as GridCell);
            }

            if (!gridBaseDict.ContainsKey(gridbase.m_gridIdInString))
            {
                gridBaseDict.Add(gridbase.m_gridIdInString, gridbase);
                gridbase.SetDelegate(this);
            }
        }

        /// <summary>
        /// UnRegister the grid base to the manager so that game manager clears all the grid reference
        /// </summary>
        /// <param name="gridbase"></param>
        public static void UnRegisterGridBase(GridCellBase gridbase) => s_Instance.InternalUnregisterGridBase(gridbase);

        public void InternalUnregisterGridBase(GridCellBase gridbase)
        {
            if (gridBaseDict.ContainsKey(gridbase.m_gridIdInString))
            {
                gridBaseDict.Remove(gridbase.m_gridIdInString);
            }
        }

        /// <summary>
        /// When playing with AI it passes the value from AI manager and then it calls the Update AI value to the game
        /// </summary>
        private void UpdateAIValue()
        {
            if (gameType != GamePlayType.AI)
                return;

            int value = m_AIManager.GetRandomGridValue();
            // Simplify by directly accessing the button and text components
            Button selectedButton = gridList[value].GetComponent<Button>();
            Text buttonText = gridList[value].GetComponentInChildren<Text>();

            if (selectedButton.interactable)
            {
                buttonText.text = OpponentPlayerSide;
                selectedButton.GetComponent<GridCell>().m_GridAcquiredBy = GameConstants.K_AIName;
                selectedButton.interactable = false;
                EndTurn(gridList[value].m_gridIdInInt, OpponentPlayerSide,GameConstants.K_AIName);
            }

        }

        #region GameBoard

        public void SetGameControllerReferenceOnButton()
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                gridList[i].GetComponent<GridCell>().SetGameControllerReference(this);
            }
        }


        /// <summary>
        /// This is  first step where player clicks on the grid and assings which side it want to play
        /// </summary>
        /// <param name="startingSide"></param>
        public void ChoosePlayerSide(string startingSide)
        {
            m_CurrentPlayerSide = startingSide;

            m_OpponentPlayerSide = (m_CurrentPlayerSide == GameConstants.K_XPlayerIdentifier) ? GameConstants.K_ZeroPlayerIdentifier : GameConstants.K_XPlayerIdentifier;

            OnPlayerSideSelected?.Invoke(GameConstants.K_CurrentPlayerName, m_CurrentPlayerSide);

            if (gameType == GamePlayType.Mutiplayer)
            {
                object[] customData = new object[]
                 {
                GameConstants.K_CurrentPlayerName,
                m_CurrentPlayerSide
                 };

                PhotonNetwork.RaiseEvent(GameConstants.EventCode_SendCurrentSideToOther,
                    customData, GetCurrentRaiseEventOptions(ReceiverGroup.All), SendOptions.SendReliable);
            }

            bool isXPanelActive = (m_CurrentPlayerSide == GameConstants.K_XPlayerIdentifier);

            ToggleGameBoardInteractable(true);

            UIManager.s_Instance.ToggleXPanelObject(isXPanelActive);
            UIManager.s_Instance.ToggleYPanelObject(!isXPanelActive);

            UIManager.s_Instance.UpdateGamePlayText(string.Empty);
        }

        /// <summary>
        /// As raise event options are usually commmon and there is a small change of receiver group so parameter is created 
        /// </summary>
        /// <param name="group">The receiver group for which this raise event should be called</param>
        /// <returns></returns>
        public RaiseEventOptions GetCurrentRaiseEventOptions(ReceiverGroup group)
        {
            RaiseEventOptions raiseEventOptions = new();
            raiseEventOptions.Receivers = group;
            return raiseEventOptions;
        }

        /// <summary>
        /// Toggling of the board so that the buttons can be interact or not
        /// </summary>
        /// <param name="value">The value which is set so that grid </param>

        public void ToggleGameBoardInteractable(bool value)
        {
            foreach (var grid in gridList)
            {
                Button button = grid.GetComponent<Button>();
                button.interactable = value;
                grid.m_ButtonText.text = string.Empty;
            }
        }

        private void OnEvent(EventData customData)
        {
            if (customData.Code == GameConstants.EventCode_UpdateGrid)
            {
                object[] receivedData = (object[])customData.CustomData;
                int gridId = (int)receivedData[0];
                string gridValue = (string)receivedData[1];
                string occupantId = (string)receivedData[2];
                UpdateGridData(gridId, gridValue,occupantId);
            }

        }

        /// <summary>
        /// Update the grid data based on any one clicked the grid
        /// </summary>
        /// <param name="gridId"></param>
        /// <param name="gridValue"></param>
        private void UpdateGridData(int gridId, string gridValue,string occupantId)
        {
            if (gridId <= gridList.Count)
            {
                gridList[gridId].GetComponentInChildren<Text>().text = gridValue;
                gridList[gridId].m_GridAcquiredBy = occupantId;
            }
        }

        /// <summary>
        /// When the board wants to reset all the data in it
        /// </summary>
        private void ResetGameBoard()
        {
            foreach (var grid in gridList)
            {
                Text buttonText = grid.GetComponentInChildren<Text>();
                buttonText.text = string.Empty;
            }
        }

        /// <summary>
        /// WHen restart button is clicked then all the things are resetted
        /// </summary>
        public void RestartGame()
        {
            moveCount = 0;
            UIManager.s_Instance.ToggleGameOverPanel(false);
            UIManager.s_Instance.ToggleRestartStartButton(false);
            playerMove = true;
            ResetGameBoard();
            ToggleGameBoardInteractable(true);
            OnGameRestarted?.Invoke();
        }


        #endregion

        /// <summary>
        /// When any player completes the click this end turn is called which has grid Id specified with grid value
        /// </summary>
        /// <param name="gridId"></param>
        /// <param name="gridValue"></param>
        public void EndTurn(int gridId, string gridValue,string occupantId)
        {
            moveCount++;

            UIManager.s_Instance.ToggleMisTouchPanel(true);

            int rows = GameConstants.rowSize;
            int columns = GameConstants.columnSize;

            if (CheckForWin(m_CurrentPlayerSide, rows, columns))
            {
                GameOver(m_CurrentPlayerSide);
            }
            else if (CheckForWin(m_OpponentPlayerSide, rows, columns))
            {
                GameOver(m_OpponentPlayerSide);
            }
            else if (moveCount >= m_GridManager.totalItems)
            {
                GameOver(GameConstants.K_GameDrawMessage);
            }
            else
            {
                ChangeSides();
                m_AIManager.ResetAllValues();
            }

            object[] customData = new object[]
            {
        gridId,
        gridValue,
        occupantId
            };

            PhotonNetwork.RaiseEvent(GameConstants.EventCode_UpdateGrid,
                customData, GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);
        }

        /// <summary>
        /// Check for win if the all the grid has all the set equal horizontally,vertically as well as diagnol
        /// </summary>
        /// <param name="playerSide"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private bool CheckForWin(string playerSide, int rows, int columns)
        {
            // Check rows

            for (int i = 0; i < rows; i++)
            {
                if (AreCellsEqual(GetCellsInRow(i, columns), playerSide))
                {
                    return true;
                }
            }

            // Check columns
            for (int i = 0; i < columns; i++)
            {
                if (AreCellsEqual(GetCellsInColumn(i, rows), playerSide))
                {
                    return true;
                }
            }

            // Check diagonals
            if (AreCellsEqual(GetCellsInDiagonal(true, rows), playerSide) ||
                AreCellsEqual(GetCellsInDiagonal(false, rows), playerSide))
            {
                return true;
            }

            return false;
        }

        private GridCell[] GetCellsInRow(int rowIndex, int columns)
        {
            GridCell[] rowCells = new GridCell[columns];
            for (int i = 0; i < columns; i++)
            {
                rowCells[i] = gridList[rowIndex * columns + i];
            }
            return rowCells;
        }

        private GridCell[] GetCellsInColumn(int columnIndex, int rows)
        {
            GridCell[] columnCells = new GridCell[rows];
            for (int i = 0; i < rows; i++)
            {
                columnCells[i] = gridList[i * rows + columnIndex];
            }
            return columnCells;
        }

        private GridCell[] GetCellsInDiagonal(bool mainDiagonal, int size)
        {
            GridCell[] diagonalCells = new GridCell[size];
            int increment = mainDiagonal ? size + 1 : size - 1;

            for (int i = mainDiagonal ? 0 : size - 1, j = 0; j < size; i += increment, j++)
            {
                diagonalCells[j] = gridList[i];
            }

            return diagonalCells;
        }

        private bool AreCellsEqual(GridCell[] cells, string playerSide)
        {
            return cells.All(cell => cell.GetComponentInChildren<Text>().text == playerSide);
        }

        /// <summary>
        /// When each turn is called it checks for winning condition then if all is denied then change turn is called
        /// </summary>

        private void ChangeSides()
        {
            playerMove = !playerMove;
            m_AIManager.TogglePlayerMove(playerMove);

            if(gameType!=GamePlayType.Mutiplayer)
                UIManager.s_Instance.ToggleMisTouchPanel(!playerMove);

            UIManager.s_Instance.ToggleXPanelObject(playerMove);
            UIManager.s_Instance.ToggleYPanelObject(!playerMove);
        }

        /// <summary>
        /// When game over is called this function is called then a raise event is called which then the opponent also shows game over
        /// </summary>
        /// <param name="winningPlayer">The winning player passes its name to be displayed on game over screen</param>
        private void GameOver(string winningPlayer)
        {
            foreach (var grid in gridList)
            {
                grid.GetComponent<Button>().interactable = false;
            }

            string gameOver = (winningPlayer == GameConstants.K_GameDrawMessage) ? GameConstants.K_GameDrawMessage : winningPlayer + GameConstants.K_GameWinMessage;
            UIManager.s_Instance.m_GameOverText.text = gameOver;

            UIManager.s_Instance.ToggleRestartStartButton(true);
            Invoke(nameof(InvokeGameOverPanel), 1f);
                        
            PhotonNetwork.RaiseEvent(GameConstants.EventCode_GameOver, gameOver,
                GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);

        }

        
        private void InvokeGameOverPanel()
        {
            UIManager.s_Instance.ToggleGameOverPanel(true);
        }

    }

    /// <summary>
    /// Gameplay type determines of which type the game is 
    /// </summary>
    public enum GamePlayType
    {
        AI = 0,
        Mutiplayer = 1
    }
}
