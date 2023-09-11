using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    private Canvas uiCanvas;
    private Image background;
    private Button hostButton;
    private Button clientButton;
    private Button serverButton;
    private TextMeshProUGUI addressText;

    public NetworkManager networkManager; 

   private Vector2 originalClientButtonPosition;
    private Vector2 originalHostButtonPosition;

    private void Start()
    {
        CreateUICanvas();
        CreateBackground();
        CreateHostButton();
        CreateClientButton();
        CreateServerButton();

        originalClientButtonPosition = clientButton.GetComponent<RectTransform>().anchoredPosition;
        originalHostButtonPosition = hostButton.GetComponent<RectTransform>().anchoredPosition;
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
        background.color = new Color(0.1f, 0.1f, 0.1f, 1.0f);
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

        Button button = buttonObj.AddComponent<Button>();
        Image image = buttonObj.AddComponent<Image>();
        
        Material gradientMaterial = new Material(Shader.Find("Custom/ButtonGradientShader"));
        gradientMaterial.SetColor("_TopColor", new Color(0.8f, 0.8f, 0.8f));
        gradientMaterial.SetColor("_BottomColor", new Color(0.05f, 0.05f, 0.05f));
        image.material = gradientMaterial;

        TextMeshProUGUI text = CreateTextForButton(buttonObj, buttonText);
        button.targetGraphic = image;

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(180, 18);
        text.fontSize = 15;
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

        text.enableVertexGradient = true;
        text.colorGradient = new VertexGradient(Color.white, Color.white, Color.gray, Color.gray);

        return text;
    }

    public void OnHostButtonClicked()
    {
        Debug.Log("OnHostButtonClicked called");

        if (networkManager == null)
        {
            Debug.LogError("networkManager is null");
            return;
        }

        networkManager.StartHost();
        UpdateUIForHostingMode();
    }

    public void OnClientButtonClicked()
    {
        Debug.Log("OnClientButtonClicked called");

        if (networkManager == null)
        {
            Debug.LogError("networkManager is null");
            return;
        }

        if (!networkManager.IsClient) // If not already a client
        {
            networkManager.StartClient();

            // Check connection after a delay
            StartCoroutine(CheckClientConnection());
        }
        else
        {
            // Moved this logic to DisplayConnectionError()
            DisplayConnectionError();
        }
    }

    public void OnServerButtonClicked()
    {
        if (networkManager == null)
        {
            Debug.LogError("networkManager is null");
            return;
        }

        networkManager.StartServer();
    }

    private IEnumerator CheckClientConnection()
    {
        yield return new WaitForSeconds(2); // Wait for 2 seconds

        if (networkManager.IsConnectedClient) // Check if the client is connected
        {
            UpdateUIForClientMode();
        }
        else
        {
            DisplayConnectionError();  // Display the error if the client failed to connect
        }
    }

    private void DisplayConnectionError()
    {
        // Create and display the error text
        GameObject errorObj = new GameObject("ErrorText");
        errorObj.transform.SetParent(uiCanvas.transform, false);
        TextMeshProUGUI errorText = errorObj.AddComponent<TextMeshProUGUI>();
        errorText.text = "Failed to connect to the host!";
        errorText.color = Color.red;
        errorText.font = TMP_Settings.defaultFontAsset;
        errorText.alignment = TextAlignmentOptions.Center;

        RectTransform errorTransform = errorText.GetComponent<RectTransform>();
        errorTransform.anchoredPosition = Vector2.zero;
        errorTransform.sizeDelta = new Vector2(400, 30);

        // Make server and host buttons inactive
        serverButton.gameObject.SetActive(false);
        hostButton.gameObject.SetActive(false); 

        // Move the client button to the position of the host button and adjust its state
        clientButton.GetComponent<RectTransform>().anchoredPosition = hostButton.GetComponent<RectTransform>().anchoredPosition;
        clientButton.GetComponentInChildren<TextMeshProUGUI>().text = "Shutdown Client";
        clientButton.onClick.RemoveAllListeners();
        clientButton.onClick.AddListener(OnShutdownClientButtonClicked);
    }

    private void ResetUIAfterShutdown()
    {
        // Make host and server buttons active again
        hostButton.gameObject.SetActive(true);
        serverButton.gameObject.SetActive(true);

        // Reset client button to its initial state
        clientButton.GetComponentInChildren<TextMeshProUGUI>().text = "Client";
        clientButton.onClick.RemoveAllListeners();
        clientButton.onClick.AddListener(OnClientButtonClicked);

        // Remove error message if present
        GameObject errorObj = GameObject.Find("ErrorText");
        if (errorObj != null) Destroy(errorObj);
    }

    private void HandleFailedConnection()
    {
        // Hide the Server button
        serverButton.gameObject.SetActive(false);

        // Set Client button to its default state
        clientButton.GetComponentInChildren<TextMeshProUGUI>().text = "Client";
        clientButton.onClick.RemoveAllListeners();
        clientButton.onClick.AddListener(OnClientButtonClicked);
        clientButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Screen.width / 2 + 100, Screen.height / 2 - 55); // Reset the position

        // Destroy the error message
        GameObject errorObj = GameObject.Find("ErrorText");
        if (errorObj != null) Destroy(errorObj);
    }

    private void UpdateUIForHostingMode()
    {
        clientButton.gameObject.SetActive(false);
        serverButton.gameObject.SetActive(false);

        hostButton.GetComponentInChildren<TextMeshProUGUI>().text = "Shutdown Host";
        hostButton.onClick.RemoveAllListeners();
        hostButton.onClick.AddListener(OnShutdownHostButtonClicked);

        GameObject modeObj = new GameObject("ModeText");
        modeObj.transform.SetParent(uiCanvas.transform, false);
        TextMeshProUGUI modeText = modeObj.AddComponent<TextMeshProUGUI>();
        modeText.text = "Mode: Host";
        modeText.color = Color.white;
        modeText.font = TMP_Settings.defaultFontAsset;
        modeText.alignment = TextAlignmentOptions.Center;

        RectTransform modeTransform = modeText.GetComponent<RectTransform>();
        modeTransform.anchoredPosition = new Vector2(0, -Screen.height / 2 + 60);
        modeTransform.sizeDelta = new Vector2(200, 30);
    }

    private void UpdateUIForClientMode()
    {
        hostButton.gameObject.SetActive(false);
        serverButton.gameObject.SetActive(false);

        clientButton.GetComponentInChildren<TextMeshProUGUI>().text = "Shutdown Client";
        clientButton.onClick.RemoveAllListeners();
        clientButton.onClick.AddListener(OnShutdownClientButtonClicked);

        // Mode Text
        GameObject modeObj = new GameObject("ModeText");
        modeObj.transform.SetParent(uiCanvas.transform, false);
        TextMeshProUGUI modeText = modeObj.AddComponent<TextMeshProUGUI>();
        modeText.text = "Mode: Client";
        modeText.color = Color.white;
        modeText.font = TMP_Settings.defaultFontAsset;
        modeText.alignment = TextAlignmentOptions.Center;

        RectTransform modeTransform = modeText.GetComponent<RectTransform>();
        modeTransform.anchoredPosition = new Vector2(0, -Screen.height / 2 + 60);
        modeTransform.sizeDelta = new Vector2(200, 30); 

        // Address Text
        GameObject addressObj = new GameObject("AddressText");
        addressObj.transform.SetParent(uiCanvas.transform, false);
        TextMeshProUGUI addressText = addressObj.AddComponent<TextMeshProUGUI>();
        addressText.text = "Transport: Unity Transport";
        addressText.color = Color.white;
        addressText.font = TMP_Settings.defaultFontAsset;
        addressText.alignment = TextAlignmentOptions.Center;

        RectTransform addressTransform = addressText.GetComponent<RectTransform>();
        addressTransform.anchoredPosition = new Vector2(0, -Screen.height / 2 + 30); // Adjusted position to be below the mode text
        addressTransform.sizeDelta = new Vector2(200, 30);
    }


    public void OnShutdownHostButtonClicked()
    {
        if (networkManager == null)
        {
            Debug.LogError("networkManager is null");
            return;
        }

        networkManager.Shutdown();
        ResetUI();
    }

    public void OnShutdownClientButtonClicked()
    {
        if (networkManager == null)
        {
            Debug.LogError("networkManager is null");
            return;
        }

        networkManager.Shutdown();
        ResetUIForClient();
    }

    private void ResetUI()
    {
        clientButton.gameObject.SetActive(true);
        serverButton.gameObject.SetActive(true);
        hostButton.gameObject.SetActive(true);

        clientButton.GetComponent<RectTransform>().anchoredPosition = originalClientButtonPosition;
        hostButton.GetComponent<RectTransform>().anchoredPosition = originalHostButtonPosition;

        clientButton.GetComponentInChildren<TextMeshProUGUI>().text = "Client";
        clientButton.onClick.RemoveAllListeners();
        clientButton.onClick.AddListener(OnClientButtonClicked);

        hostButton.GetComponentInChildren<TextMeshProUGUI>().text = "Host";
        hostButton.onClick.RemoveAllListeners();
        hostButton.onClick.AddListener(OnHostButtonClicked);

        GameObject modeObj = GameObject.Find("ModeText");
        if (modeObj != null) Destroy(modeObj);

        GameObject addressObj = GameObject.Find("AddressText");
        if (addressObj != null) Destroy(addressObj);
    }

    private void ResetUIForClient()
    {
        clientButton.gameObject.SetActive(true);
        serverButton.gameObject.SetActive(true);
        hostButton.gameObject.SetActive(true);

        clientButton.GetComponent<RectTransform>().anchoredPosition = originalClientButtonPosition;
        hostButton.GetComponent<RectTransform>().anchoredPosition = originalHostButtonPosition;

        clientButton.GetComponentInChildren<TextMeshProUGUI>().text = "Client";
        clientButton.onClick.RemoveAllListeners();
        clientButton.onClick.AddListener(OnClientButtonClicked);

        hostButton.GetComponentInChildren<TextMeshProUGUI>().text = "Host";
        hostButton.onClick.RemoveAllListeners();
        hostButton.onClick.AddListener(OnHostButtonClicked);

        GameObject modeObj = GameObject.Find("ModeText");
        if (modeObj != null) Destroy(modeObj);

        GameObject addressObj = GameObject.Find("AddressText");
        if (addressObj != null) Destroy(addressObj);

        GameObject errorObj = GameObject.Find("ErrorText");
        if (errorObj != null) Destroy(errorObj);
    }
}
