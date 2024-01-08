using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DC.Tools
{
    public class DCToast : MonoBehaviour
    {
        private const float DISPLAY_TIME = 2.5f;
        private Canvas m_ToastCanvas;
        private Canvas ToastCanvas
        {
            get
            {
                if (m_ToastCanvas == null)
                    BuildCanvas();

                return m_ToastCanvas;
            }
        }

        private Image m_ToastImage;
        private Image ToastImage
        {
            get
            {
                if (m_ToastImage == null)
                    BuildImage();

                return m_ToastImage;
            }
        }


        private Text m_ToastText;
        private Text ToastText
        {
            get
            {
                if (m_ToastText == null)
                    BuildText();

                return m_ToastText;
            }
        }

        private float elapsedTime;
        private bool m_isToasting;

        private void Update()
        {
            if (m_isToasting)
            {
                elapsedTime -= Time.deltaTime;
                if (elapsedTime <= 0)
                    HideToast();
            }
        }

        public void InitializeToast()
        {
            BuildCanvas();
            BuildImage();
            BuildText();
            HideToast();
        }

        private void BuildImage()
        {
            var image = gameObject.AddComponent<Image>();

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f * 255f));
            texture.Apply();
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 1);

            image.sprite = sprite;
            image.raycastTarget = false;

            m_ToastImage = image;
        }

        private void BuildText()
        {
            var go = new GameObject("ToastText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));

            var text = go.GetComponent<Text>();
            text.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
            text.fontSize = 24;
            text.lineSpacing = 1;
            text.supportRichText = true;
            text.alignment = TextAnchor.MiddleCenter;
            text.alignByGeometry = true;
            text.color = Color.white;
            text.raycastTarget = false;
            text.text = "";

            var textRT = text.GetComponent<RectTransform>();
            textRT.SetParent(GetComponent<RectTransform>(), false);
            textRT.anchorMin = new Vector2(0, 0);
            textRT.anchorMax = new Vector2(1, 1);

            m_ToastText = text;
        }

        private void BuildCanvas()
        {
            var go = new GameObject("ToastCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
            var canvas = go.GetComponent<Canvas>();
            var scaler = go.GetComponent<CanvasScaler>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = false;
            canvas.sortingOrder = 10001;

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            Debug.Log(Screen.orientation);
            scaler.referenceResolution = new Vector2(Screen.orientation == ScreenOrientation.LandscapeLeft ? 1920 : 1080,
                                                     Screen.orientation == ScreenOrientation.LandscapeLeft ? 1080 : 1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            scaler.referencePixelsPerUnit = 100;

            m_ToastCanvas = canvas;

            SetCanvasParent(canvas);
        }

        private void SetCanvasParent(Canvas canvas)
        {
            var canvasRT = canvas.GetComponent<RectTransform>();
            transform.localScale = Vector3.one;
            transform.SetParent(canvas.transform, false);

            RectTransform rectTransform;
            if ((rectTransform = GetComponent<RectTransform>()) == null)
                rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchorMin = new Vector2(0f, 0);
            rectTransform.anchorMax = new Vector2(1f, 0f);
            rectTransform.offsetMin = new Vector2(50f, 0f);
            rectTransform.offsetMax = new Vector2(-50f, 0f);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);
            rectTransform.anchoredPosition = new Vector2(0, 50);
        }

        public void Log(string message)
        {
            ToastText.text = message;
            ShowToast();
        }

        private void ShowToast()
        {
            ToastImage.enabled = true;
            ToastText.enabled = true;
            m_isToasting = true;
            elapsedTime = DISPLAY_TIME;
        }

        private void HideToast()
        {
            ToastImage.enabled = false;
            ToastText.enabled = false;
            m_isToasting = false;
            elapsedTime = 0;
        }
    }
}
