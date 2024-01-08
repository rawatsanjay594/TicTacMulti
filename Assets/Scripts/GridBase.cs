using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public abstract class GridBase : MonoBehaviour
    {
        public Button m_Button;
        public Text m_ButtonText;
        public string playerSide;
    }
}
