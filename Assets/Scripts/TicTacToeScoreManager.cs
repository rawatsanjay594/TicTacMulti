using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using DC.Tools;

namespace TicTacToe
{
    public class TicTacToeScoreManager : MonoBehaviour
    {
        public Dictionary<string,string> PlayersDict = new Dictionary<string,string>();

        private string defaulSelectionValue = "--";

        private void OnEnable()
        {
            UIManager.OnUserNameSet += AddPlayerToPlayersDict;
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable()
        {
            UIManager.OnUserNameSet -= AddPlayerToPlayersDict;
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;

        }

        private void OnEvent(EventData customData)
        {
            if(customData.Code == GameConstants.SendCurrentNameToOtherEventCode)
            {

            }
        }

        public void UpdatePlayerDict()
        {

        }

        public void AddPlayerToPlayersDict(string playerName)
        {
            if(!PlayersDict.ContainsKey(playerName)) 
            {
                PlayersDict.Add(playerName, defaulSelectionValue);               
            }
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
