using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    // Declare the material field here
    private Material material;

    private Material Material
    {
        get => material;
        set
        {
            material = value;
            originalTopColor = material.GetColor("_TopColor");
            originalBottomColor = material.GetColor("_BottomColor");
        }
    }

    private Color originalTopColor;
    private Color originalBottomColor;

    private void Start()
    {
        material = GetComponent<Image>().material;
        originalTopColor = material.GetColor("_TopColor");
        originalBottomColor = material.GetColor("_BottomColor");

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        StartCoroutine(ClickEffect());
    }

    private IEnumerator ClickEffect()
    {
        material.SetColor("_TopColor", originalTopColor * 0.7f);
        material.SetColor("_BottomColor", originalBottomColor * 0.7f);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_TopColor", originalTopColor);
        material.SetColor("_BottomColor", originalBottomColor);
    }
}
