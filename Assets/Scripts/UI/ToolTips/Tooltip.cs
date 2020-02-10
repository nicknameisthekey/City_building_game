using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] int textPadding;
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform canvas;
    // [SerializeField] RectTransform border;
    private void Awake()
    {
        Tooltip_Manager.HideAllTooltips += HideTooltip;
        HideTooltip();
    }

    protected void ShowTooltip(string message)
    {
        canvas.gameObject.SetActive(true);
        text.text = message.Replace("++", "\n");
        var size = new Vector2(text.preferredWidth + textPadding * 2, text.preferredHeight + textPadding * 2);
        background.sizeDelta = size;
        // border.sizeDelta = size + new Vector2(3, 3);
        if (canvas.gameObject.activeSelf)
            Debug.Log(canvas.gameObject.name);
        StartCoroutine(moveTooltip());
    }
    protected void HideTooltip()
    {
        canvas.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    IEnumerator moveTooltip()
    {
        while (true)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out Vector2 localPoint);
            //if (Input.mousePosition.x / Screen.width >= 0.5)
            //    localPoint.x -= border.rect.width;
            //if (Input.mousePosition.y / Screen.height >= 0.5)
            //    localPoint.y -= border.rect.height;
            gameObject.transform.localPosition = localPoint;
            yield return new WaitForEndOfFrame();
        }
    }
}
