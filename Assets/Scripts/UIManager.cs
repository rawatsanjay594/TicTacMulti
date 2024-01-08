using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TicTacToe
{
    public class UIManager : MonoBehaviour
    {
        public PlayerColor activePlayerColor;
        public PlayerColor inactivePlayerColor;

        public Text m_StatusText;
        public Text m_gameOverText;

        public Button m_gameStartButton;

        public InputField m_UserNameInputField;


        public static UIManager s_Instance;

        public GameObject m_MenuPanel;
        public GameObject m_GameOverPanel;

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
           //TicTacToePhotonManager.OnPhotonStatusUpdate += DisplayGameStats;
        }

        private void OnDisable()
        {
            //TicTacToePhotonManager.OnPhotonStatusUpdate += DisplayGameStats;
        }

        private void Start()
        {
            Invoke(nameof(SetUserName), 5f);
        }

        public void SetUserName()
        {
            //m_UserName = m_UserNameInputField.text;

            m_UserName = "player " + Random.Range(0, 100);

            Debug.Log("<color=yellow>usrname set to </color>" + m_UserName);

            OnUserNameSet?.Invoke(m_UserName);

        }

        private void DisplayGameStats(string statusMessage)
        {
            if (m_StatusText != null && !string.IsNullOrEmpty(statusMessage))
                m_StatusText.text = statusMessage;
        }

        public void GameOverText(string gameOverMsg)
        {
            if (m_gameOverText != null && string.IsNullOrEmpty(gameOverMsg))
                m_gameOverText.text = gameOverMsg;
        }


        public void ToggleMenuPanel(bool toggle)
        {
            if (m_MenuPanel != null)
            {
                m_MenuPanel.SetActive(toggle);
            }
        }

        public void ToggleGameOverPanel(bool toggle)
        {
            if (m_GameOverPanel != null)
            {
                m_GameOverPanel.SetActive(toggle);
            }
        }

        public void ToggleXPanelObject(bool value)
        {
            if (value)
            {
                X_PanelObject.GetComponent<Image>().color = activePlayerColor.panelcolor;
                X_PanelObject.GetComponentInChildren<Text>().color = activePlayerColor.textColor;
            }
            else
            {
                X_PanelObject.GetComponent<Image>().color = inactivePlayerColor.panelcolor;
                X_PanelObject.GetComponentInChildren<Text>().color = inactivePlayerColor.textColor;
            }
        }

        public void ToggleYPanelObject(bool value)
        {
            if (value)
            {
                Y_PanelObject.GetComponent<Image>().color = activePlayerColor.panelcolor;
                Y_PanelObject.GetComponentInChildren<Text>().color = activePlayerColor.textColor;
            }
            else
            {
                Y_PanelObject.GetComponent<Image>().color = inactivePlayerColor.panelcolor;
                Y_PanelObject.GetComponentInChildren<Text>().color = inactivePlayerColor.textColor;
            }
        }


        //public void UpdateXPanelObject(bool value)
        //{
        //    if (X_PanelObject != null && value)
        //    {
        //        X_PanelObject.GetComponent<Image>().color = activePlayerColor.panelcolor;
        //        X_PanelObject.GetComponentInChildren<Text>().color = activePlayerColor.textColor;
        //    }
        //    else
        //    {
        //        X_PanelObject.GetComponent<Image>().color = inactivePlayerColor.panelcolor;
        //        X_PanelObject.GetComponentInChildren<Text>().color = inactivePlayerColor.textColor;
        //    }
        //}

        //public void Update0PanelObject(bool value)
        //{
        //    if (Y_PanelObject != null && value)
        //    {
        //        Y_PanelObject.GetComponent<Image>().color = activePlayerColor.panelcolor;
        //        Y_PanelObject.GetComponentInChildren<Text>().color = activePlayerColor.textColor;
        //    }
        //    else
        //    {
        //        Y_PanelObject.GetComponent<Image>().color = inactivePlayerColor.panelcolor;
        //        Y_PanelObject.GetComponentInChildren<Text>().color = inactivePlayerColor.textColor;
        //    }
        //}
    }
}
