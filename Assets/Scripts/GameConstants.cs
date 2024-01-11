namespace TicTacToe.Constants
{
    /// <summary>
    /// All game related constants can be stored here which can be then easily manipulated and accessed without having references
    /// </summary>
    public class GameConstants 
    {
        //GamePlay
        public static int rowSize;
        public static int columnSize;

        //Identifiers for player side
        public const string K_XPlayerIdentifier = "X";
        public const string K_ZeroPlayerIdentifier = "0";

        //Game Message
        public const string K_GameDrawMessage = "Its a Draw !!";
        public const string K_GameWinMessage = "Wins !!";

        //Player Names for current player and opponent player
        public static string K_CurrentPlayerName;
        public static string K_OpponentPlayerName;
        public static string K_AIName = "AI";

        //Photon player Key for player side
        public const string K_PlayerSide = "Photon_Key_Player_Side";

        /////Photon Event Codes
        public static byte EventCode_SendCurrentNameToOther = 1;
        public static byte EventCode_MasterClientSendData = 2;
        public static byte EventCode_NonMasterClientSendData = 3 ;
        public static byte EventCode_SendCurrentSideToOther = 4;
        public static byte EventCode_UpdateGrid = 5;
        public static byte EventCode_GameOver = 6;
    }
}
