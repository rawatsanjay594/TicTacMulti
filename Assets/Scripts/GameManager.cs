using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour
    {
        public List<GridSpace> gridList = new List<GridSpace>();

        public GamePlayType gameType;

        public PlayerSideSelection playerSelectionForX;
        public PlayerSideSelection playerSelectionFor0;

        private string m_CurrentPlayerSide;
        private string m_OpponentPlayerSide;

        private void Start()
        {
            SetGameControllerReferenceOnButton();
        }

        public void SetGameControllerReferenceOnButton()
        {
            for (int i = 0; i < gridList.Count; i++)
            {
                gridList[i].GetComponent<GridSpace>().SetGameControllerReference(this);
            }
        }

        public void ChoosePlayerSide(string startingSide)
        {
            m_CurrentPlayerSide = startingSide;
            m_OpponentPlayerSide = (m_CurrentPlayerSide == "X") ? "0" : "X";

            if(m_CurrentPlayerSide=="X")
            {
                UIManager.s_Instance.UpdateXPanelObject(true);
            }
            else
            {
                UIManager.s_Instance.Update0PanelObject(true);
            }

        }

    }

    public enum GamePlayType
    {
        PassAndPlay,
        AI,
        Mutiplayer
    }
}
