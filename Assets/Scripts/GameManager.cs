using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour, IGridData
    {
        public List<GridSpace> gridList = new List<GridSpace>();

        public Dictionary<string, GridBase> gridBaseDict = new Dictionary<string, GridBase>();

        private static GameManager s_Instance;

        public GamePlayType gameType;

        public GamePlayType GamePlayType
        {
            get { return gameType; }
        }

        public PlayerSideSelection playerSelectionForX;
        public PlayerSideSelection playerSelectionFor0;

        private string m_CurrentPlayerSide;
        public string CurrentPlayerSide { get => m_CurrentPlayerSide; }

        private string m_OpponentPlayerSide;
        public string OpponentPlayerSide { get => m_OpponentPlayerSide; }

        public string OccupiedBy => string.Empty;
        public int GetGridId => 0;

        private int moveCount;

        public bool playerMove;

        public AIManager m_AIManager;
        public TicTacToeGridManager m_GridManager;

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
            ResetGameBoard();
            ToggleGameBoardInteractable(true);
            playerMove = true;
            m_AIManager.TogglePlayerMove(true);

            UIManager.s_Instance.ToggleRestartStartButton(false);
        }

        public void StartGame(int value)
        {
            gameType = (GamePlayType)value;
            UIManager.s_Instance.ToggleMenuPanel(false);
        }

        private void InitGameManager()
        {
            OnGameInitialized?.Invoke();
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

        public static void RegisterGridBase(GridBase gridbase) => s_Instance.InternalRegisterGridBase(gridbase);

        public void InternalRegisterGridBase(GridBase gridbase)
        {
            if (!gridList.Contains(gridbase as GridSpace))
            {
                gridList.Add(gridbase as GridSpace);
            }

            if (!gridBaseDict.ContainsKey(gridbase.m_gridIdInString))
            {
                gridBaseDict.Add(gridbase.m_gridIdInString, gridbase);
                gridbase.SetDelegate(this);
            }

        }

        public static void UnRegisterGridBase(GridBase gridbase) => s_Instance.InternalUnregisterGridBase(gridbase);

        public void InternalUnregisterGridBase(GridBase gridbase)
        {
            if (gridBaseDict.ContainsKey(gridbase.m_gridIdInString))
            {
                gridBaseDict.Remove(gridbase.m_gridIdInString);
            }
        }

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
                selectedButton.GetComponent<GridSpace>().m_GridAcquiredBy = GameConstants.aiName;
                selectedButton.interactable = false;
                EndTurn(gridList[value].m_gridIdInInt, OpponentPlayerSide); //NEED TO MNAGE THIS FOR AI
            }

        }

        #region GameBoard

        public void SetGameControllerReferenceOnButton()
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                gridList[i].GetComponent<GridSpace>().SetGameControllerReference(this);
            }
        }

        public void ChoosePlayerSide(string startingSide)
        {
            m_CurrentPlayerSide = startingSide;

            m_OpponentPlayerSide = (m_CurrentPlayerSide == GameConstants.XPlayerIdentifier) ? GameConstants.ZeroPlayerIdentifier : GameConstants.XPlayerIdentifier;

            OnPlayerSideSelected?.Invoke(GameConstants.currentPlayerName, m_CurrentPlayerSide);

            if (gameType == GamePlayType.Mutiplayer)
            {
                object[] customData = new object[]
                 {
                GameConstants.currentPlayerName,
                m_CurrentPlayerSide
                 };

                PhotonNetwork.RaiseEvent(GameConstants.SendCurrentSideToOtherEventCode,
                    customData, GetCurrentRaiseEventOptions(ReceiverGroup.All), SendOptions.SendReliable);
            }

            bool isXPanelActive = (m_CurrentPlayerSide == GameConstants.XPlayerIdentifier);

            ToggleGameBoardInteractable(true);

            UIManager.s_Instance.ToggleXPanelObject(isXPanelActive);
            UIManager.s_Instance.ToggleYPanelObject(!isXPanelActive);

            UIManager.s_Instance.UpdateGamePlayText(string.Empty);
        }

        public RaiseEventOptions GetCurrentRaiseEventOptions(ReceiverGroup group)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            raiseEventOptions.Receivers = group;
            return raiseEventOptions;
        }


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
            if (customData.Code == GameConstants.UpdateGridEventCode)
            {
                object[] receivedData = (object[])customData.CustomData;
                int gridId = (int)receivedData[0];
                string gridValue = (string)receivedData[1];
                UpdateGridData(gridId, gridValue);
            }

        }

        private void UpdateGridData(int gridId, string gridValue)
        {
            if (gridId <= gridList.Count)
            {
                gridList[gridId].GetComponentInChildren<Text>().text = gridValue;
            }
        }

        private void ResetGameBoard()
        {
            foreach (var grid in gridList)
            {
                Text buttonText = grid.GetComponentInChildren<Text>();
                buttonText.text = string.Empty;
            }
        }

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

        public void EndTurn(int gridId, string gridValue)
        {
            moveCount++;

            int rows = m_GridManager.gridRow;
            int columns = m_GridManager.gridColumn;

            if (CheckForWin(m_CurrentPlayerSide, rows, columns))  // Assuming a 3x3 grid
            {
                GameOver(m_CurrentPlayerSide);
            }
            else if (CheckForWin(m_OpponentPlayerSide, rows, columns))
            {
                GameOver(m_OpponentPlayerSide);
            }
            else if (moveCount >= m_GridManager.totalItems)
            {
                GameOver(GameConstants.gameDrawMessage);
            }
            else
            {
                ChangeSides();
                m_AIManager.ResetAllValues();
            }

            object[] customData = new object[]
            {
        gridId,
        gridValue
            };

            PhotonNetwork.RaiseEvent(GameConstants.UpdateGridEventCode,
                customData, GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);
        }

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

        private GridSpace[] GetCellsInRow(int rowIndex, int columns)
        {
            GridSpace[] rowCells = new GridSpace[columns];
            for (int i = 0; i < columns; i++)
            {
                rowCells[i] = gridList[rowIndex * columns + i];
            }
            return rowCells;
        }

        private GridSpace[] GetCellsInColumn(int columnIndex, int rows)
        {
            GridSpace[] columnCells = new GridSpace[rows];
            for (int i = 0; i < rows; i++)
            {
                columnCells[i] = gridList[i * rows + columnIndex];
            }
            return columnCells;
        }

        private GridSpace[] GetCellsInDiagonal(bool mainDiagonal, int size)
        {
            GridSpace[] diagonalCells = new GridSpace[size];
            int increment = mainDiagonal ? size + 1 : size - 1;

            for (int i = mainDiagonal ? 0 : size - 1, j = 0; j < size; i += increment, j++)
            {
                diagonalCells[j] = gridList[i];
            }

            return diagonalCells;
        }

        private bool AreCellsEqual(GridSpace[] cells, string playerSide)
        {
            return cells.All(cell => cell.GetComponentInChildren<Text>().text == playerSide);
        }



        private void ChangeSides()
        {
            playerMove = !playerMove;
            m_AIManager.TogglePlayerMove(playerMove);

            UIManager.s_Instance.ToggleMistouchPanel(!playerMove);

            UIManager.s_Instance.ToggleXPanelObject(playerMove);
            UIManager.s_Instance.ToggleYPanelObject(!playerMove);
        }

        private void GameOver(string winningPlayer)
        {
            foreach (var grid in gridList)
            {
                grid.GetComponent<Button>().interactable = false;
            }

            string gameOver = (winningPlayer == GameConstants.gameDrawMessage) ? GameConstants.gameDrawMessage : winningPlayer + GameConstants.gameWinMessage;
            UIManager.s_Instance.m_gameOverText.text = gameOver;

            UIManager.s_Instance.ToggleRestartStartButton(true);
            Invoke(nameof(InvokeGameOverPanel), 1f);
                        
            PhotonNetwork.RaiseEvent(GameConstants.GameOverEventCode, gameOver,
                GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);

        }

        private void InvokeGameOverPanel()
        {
            UIManager.s_Instance.ToggleGameOverPanel(true);
        }

    }

    public enum GamePlayType
    {
        AI = 0,
        Mutiplayer = 1
    }
}
