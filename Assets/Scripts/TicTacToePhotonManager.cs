using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace TicTacToe
{
    public class TicTacToePhotonManager : MonoBehaviourPunCallbacks
    {
        private TypedLobby tictactoeLobby;

        private string m_tictactoeLobbyName = "TicTacToeLobby";
        private string currentLobbyName;
        [SerializeField] private int m_maxPlayerCount = 2;
        private string currentRoomName;

        public static UnityAction OnOnConnectedToMaster;
        public static UnityAction OnOnDisconnected;
        public static UnityAction OnOnJoinedLobby;
        public static UnityAction OnOnRoomJoined;
        public static UnityAction OnOnRoomLeft;

        public static UnityAction<string> OnPhotonStatusUpdate;

        private string m_StatusMessage;

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        
        public void ConnectToPhoton()
        {
            if (CheckConnection())
            {
                PhotonNetwork.Disconnect();
            }
            else
            {
                PhotonNetwork.PhotonServerSettings.AppSettings.EnableLobbyStatistics = true;
                PhotonNetwork.ConnectUsingSettings();
                InitTicTacToeLobby();
            }
        }
        private void InitTicTacToeLobby()
        {
            tictactoeLobby = new TypedLobby(m_tictactoeLobbyName, LobbyType.Default);
        }

        private bool CheckConnection()
        {
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.InLobby && PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LeaveLobby();
                }
                else if (PhotonNetwork.InLobby && !PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveLobby();
                }
                else if (!PhotonNetwork.InLobby && PhotonNetwork.InRoom)
                {
                    PhotonNetwork.LeaveRoom();
                }
                else
                {
                    // Noting to Do
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void JointictactoeLobby()
        {
            currentLobbyName = tictactoeLobby.Name;
            PhotonNetwork.JoinLobby(tictactoeLobby);
        }

        private RoomOptions GetRoomOptions()
        {
            var roomOptions = new RoomOptions();

            List<string> roomPropertiesList = new List<string>();
            roomPropertiesList.Add("TicTacToe");
            roomOptions.MaxPlayers = m_maxPlayerCount;

            return roomOptions;
        }

        private void JoinRandomTicTacToeRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        #region Photon Pun Callbacks

        public override void OnConnectedToMaster()
        {
            m_StatusMessage = "On Connected to master";
            OnOnConnectedToMaster?.Invoke();
            OnPhotonStatusUpdate?.Invoke(m_StatusMessage);
            JointictactoeLobby();
        }

        private void SetPlayerProperties()
        {
            Hashtable playerData = new Hashtable();
            playerData.Add(GameConstants.ph_key_PlayerSide, "");
            PhotonNetwork.SetPlayerCustomProperties(playerData);
        }

        public RaiseEventOptions GetCurrentRaiseEventOptions(ReceiverGroup group)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
            raiseEventOptions.Receivers = group;
            return raiseEventOptions;
        }

        public override void OnJoinedLobby()
        {
            currentLobbyName = PhotonNetwork.CurrentLobby.Name;
            m_StatusMessage = "On Joined Lobby " + currentLobbyName;
            OnOnJoinedLobby?.Invoke();
            OnPhotonStatusUpdate?.Invoke(m_StatusMessage);

            if (PhotonNetwork.CurrentLobby.Name.Contains("TicTacToe"))
            {
                JoinRandomTicTacToeRandomRoom();
            }
        }


        public override void OnLeftLobby() { }

        public override void OnCreatedRoom()
        {
            currentRoomName = PhotonNetwork.CurrentRoom.Name;
            OnPhotonStatusUpdate?.Invoke("Created room " + currentRoomName);
        }

        public override void OnJoinedRoom()
        {
            m_StatusMessage = "On Joined Room " + PhotonNetwork.CurrentRoom.Name;

            if (!PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == m_maxPlayerCount)
            {
                PhotonNetwork.RaiseEvent(GameConstants.SendCurrentNameToOtherEventCode,
               PhotonNetwork.NickName, GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);
            }


            if (PhotonNetwork.CurrentRoom.PlayerCount == m_maxPlayerCount)
            {
                OnOnRoomJoined?.Invoke();
                OnPhotonStatusUpdate?.Invoke(m_StatusMessage);
                Invoke(nameof(InvokeMenuPanel), 2f);
            }
        }

        public void InvokeMenuPanel()
        {
            UIManager.s_Instance.ToggleMenuPanel(false);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount==m_maxPlayerCount)
            {
                PhotonNetwork.RaiseEvent(GameConstants.SendCurrentNameToOtherEventCode,
                PhotonNetwork.NickName, GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);
            }


            if (PhotonNetwork.CurrentRoom.PlayerCount == m_maxPlayerCount)
            {
                UIManager.s_Instance.ToggleMenuPanel(false);
            }
        }

        public override void OnLeftRoom()
        {

        }

        public override void OnCreateRoomFailed(short returnCode, string message) { }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (returnCode == 32760)     // this is the return code when room not found or room full
                PhotonNetwork.CreateRoom(null, GetRoomOptions());//5
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {

        }


        #endregion





    }
}