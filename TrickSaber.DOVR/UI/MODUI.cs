using System.Linq;
using TMPro;
using UnityEngine;

namespace TrickSaber.DOVR
{
    public class ModUI : MonoBehaviour
    {
        private Canvas _canvas;
        private TMP_Text _text;
        public static ModUI Instance;

        public static ModUI Create()
        {
            Instance = new GameObject("MODUI").AddComponent<ModUI>();
            return Instance;
        }

        private void Awake()
        {
            transform.position = new Vector3(-0.1f, 2f, 2.6f);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.enabled = false;
            var rectTransform = _canvas.transform as RectTransform;
            rectTransform.sizeDelta = new Vector2(100f, 50f);
            _text = UITools.CreateText(rectTransform, "", new Vector2(10f, 31f), new Vector2(0f, 0f));
            rectTransform = _text.transform as RectTransform;
            rectTransform.SetParent(_canvas.transform, false);
            rectTransform.anchoredPosition = new Vector2(10f, 31f);
            rectTransform.sizeDelta = new Vector2(100f, 20f);
            _text.text = "-";
            _text.fontSize = 14f;
            _canvas.enabled = true;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
    }

    public class UITools : MonoBehaviour
    {
        public static TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 anchoredPosition,
            Vector2 sizeDelta)
        {
            var go_customUiText = new GameObject("Text");
            go_customUiText.SetActive(false);
            var textMeshProUGUI = go_customUiText.AddComponent<TextMeshProUGUI>();
            textMeshProUGUI.font = Instantiate(Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
                .First(t => t.name == "Teko-Medium SDF No Glow"));
            textMeshProUGUI.rectTransform.SetParent(parent, false);
            textMeshProUGUI.text = text;
            textMeshProUGUI.fontSize = 4f;
            textMeshProUGUI.overrideColorTags = true;
            textMeshProUGUI.color = new Color(0.349f, 0.69f, 0.957f, 1f);
            textMeshProUGUI.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            textMeshProUGUI.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            textMeshProUGUI.rectTransform.sizeDelta = sizeDelta;
            textMeshProUGUI.rectTransform.anchoredPosition = anchoredPosition;
            go_customUiText.SetActive(true);
            return textMeshProUGUI;
        }
    }
}