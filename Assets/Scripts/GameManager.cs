using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour , IGridData
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
        public string CurrentPlayerSide { get=> m_CurrentPlayerSide;}

        private string m_OpponentPlayerSide;
        public string OpponentPlayerSide { get => m_OpponentPlayerSide; }

        public string OccupiedBy => string.Empty;
        public int GetGridId => 0;

        private int moveCount;

        public bool playerMove;

        public AIManager m_AIManager;

        public static UnityAction OnGameInitialized;
        public static UnityAction<string,string> OnPlayerSideSelected;

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
            SetGameControllerReferenceOnButton();
            ResetGameBoard();
            ToggleGameBoardInteractable(true);
            playerMove = true;
            m_AIManager.TogglePlayerMove(true);

            UIManager.s_Instance.ToggleGameStartButton(false);
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

            int value = m_AIManager.GetRandomGridValue(gridList.Count);
            // Simplify by directly accessing the button and text components
            Button selectedButton = gridList[value].GetComponent<Button>();
            Text buttonText = gridList[value].GetComponentInChildren<Text>();

            if (selectedButton.interactable)
            {
                buttonText.text = OpponentPlayerSide;
                selectedButton.interactable = false;
                EndTurn(gridList[value].m_gridIdInInt,OpponentPlayerSide); //NEED TO MNAGE THIS FOR AI
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

            OnPlayerSideSelected?.Invoke(GameConstants.currentPlayerName,m_CurrentPlayerSide);

            object[] customData = new object[]
             {
                GameConstants.currentPlayerName,
                m_CurrentPlayerSide
             };

            PhotonNetwork.RaiseEvent(GameConstants.SendCurrentSideToOtherEventCode,
                customData, GetCurrentRaiseEventOptions(ReceiverGroup.All), SendOptions.SendReliable);


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

        private void UpdateGridData(int gridId,string gridValue)
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
            playerMove = true;
            ResetGameBoard();
            ToggleGameBoardInteractable(true);
        }


        #endregion

        public void EndTurn(int gridId,string gridValue)
        {

            moveCount++;

            if (gridList[0].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[1].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[2].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);
            }
            else if (gridList[3].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[5].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }
            else if (gridList[6].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[7].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[8].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }
            else if (gridList[0].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[1].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[2].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }
            else if (gridList[0].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[3].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[6].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }
            else if (gridList[1].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[7].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }
            else if (gridList[2].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[5].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[8].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }
            else if (gridList[0].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[8].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }
            else if (gridList[2].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_CurrentPlayerSide && gridList[6].GetComponentInChildren<Text>().text == m_CurrentPlayerSide)
            {
                GameOver(m_CurrentPlayerSide);

            }

            ///


            else if (gridList[0].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[1].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[2].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);
            }
            else if (gridList[3].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[5].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }
            else if (gridList[6].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[7].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[8].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }
            else if (gridList[0].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[1].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[2].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }
            else if (gridList[0].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[3].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[6].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }
            else if (gridList[1].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[7].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }
            else if (gridList[2].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[5].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[8].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }
            else if (gridList[0].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[8].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }
            else if (gridList[2].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[4].GetComponentInChildren<Text>().text == m_OpponentPlayerSide && gridList[6].GetComponentInChildren<Text>().text == m_OpponentPlayerSide)
            {
                GameOver(m_OpponentPlayerSide);

            }


            else if (moveCount >= 9)
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

        private void ChangeSides()
        {
            playerMove = !playerMove;
            m_AIManager.TogglePlayerMove(playerMove);

            UIManager.s_Instance.ToggleXPanelObject(playerMove);
            UIManager.s_Instance.ToggleYPanelObject(!playerMove);
        }

        private void GameOver(string winningPlayer)
        {
            foreach (var grid in gridList)
            {
                grid.GetComponent<Button>().interactable = false;
            }

            UIManager uiManager = UIManager.s_Instance;
            string gameOver= (winningPlayer == GameConstants.gameDrawMessage) ? GameConstants.gameDrawMessage : winningPlayer + GameConstants.gameWinMessage;
            uiManager.m_gameOverText.text = gameOver;
            uiManager.ToggleGameOverPanel(true);

            PhotonNetwork.RaiseEvent(GameConstants.GameOverEventCode, gameOver,
                GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);


            UIManager.s_Instance.ToggleGameStartButton(true);
        }

    }

    public enum GamePlayType
    {
        AI =0,
        Mutiplayer = 1
    }
}
