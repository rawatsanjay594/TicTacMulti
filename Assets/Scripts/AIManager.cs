using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class AIManager : MonoBehaviour
    {
        public AIType m_AIType;

    }

    public enum AIType
    {
        Easy,
        Medium,
        Hard
    }
}
