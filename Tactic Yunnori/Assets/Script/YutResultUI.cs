using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class YutResultUI : NetworkBehaviour
{
    public Image[] yutImages;
    public Sprite frontSprite;
    public Sprite backSprite;

    public void ShowResult(bool[] results)
    {
        for (int i = 0; i < yutImages.Length; i++)
        {
            if (i < results.Length)
            {
                yutImages[i].sprite = results[i] ? frontSprite : backSprite;

                yutImages[i].color = Color.white;
            }
        }

        this.gameObject.SetActive(true);
    }

    public void CloseResult()
    {
        this.gameObject.SetActive(false);
    }
}