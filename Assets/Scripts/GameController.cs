using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace TicTacToe.GamePlay
{
    [Serializable]
    public class Player
    {
        public Image panel;
        public Text text;
    }

    [Serializable]
    public class PlayerColor
    {
        public Color panelcolor;
        public Color textColor;
    }

    public class GameController : MonoBehaviour
    {
        public List<Text> buttonList = new List<Text>();

        private string playerSide;

        public GameObject gameOverPanel;

        public Text gameOverText;

        private int moveCount;

        public GameObject restartButton;


        public Player playerX;
        public Player playerO;

        public PlayerColor activePlayerColor;
        public PlayerColor inactivePlayerColor;

        private void Awake()
        {
            SetGameControllerReferenceOnButton();
            playerSide = "X";
            moveCount = 0;
            restartButton.SetActive(false);

            SetPlayerColor(playerX, playerO);
        }

        public void SetGameControllerReferenceOnButton()
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
            
            }
        }

        public string GetPlayerSide() { return playerSide; }

        public void EndTurn()
        {

            moveCount++;

            if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            {
                GameOver(playerSide);
            }

            if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
            {
                GameOver(playerSide);
            }

            if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver(playerSide);
            }

            if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            {
                GameOver(playerSide);
            }

            if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
            {
                GameOver(playerSide);
            }


            if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
            {
                GameOver(playerSide);
            }


            if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver(playerSide);
            }


            if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver(playerSide);
            }


            if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
            {
                GameOver(playerSide);
            }


            if (moveCount >= 9)
            {
                GameOver("draw");
            }

            ChangeSides();
        }

        private void SetPlayerColor(Player newPlayer, Player oldPlayer)
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
            playerSide = (playerSide == "X") ? "0" : "X";

            if(playerSide == "X") 
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
            playerSide = "X";
            moveCount = 0;
            gameOverPanel.SetActive(false);

            for(int i = 0; i < buttonList.Count; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = true;
                buttonList[i].text = "";
            }

            SetPlayerColor(playerX,playerO);
            restartButton.SetActive(false);
        }

        private void SetBoardInteractable(bool toggle)
        {

        }
    }





}
