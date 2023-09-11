using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;  // Add this at the top with other using statements

public class UIManager : MonoBehaviour
{
    private Canvas uiCanvas;
    private Image background;
    private Button hostButton;
    private Button clientButton;
    private Button serverButton;

    private void Start()
    {
        CreateUICanvas();
        CreateBackground();
        CreateHostButton();
        CreateClientButton();
        CreateServerButton();
    }

    private void CreateUICanvas()
    {
        GameObject canvasObj = new GameObject("UICanvas");
        uiCanvas = canvasObj.AddComponent<Canvas>();
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    private void CreateBackground()
    {
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(uiCanvas.transform, false);
        background = bgObj.AddComponent<Image>();
        background.color = new Color(0.1f, 0.1f, 0.1f, 1.0f); // Opaque dark background
        RectTransform rectTransform = background.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void CreateHostButton()
    {
        hostButton = CreateButton(new Vector2(-Screen.width / 2 + 100, Screen.height / 2 - 30), "Host");
        hostButton.onClick.AddListener(OnHostButtonClicked);
    }

    private void CreateClientButton()
    {
        clientButton = CreateButton(new Vector2(-Screen.width / 2 + 100, Screen.height / 2 - 55), "Client");
        clientButton.onClick.AddListener(OnClientButtonClicked);
    }

    private void CreateServerButton()
    {
        serverButton = CreateButton(new Vector2(-Screen.width / 2 + 100, Screen.height / 2 - 80), "Server");
        serverButton.onClick.AddListener(OnServerButtonClicked);
    }

    private Button CreateButton(Vector2 position, string buttonText)
    {
        GameObject buttonObj = new GameObject(buttonText);
        buttonObj.transform.SetParent(uiCanvas.transform, false);

        // Create button and text components
        Button button = buttonObj.AddComponent<Button>();
        Image image = buttonObj.AddComponent<Image>();
        
        // Create a material for the gradient and assign to the image
        Material gradientMaterial = new Material(Shader.Find("Custom/ButtonGradientShader"));
        gradientMaterial.SetColor("_TopColor", new Color(0.8f, 0.8f, 0.8f));
        gradientMaterial.SetColor("_BottomColor", new Color(0.05f, 0.05f, 0.05f));
        image.material = gradientMaterial;

        TextMeshProUGUI text = CreateTextForButton(buttonObj, buttonText);

        // Set button appearance
        button.targetGraphic = image;

        // Position the button
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(180, 18);
        text.fontSize = 15;  // Adjust this value as desired
        text.fontStyle = FontStyles.Bold;
        text.color = Color.black;

        return button;
    }

    private TextMeshProUGUI CreateTextForButton(GameObject buttonObj, string textContent)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);

        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.text = textContent;
        text.color = Color.black;
        text.font = TMP_Settings.defaultFontAsset;
        text.alignment = TextAlignmentOptions.Center;

        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        // Add gradient effect to TMP text
        text.enableVertexGradient = true;
        text.colorGradient = CreateTMPGradient();

        return text;
    }

    private VertexGradient CreateTMPGradient()
    {
        return new VertexGradient(Color.white, Color.white, Color.gray, Color.gray);
    }

    private InputField CreateInputField(Vector2 position, string placeholderText)
    {
        GameObject inputFieldObj = new GameObject("InputField");
        inputFieldObj.transform.SetParent(uiCanvas.transform, false);

        InputField inputField = inputFieldObj.AddComponent<InputField>();
        Image image = inputFieldObj.AddComponent<Image>();
        Text placeholder = CreateTextForInputField(inputFieldObj, placeholderText, Color.gray);
        Text text = CreateTextForInputField(inputFieldObj, "", Color.black);

        inputField.textComponent = text;
        inputField.placeholder = placeholder;

        RectTransform rectTransform = inputField.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(200, 30);

        return inputField;
    }

    private Text CreateTextForInputField(GameObject parentObj, string textContent, Color color)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(parentObj.transform, false);

        Text text = textObj.AddComponent<Text>();
        text.text = textContent;
        text.color = color;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleLeft;

        RectTransform rectTransform = text.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        return text;
    }

    public void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void OnClientButtonClicked()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void OnServerButtonClicked()
    {
        NetworkManager.Singleton.StartServer();
    }
}
