using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    [Serializable]
    public class PlayerSideSelection
    {
        public Image panel;
        public Text text;
        public Button button;
        public string playerSide;
    }

    [Serializable]
    public class PlayerColor
    {
        public Color panelcolor;
        public Color textColor;
    }
}
