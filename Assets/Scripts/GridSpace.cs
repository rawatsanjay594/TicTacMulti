using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class GridSpace : GridBase
    {
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
