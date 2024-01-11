using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TicTacToe.Constants;

namespace TicTacToe.Multiplayer
{
    /// <summary>
    /// Photon manager is responsible for handling all multiplayer related functions connection, data transfer and everthing related to photon
    /// </summary>
    public class TicTacToePhotonManager : MonoBehaviourPunCallbacks
    {
        private TypedLobby m_TictactoeLobby;
        private string m_StatusMessage;

        private string m_TictactoeLobbyName = "TicTacToeLobby";
        private string currentLobbyName;
        [SerializeField] private int m_maxPlayerCount = 2;
        private string currentRoomName;

        public static UnityAction OnOnConnectedToMaster;
        public static UnityAction OnOnDisconnected;
        public static UnityAction OnOnJoinedLobby;
        public static UnityAction OnOnRoomJoined;
        public static UnityAction OnOnRoomLeft;

        public static UnityAction<string> OnPhotonStatusUpdate;

        public override void OnEnable() => base.OnEnable();

        public override void OnDisable() => base.OnDisable();

        /// <summary>
        /// Connection function which tries to connect to photon based on current scenario
        /// </summary>
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
            m_TictactoeLobby = new TypedLobby(m_TictactoeLobbyName, LobbyType.Default);
        }

        /// <summary>
        /// Check connection and then leaves so that everything gets resetted
        /// </summary>
        /// <returns></returns>
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
                    Debug.Log("Not in any state");
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void JoinTicTacToeLobby()
        {
            currentLobbyName = m_TictactoeLobby.Name;
            PhotonNetwork.JoinLobby(m_TictactoeLobby);
        }

        private RoomOptions GetRoomOptions()
        {
            var roomOptions = new RoomOptions();

            List<string> roomPropertiesList = new List<string>
            {
                "TicTacToe"
            };
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
            JoinTicTacToeLobby();
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
                PhotonNetwork.RaiseEvent(GameConstants.EventCode_SendCurrentNameToOther,
               PhotonNetwork.NickName, GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);
            }

            if (PhotonNetwork.CurrentRoom.PlayerCount == m_maxPlayerCount)
            {
                OnOnRoomJoined?.Invoke();
                OnPhotonStatusUpdate?.Invoke(m_StatusMessage);
            }
        }
              

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount==m_maxPlayerCount)
            {
                PhotonNetwork.RaiseEvent(GameConstants.EventCode_SendCurrentNameToOther,
                PhotonNetwork.NickName, GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);
            }

        }

        public override void OnCreateRoomFailed(short returnCode, string message) { }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            if (returnCode == 32760)     // this is the return code when room not found or room full
                PhotonNetwork.CreateRoom(null, GetRoomOptions());
        }


        #endregion


    }
}