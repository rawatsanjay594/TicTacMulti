using UnityEngine;
using UnityEngine.UI;
using TicTacToe.DataClass;

namespace TicTacToe.Grid
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
                
        /// <summary>
        /// Reference for Grid so that each grid registers themselves to the manager so that they can inform if anything happens
        /// </summary>
        protected IGridData GridDelegate;

        public abstract void SetDelegate(IGridData gridDelegate);
    }
}
