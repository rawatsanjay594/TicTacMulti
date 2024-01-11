using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{

    /// <summary>
    /// Data class to store player side selection Data it is like a data holder class
    /// </summary>
    [Serializable]
    public class PlayerSideSelection : IPlayerSide
    {
        public Image panel;
        public Text text;
        public Button button;
        public string playerSide;

        public string GetPlayerSide => playerSide;
    }

    /// <summary>
    /// IPlayerSide provides any data which needs to be accessed by other classes other than which has all references
    /// </summary>
    public interface IPlayerSide
    {
       string GetPlayerSide { get; }
    }

    /// <summary>
    /// Data Holder class to store common Panel color and text color when whose side is selected at Top
    /// </summary>
    [Serializable]
    public class PlayerColor
    {
        public Color m_Panelcolor;
        public Color m_TextColor;
    }

    /// <summary>
    /// If Other class wants to get who acquired that grid along with what was the grid ID
    /// </summary>
    public interface IGridData
    {
        public string CurrentOccupant { get; }

        public int GetGridCellId { get; }
    }
}
