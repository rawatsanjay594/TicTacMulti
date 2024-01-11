using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    [Serializable]
    public class PlayerSideSelection : IPlayerSide
    {
        public Image panel;
        public Text text;
        public Button button;
        public string playerSide;

        public string GetPlayerSide => playerSide;
    }

    public interface IPlayerSide
    {
       string GetPlayerSide { get; }
    }

    [Serializable]
    public class PlayerColor
    {
        public Color panelcolor;
        public Color textColor;
    }

    public interface IGridData
    {
        public string OccupiedBy { get; }

        public int GetGridId { get; }
    }


    [Serializable]
    public class GridSize
    {
        public Vector2 cellSize;
    }

    [Serializable]
    public class GridSpacing
    {
        public int XSpacing;
        public int YSpacing;
    }

    [Serializable]
    public class GridStartAxis
    {
        public GridLayoutGroup.Axis axis;
    }


}
