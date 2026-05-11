using UnityEngine;
using UnityEngine.UI;

public class YutResultUI : MonoBehaviour
{
    public Image[] yutImages;
    public Sprite frontSprite;
    public Sprite backSprite;

    public void ShowResult(bool[] results)
    {
        for (int i = 0; i < 4; i++)
        {
            yutImages[i].sprite = results[i] ? frontSprite : backSprite;
        }

        this.gameObject.SetActive(true);
    }
}
