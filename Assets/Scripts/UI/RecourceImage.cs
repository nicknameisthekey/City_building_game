using UnityEngine;
using UnityEngine.UI;

public class RecourceImage : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text text;
   public void ChangeRecourceText(string newText)
    {
        text.text = newText;
    }
    public void ChangeRecorceImage(Sprite newImage)
    {
        image.sprite = newImage;
    }
}
