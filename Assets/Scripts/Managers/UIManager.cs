using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TicTacToe.DataClass;
using TicTacToe.Constants;
using TicTacToe.Multiplayer;

namespace TicTacToe
{
    /// <summary>
    /// UI manager as the name suggest it is responsible for handling all the game related UI toggling updating etc
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public PlayerColor activePlayerColor;
        public PlayerColor inactivePlayerColor;

        public Text m_StatusText;
        public Text m_GameOverText;
        public Text m_GameplayStatusText;

        public GameObject m_gameRestartButton;

        public GameObject m_MenuPanel;
        public GameObject m_GameOverPanel;
        public GameObject m_MistouchPrevention;

        public GameObject X_PanelObject;
        public GameObject Y_PanelObject;

        [HideInInspector] public string m_UserName;

        public static UIManager s_Instance;
        public static UnityAction<string> OnUserNameSet;

        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
           TicTacToePhotonManager.OnPhotonStatusUpdate += DisplayGameStats;
           GameManager.OnGameInitialized += SetUserName;
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable()
        {
            TicTacToePhotonManager.OnPhotonStatusUpdate += DisplayGameStats;
            GameManager.OnGameInitialized -= SetUserName;
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        public void SetUserName()
        {
            m_UserName = GameConstants.K_RandomPlayerPrefix + Random.Range(0, 100);
            GameConstants.K_CurrentPlayerName = m_UserName;
            Debug.Log("User name set to this" + m_UserName);
            PhotonNetwork.NickName = m_UserName;
            OnUserNameSet?.Invoke(m_UserName);
        }

        private void DisplayGameStats(string statusMessage)
        {
            if (m_StatusText != null && !string.IsNullOrEmpty(statusMessage))
                m_StatusText.text = statusMessage;
        }

        public void GameOverText(string gameOverMsg)
        {
            if (m_GameOverText != null && !string.IsNullOrEmpty(gameOverMsg))
                m_GameOverText.text = gameOverMsg;
        }

        public void UpdateGamePlayText(string gamePlayMessage)
        {
            if (m_GameplayStatusText != null)
                m_GameplayStatusText.text = gamePlayMessage;
        }

        public void ToggleMenuPanel(bool toggle)
        {
            m_MenuPanel?.SetActive(toggle);
        }

        public void ToggleMisTouchPanel(bool toggle)
        {
            m_MistouchPrevention?.SetActive(toggle);
        }

        public void ToggleGameOverPanel(bool toggle)
        {
            m_GameOverPanel?.SetActive(toggle);
        }

        public void ToggleRestartStartButton(bool value)
        {
            if (m_gameRestartButton != null)
            {
                m_gameRestartButton.SetActive(value);
                m_gameRestartButton.GetComponent<Button>().interactable = value;
            }
        }

        public void ToggleXPanelObject(bool value)
        {
            Image panelImage = X_PanelObject.GetComponent<Image>();
            Text panelText = X_PanelObject.GetComponentInChildren<Text>();

            panelImage.color = value ? activePlayerColor.m_Panelcolor : inactivePlayerColor.m_Panelcolor;
            panelText.color = value ? activePlayerColor.m_TextColor : inactivePlayerColor.m_TextColor;
        }

        public void ToggleYPanelObject(bool value)
        {
            Image panelImage = Y_PanelObject.GetComponent<Image>();
            Text panelText = Y_PanelObject.GetComponentInChildren<Text>();

            panelImage.color = value ? activePlayerColor.m_Panelcolor : inactivePlayerColor.m_Panelcolor;
            panelText.color = value ? activePlayerColor.m_TextColor : inactivePlayerColor.m_TextColor;
        }

        private void OnEvent(EventData customData)
        {
            if (customData.Code == GameConstants.EventCode_GameOver)
            {
                string gameOver = (string)customData.CustomData;
                m_GameOverText.text = gameOver;
                ToggleGameOverPanel(true);
            }

        }

    }
}
