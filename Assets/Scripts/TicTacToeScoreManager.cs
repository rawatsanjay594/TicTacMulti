using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using DC.Tools;
using Photon.Realtime;
using System.Linq;

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
            //GameManager.OnPlayerSideSelected += UpdatePlayerSideOnPlayersDict;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
            UIManager.OnUserNameSet -= AddPlayerToPlayersDict;
            //GameManager.OnPlayerSideSelected -= UpdatePlayerSideOnPlayersDict;
        }

        private void OnEvent(EventData customData)
        {
            if(customData.Code == GameConstants.SendCurrentNameToOtherEventCode)
            {
                string UserName = (string)customData.CustomData;
                AddPlayerToPlayersDict(UserName);
            }

            if(customData.Code == GameConstants.SendCurrentSideToOtherEventCode)
            {
                object[] receivedData = (object[])customData.CustomData;
                string playerName = (string)receivedData[0];
                string playerSide = (string)receivedData[1];
                UpdatePlayerSideOnPlayersDict(playerName, playerSide);
            }
        }

        public void AddPlayerToPlayersDict(string playerName)
        {
            if(!PlayersDict.ContainsKey(playerName)) 
            {
                PlayersDict.Add(playerName, defaulSelectionValue);
            }

            if (playerName == PhotonNetwork.NickName)
                GameConstants.currentPlayerName = playerName;
            else
                GameConstants.opponentPlayerName = playerName;

        }       


        public void RemovePlayerFromPlayersDict(string playerName)
        {
            if(PlayersDict.ContainsKey(playerName)) 
            {
               PlayersDict.Remove(playerName);            
            }
        }

        public void UpdatePlayerSideOnPlayersDict(string playerName,string playerSide)
        {
            foreach (var key in PlayersDict.Keys.ToList())
            {
                if (key == playerName)
                {
                    PlayersDict[key] = playerSide;

                    Debug.Log($"key matches playername is {key} and side {playerSide}");
                }
                else
                {
                    if (Equals(playerSide, GameConstants.XPlayerIdentifier))
                    {
                        PlayersDict[key] = GameConstants.XPlayerIdentifier;
                    }
                    else
                        PlayersDict[key] = GameConstants.ZeroPlayerIdentifier;

                    Debug.Log($"key other player is {key} and side {playerSide}");

                }

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
