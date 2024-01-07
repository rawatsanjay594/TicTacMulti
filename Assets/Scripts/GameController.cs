using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TicTacToe
{
    public class GameController : MonoBehaviour
    {
        public List<Text> buttonList = new List<Text>();


        public GameObject gameOverPanel;

        public Text gameOverText;

        private int moveCount;

        public GameObject restartButton;


        public PlayerSideSelection playerX;
        public PlayerSideSelection playerO;

        public PlayerColor activePlayerColor;
        public PlayerColor inactivePlayerColor;

        private string playerSide;
        private string computerSide;
        public float delay;
        public bool playerMove;
        private int value;

        private void Awake()
        {
            //playerSide = "X";
            moveCount = 0;
            restartButton.SetActive(false);
            playerMove = true;
            //SetPlayerColor(playerX, playerO);
        }

        private void Update()
        {
            if (!playerMove)
            {
                delay += delay * Time.deltaTime;
                if (delay >= 100)
                {
                    value = UnityEngine.Random.Range(0, 8);
                    if (buttonList[value].GetComponentInParent<Button>().interactable == true)
                    {
                        buttonList[value].text = GetComputerSide();
                        buttonList[value].GetComponentInParent<Button>().interactable = false;
                        EndTurn();
                    }
                }
            }
        }


        public void SetStartingSide(string startingSide)
        {
            playerSide  = startingSide;
            if (playerSide == "X")
            {
                computerSide = "0";
                SetPlayerColor(playerX, playerO);
            }
            else
            {
                computerSide = "X";
                SetPlayerColor(playerO, playerX);
            }

            StartGame();
        }

        void StartGame()
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = true;
                buttonList[i].text = "";
            }

            SetPlayerButtons(false);
        }

        public string GetPlayerSide() { return playerSide; }
        public string GetComputerSide() { return computerSide; }

        public void EndTurn()
        {

            moveCount++;

            if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver(playerSide);
            }
            else if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
            {
                GameOver(playerSide);
            }

            ///


            else if (buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[3].text == computerSide && buttonList[4].text == computerSide && buttonList[5].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[6].text == computerSide && buttonList[7].text == computerSide && buttonList[8].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[0].text == computerSide && buttonList[1].text == computerSide && buttonList[2].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[0].text == computerSide && buttonList[3].text == computerSide && buttonList[6].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[1].text == computerSide && buttonList[4].text == computerSide && buttonList[7].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[2].text == computerSide && buttonList[5].text == computerSide && buttonList[8].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[0].text == computerSide && buttonList[4].text == computerSide && buttonList[8].text == computerSide)
            {
                GameOver(computerSide);
            }
            else if (buttonList[2].text == computerSide && buttonList[4].text == computerSide && buttonList[6].text == computerSide)
            {
                GameOver(computerSide);
            }


            else if (moveCount >= 9)
            {
                GameOver("draw");
            }
            else
            {
                ChangeSides();
                delay = 2;
            }

        }

        private void SetPlayerColor(PlayerSideSelection newPlayer, PlayerSideSelection oldPlayer)
        {
            newPlayer.panel.color = activePlayerColor.panelcolor;
            newPlayer.text.color = activePlayerColor.textColor;

            oldPlayer.panel.color = inactivePlayerColor.panelcolor;
            oldPlayer.text.color = inactivePlayerColor.textColor;
        }

        private void GameOver(string winningPlayer)
        {
            for(int i = 0;i < buttonList.Count; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }

            if (winningPlayer == "draw")
            {
                gameOverText.text = "Draw";
                SetPlayerColorInActive();
            }
            else
            {
                gameOverText.text = winningPlayer + "Wins !!!";
            }

            gameOverPanel.SetActive(true);
            restartButton.SetActive(true);

        }

        private void ChangeSides()
        {
            //playerSide = (playerSide == "X") ? "0" : "X";
            playerMove = (playerMove == true) ? false: true;

            //if(playerSide == "X") 
            if(playerMove) 
            {
                SetPlayerColor(playerX, playerO);
            }
            else
            {
                SetPlayerColor(playerO, playerX);
            }
        }

        public void RestartGame()
        {
            //playerSide = "X";
            moveCount = 0;
            gameOverPanel.SetActive(false);
            restartButton.SetActive(false);
            SetPlayerButtons(true);
            SetPlayerColorInActive();
            playerMove = true;
            delay = 10;

            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].text = "";
            }

            SetPlayerButtons(true);
            //SetPlayerColor(playerX,playerO);
        }

        private void SetBoardInteractable(bool toggle)
        {

        }

        private void SetPlayerButtons(bool toggle)
        {
            playerX.button.interactable = toggle;
            playerO.button.interactable = toggle;
        }

        private void SetPlayerColorInActive()
        {
            playerX.panel.color = inactivePlayerColor.panelcolor;
            playerX.text.color = inactivePlayerColor.textColor;

            playerO.panel.color = inactivePlayerColor.panelcolor;
            playerO.text.color = inactivePlayerColor.textColor;
        }
    }





}
