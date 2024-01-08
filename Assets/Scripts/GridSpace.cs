using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class GridSpace : MonoBehaviour
    {
        public Button m_Button;
        public Text m_ButtonText;
        public string playerSide;
        public string occupiedBy;

        private GameManager m_gameManager;

        public void SetSpace()
        {
            //if (gameController.playerMove)
            //{
            //    m_ButtonText.text = gameController.GetPlayerSide();
            //    m_Button.interactable = false;
            //    gameController.EndTurn();
            //}

        }


        public void SetGameControllerReference(GameManager manager )
        {
            m_gameManager = manager;
        }

    }
}
