using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using DC.Tools;
using System.Linq;
using TicTacToe.Constants;

namespace TicTacToe
{
    /// <summary>
    /// Score manager handles all score related mechanism inside the game like maintaining the dictionary and updating the dictionary
    /// </summary>
    public class TicTacToeScoreManager : MonoBehaviour
    {
        /// <summary>
        /// Players Dict maintains list of user name and the in values it assings which side it has been aside to
        /// </summary>
        public Dictionary<string,string> PlayersDict = new Dictionary<string,string>();

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
            if(customData.Code == GameConstants.EventCode_SendCurrentNameToOther)
            {
                string UserName = (string)customData.CustomData;
                AddPlayerToPlayersDict(UserName);
            }

            if(customData.Code == GameConstants.EventCode_SendCurrentSideToOther)
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
                PlayersDict.Add(playerName, GameConstants.ScoreDefaultSelection);
            }

            if (playerName == PhotonNetwork.NickName)
                GameConstants.K_CurrentPlayerName = playerName;
            else
                GameConstants.K_OpponentPlayerName = playerName;
        }       

        public void RemovePlayerFromPlayersDict(string playerName)
        {
            if(PlayersDict.ContainsKey(playerName))             
               PlayersDict.Remove(playerName);                 
        }

        /// <summary>
        /// Update players name based on dict so that it can assing name reference to player side
        /// </summary>
        /// <param name="playerName"> your name getting the gameconstants current name</param>
        /// <param name="playerSide">your player side by getting the clicking on the grid button</param>
        public void UpdatePlayerSideOnPlayersDict(string playerName,string playerSide)
        {
            foreach (var key in PlayersDict.Keys.ToList())
            {
                if (key == playerName)
                {
                    PlayersDict[key] = playerSide;
                }
                else
                {
                    PlayersDict[key] = "0";
                }
            }
        }
        
        //Need to test then update
        //public void UpdatePlayerSideOnPlayersDict(string playerName, string playerSide)
        //{
        //    foreach (var key in PlayersDict.Keys.ToList())
        //    {
        //        PlayersDict[key] = (key == playerName) ? playerSide : "0";
        //    }
        //}

        [Button("Display player dict")]
        public void DisplayPlayersDict()
        {
            foreach (KeyValuePair<string,string> item in PlayersDict)
            {
                Debug.Log($"Players Dict key is {item.Key} and the value is {item.Value}");
            }
        }

        /// <summary>
        /// If you want to get your player side 
        /// </summary>
        /// <param name="playerName">If any player wants to get the side which has been assigned to it it can pass its name and get the side</param>
        /// <returns></returns>
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