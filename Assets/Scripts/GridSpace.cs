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

        public void SetSpace()
        {
            m_ButtonText.text = playerSide;
            m_Button.interactable = false;
        }

    }
}
