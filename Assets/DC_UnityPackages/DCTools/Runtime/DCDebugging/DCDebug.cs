using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC.Tools
{
    public enum DCLogLevel
    {
        Enabled,
        Disabled
    }

    public class DCDebug
    {
        private static DCToast m_Toast;
        private static DCToast Toast
        {
            get
            {
                if (m_Toast == null)
                {
                    m_Toast = BuildToast();
                }
                return m_Toast;
            }
        }

        public static void Log(object message)
        {
            Log(message, null);
        }

        public static void Log(object message, DCLogLevel logType)
        {
            Log(message, null, logType);
        }

        public static void Log(object message, Object context, DCLogLevel logType = DCLogLevel.Enabled, bool useToast = false)
        {
            if (logType == DCLogLevel.Disabled)
                return;

            Debug.Log(message, context);
#if !UNITY_EDITOR
            if (useToast)
                Toast.Log(message.ToString());
#endif
        }

        private static DCToast BuildToast()
        {
            var go = new GameObject("DCToast", typeof(DCToast));
            var toast = go.GetComponent<DCToast>();
            toast.InitializeToast();

            return toast;
        }

        public static void LogC(string color, string message, Object context, DCLogLevel logType = DCLogLevel.Enabled)
        {
            Log($"<color={color}>{message}</color>", context, logType: logType);
        }

        public static void LogC(string color, string message, DCLogLevel logType = DCLogLevel.Enabled)
        {
            LogC(color, message, null, logType);
        }

        public static void LogC(Color color, string message, Object context, DCLogLevel logType = DCLogLevel.Enabled)
        {
            LogC($"#{ColorUtility.ToHtmlStringRGB(color)}", message, context);
        }

        public static void LogC(Color color, string message)
        {
            LogC(color, message, null);
        }

        public static void LogC(Color color, string message, DCLogLevel logType)
        {
            LogC(color, message, null, logType);
        }
    }
}
