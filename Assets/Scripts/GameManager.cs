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
                if (delay >= 100)
                {
                    value = UnityEngine.Random.Range(0, 8);
                    if (gridList[value].GetComponent<Button>().interactable == true)
                    {
                        gridList[value].GetComponentInChildren<Text>().text = OpponentPlayerSide;
                        gridList[value].GetComponent<Button>().interactable = false;
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
            m_OpponentPlayerSide = (m_CurrentPlayerSide == "X") ? "0" : "X";

            if(m_CurrentPlayerSide=="X")
            {
                UIManager.s_Instance.UpdateXPanelObject(true);
            }
            else
            {
                UIManager.s_Instance.Update0PanelObject(true);
            }
        }

        public void ToggleGameBoardInteractable(bool value)
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                gridList[i].GetComponent<Button>().interactable = value;
                gridList[i].m_ButtonText.text = string.Empty;
            }
        }

        private void ResetGameBoard()
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                gridList[i].GetComponentInChildren<Text>().text = string.Empty;
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
            //playerSide = (playerSide == "X") ? "0" : "X";
            playerMove = (playerMove == true) ? false : true;

            //if(playerSide == "X") 
            if (playerMove)
            {
                //SetPlayerColor(playerX, playerO);
            }
            else
            {
                //SetPlayerColor(playerO, playerX);
            }
        }

        private void GameOver(string winningPlayer)
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                gridList[i].GetComponent<Button>().interactable = false;
            }

            if (winningPlayer == "draw")
            {
                //gameOverText.text = "Draw";
                //SetPlayerColorInActive();
            }
            else
            {
                //gameOverText.text = winningPlayer + "Wins !!!";
            }

            //gameOverPanel.SetActive(true);
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
