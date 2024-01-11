using UnityEngine;
using UnityEngine.UI;
using TicTacToe.DataClass;

namespace TicTacToe
{
    /// <summary>
    /// Grid Base class is for each grid cell so that it is used as a component
    /// </summary>
    public abstract class GridCellBase : MonoBehaviour
    {
        public Button m_Button;
        public Text m_ButtonText;
        public string playerSide;
        public int m_gridIdInInt;
        public string m_gridIdInString;
                
        protected IGridData GridDelegate;

        public abstract void SetDelegate(IGridData gridDelegate);
    }
}
