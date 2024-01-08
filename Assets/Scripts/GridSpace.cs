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
            if (m_gameManager.playerMove)
            {
                m_ButtonText.text = m_gameManager.CurrentPlayerSide;
                m_Button.interactable = false;
                m_gameManager.EndTurn();
            }
        }

        public void SetGameControllerReference(GameManager manager ) => m_gameManager = manager;

    }
}
