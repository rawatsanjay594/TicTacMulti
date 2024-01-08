using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using DC.Tools;
using Photon.Realtime;

namespace TicTacToe
{
    public class TicTacToeScoreManager : MonoBehaviour
    {
        public Dictionary<string,string> PlayersDict = new Dictionary<string,string>();

        private string defaulSelectionValue = "--";

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
            UIManager.OnUserNameSet += AddPlayerToPlayersDict;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
            UIManager.OnUserNameSet -= AddPlayerToPlayersDict;
        }

        private void OnEvent(EventData customData)
        {
            if(customData.Code == GameConstants.SendCurrentNameToOtherEventCode)
            {
                string UserName = (string)customData.CustomData;
                AddPlayerToPlayersDict(UserName);
            }
        }

        public void AddPlayerToPlayersDict(string playerName)
        {
            if(!PlayersDict.ContainsKey(playerName)) 
            {
                PlayersDict.Add(playerName, defaulSelectionValue);
            }

            //     PhotonNetwork.RaiseEvent(GameConstants.SendCurrentNameToOtherEventCode,
           // playerName, GetCurrentRaiseEventOptions(ReceiverGroup.Others), SendOptions.SendReliable);

        }       


        public void RemovePlayerFromPlayersDict(string playerName)
        {
            if(PlayersDict.ContainsKey(playerName)) 
            {
               PlayersDict.Remove(playerName);            
            }
        }

        [Button("Display player dict")]
        public void DisplayPlayersDict()
        {
            foreach (KeyValuePair<string,string> item in PlayersDict)
            {
                Debug.Log($"Players Dict key is {item.Key} and the value is {item.Value}");
            }
        }

        public string GetPlayerSide(string playerName)
        {
            foreach (KeyValuePair<string,string> item in PlayersDict)
            {
                if (item.Key == playerName)
                {
                    return item.Value;
                }
            }

            return null;
        }



    }
}
