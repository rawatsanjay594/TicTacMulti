using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.GamePlay
{
    public class GridSpace : MonoBehaviour
    {
        public Button m_Button;
        public Text m_ButtonText;
        public string playerSide;

        private GameController gameController;

        public void SetSpace()
        {
            if (gameController.playerMove)
            {
                m_ButtonText.text = gameController.GetPlayerSide();
                m_Button.interactable = false;
                gameController.EndTurn();
            }
        }


        public void SetGameControllerReference(GameController controller )
        {
            gameController = controller;
        }

    }
}
