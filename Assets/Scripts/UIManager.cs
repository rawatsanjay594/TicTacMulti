using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace TicTacToe
{
    public class UIManager : MonoBehaviour
    {
        public PlayerColor activePlayerColor;
        public PlayerColor inactivePlayerColor;

        public Text m_StatusText;
        public Text m_gameOverText;
        public Text m_gameplayStatusText;

        public GameObject m_gameRestartButton;

        public static UIManager s_Instance;

        public GameObject m_MenuPanel;
        public GameObject m_GameOverPanel;
        public GameObject m_MistouchPrevention;

        public GameObject X_PanelObject;
        public GameObject Y_PanelObject;

        [HideInInspector] public string m_UserName;

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
            m_UserName = "player " + Random.Range(0, 100);

            GameConstants.currentPlayerName = m_UserName;
           // Debug.Log("<color=yellow>usrname set to </color>" + m_UserName);
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
            if (m_gameOverText != null && !string.IsNullOrEmpty(gameOverMsg))
                m_gameOverText.text = gameOverMsg;
        }

        public void UpdateGamePlayText(string gameplayMessage)
        {
            if (m_gameplayStatusText != null)
                m_gameplayStatusText.text = gameplayMessage;
        }

        public void ToggleMenuPanel(bool toggle)
        {
            m_MenuPanel?.SetActive(toggle);
        }

        public void ToggleMistouchPanel(bool toggle)
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

            panelImage.color = value ? activePlayerColor.panelcolor : inactivePlayerColor.panelcolor;
            panelText.color = value ? activePlayerColor.textColor : inactivePlayerColor.textColor;
        }

        public void ToggleYPanelObject(bool value)
        {
            Image panelImage = Y_PanelObject.GetComponent<Image>();
            Text panelText = Y_PanelObject.GetComponentInChildren<Text>();

            panelImage.color = value ? activePlayerColor.panelcolor : inactivePlayerColor.panelcolor;
            panelText.color = value ? activePlayerColor.textColor : inactivePlayerColor.textColor;
        }

        private void OnEvent(EventData customData)
        {
            if (customData.Code == GameConstants.GameOverEventCode)
            {
                string gameOver = (string)customData.CustomData;
                m_gameOverText.text = gameOver;
                ToggleGameOverPanel(true);
            }

        }

    }
}
