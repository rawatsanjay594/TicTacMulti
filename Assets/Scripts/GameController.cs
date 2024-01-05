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

        private void Awake()
        {
            SetGameControllerReferenceOnButton();
            playerSide = "X";
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
        }

        private void GameOver()
        {
            for(int i = 0;i < buttonList.Count; i++)
            {
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }
        }

    }





}
