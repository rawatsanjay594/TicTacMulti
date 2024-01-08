namespace TicTacToe
{
    public class GameConstants 
    {
        public const string XPlayerIdentifier = "X";
        public const string ZeroPlayerIdentifier = "0";


        public const string gameDrawMessage = "Its a Draw !!";
        public const string gameWinMessage = "Wins !!";


        public const string ph_key_PlayerSide = "Photon_Key_Player_Side";


        public static string currentPlayerName;
        public static string opponentPlayerName;



        /////Photon Event Codes

        public static byte SendCurrentNameToOtherEventCode = 1;
        public static byte MasterClientSendDataEventCode = 2;
        public static byte NonMasterClientSendDataEventCode = 3 ;
        public static byte SendCurrentSideToOtherEventCode = 4;
        public static byte UpdateGridEventCode = 5;
    }
}
