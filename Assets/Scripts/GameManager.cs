using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour
    {
        public List<GridSpace> gridList = new List<GridSpace>();

        public GamePlayType gameType;

        public PlayerSideSelection playerSelectionForX;
        public PlayerSideSelection playerSelectionFor0;

        private string m_CurrentPlayerSide;
        public string CurrentPlayerSide { get=> m_CurrentPlayerSide;}

        private string m_OpponentPlayerSide;
        public string OpponentPlayerSide { get => m_OpponentPlayerSide; }

        private int moveCount;
        private float delay;
        private int value;

        public bool playerMove;

        private void Start()
        {
            SetGameControllerReferenceOnButton();
            ResetGameBoard();
            ToggleGameBoardInteractable(true);
            playerMove = true;
        }

        private void Update()
        {
            if (!playerMove)
            {
                delay += delay * Time.deltaTime;

                // Instead of checking delay >= 100, use a threshold value for clarity
                float threshold = 10f;
                if (delay >= threshold)
                {
                    value = Random.Range(0, 8);

                    // Simplify by directly accessing the button and text components
                    Button selectedButton = gridList[value].GetComponent<Button>();
                    Text buttonText = gridList[value].GetComponentInChildren<Text>();

                    if (selectedButton.interactable)
                    {
                        buttonText.text = OpponentPlayerSide;
                        selectedButton.interactable = false;
                        EndTurn();
                    }
                }
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
            m_OpponentPlayerSide = (m_CurrentPlayerSide == "X") ? "O" : "X";

            bool isXPanelActive = (m_CurrentPlayerSide == "X");

            UIManager.s_Instance.ToggleXPanelObject(isXPanelActive);
            UIManager.s_Instance.ToggleYPanelObject(!isXPanelActive);
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

        private void ResetGameBoard()
        {
            foreach (var grid in gridList)
            {
                Text buttonText = grid.GetComponentInChildren<Text>();
                buttonText.text = string.Empty;
            }
        }


        #endregion

        public void EndTurn()
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
                GameOver("draw");
            }
            else
            {
                ChangeSides();
                delay = 1;
            }

        }



        private void ChangeSides()
        {
            playerMove = !playerMove;

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
            uiManager.m_gameOverText.text = (winningPlayer == "draw") ? "Draw" : winningPlayer + " Wins !!";
            uiManager.ToggleGameOverPanel(true);
            //restartButton.SetActive(true);
        }

    }

    public enum GamePlayType
    {
        PassAndPlay,
        AI,
        Mutiplayer
    }
}
