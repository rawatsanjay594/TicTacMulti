using System.Collections;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public abstract class GridBase : MonoBehaviour
    {
        public Button m_Button;
        public Text m_ButtonText;
        public string playerSide;
        public string m_gridId;
                
        protected IGridData GridDelegate;

        public abstract void SetDelegate(IGridData gridDelegate);
    }
}
