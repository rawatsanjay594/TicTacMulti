using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay
{
    public class GameController : MonoBehaviour
    {
        public List<Text> buttonList = new List<Text>();

        private string playerSide;

        public GameObject gameOverPanel;

        public Text gameOverText;

        private int moveCount;

        public GameObject restartButton;

        private void Awake()
        {
            SetGameControllerReferenceOnButton();
            playerSide = "X";
            moveCount = 0;
            restartButton.SetActive(false);
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
                GameOver();
            }

            if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
            {
                GameOver();
            }

            if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver();
            }

            if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
            {
                GameOver();
            }

            if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
            {
                GameOver();
            }


            if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
            {
                GameOver();
            }


            if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver();
            }


            if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
            {
                GameOver();
            }


            if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
            {
                GameOver();
            }


            if (moveCount >= 9)
            {
                gameOverPanel.SetActive(true);
                gameOverText.text = "It's a Draw";
            }

            ChangeSides();
        }

        private void GameOver()
        {
            for(int i = 0;i < buttonList.Count; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }

            gameOverPanel.SetActive(true);
            gameOverText.text = playerSide + "Wins !!!";
            restartButton.SetActive(true);

        }

        private void ChangeSides()
        {
            playerSide = (playerSide == "X") ? "0" : "X";
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
        }

        private void SetBoardInteractable(bool toggle)
        {

        }
    }





}
