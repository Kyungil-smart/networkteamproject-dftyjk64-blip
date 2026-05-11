using UnityEngine;
using UnityEngine.UI;

public class YutSelectionUI : MonoBehaviour
{
    private int selectionState = -1;

    [Header("윷 이미지 버튼 연결")]
    public Image frontImage;
    public Image backImage;

    [Header("확인 버튼")]
    public Button confirmButton;

    private void Start()
    {

        ResetSelection();
    }

    public void ResetSelection()
    {
        selectionState = -1;
        confirmButton.interactable = false;

        frontImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        backImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    public void SelectFront()
    {
        selectionState = 0;
        confirmButton.interactable = true;
        UpdateUI();
        Debug.Log("앞면 선택");
    }

    public void SelectBack()
    {
        selectionState = 1;
        confirmButton.interactable = true;
        UpdateUI();
        Debug.Log("뒷면 선택");
    }

    private void UpdateUI()
    {
        Color dimColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        Color activeColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        frontImage.color = (selectionState == 0) ? activeColor : dimColor;
        backImage.color = (selectionState == 1) ? activeColor : dimColor;

        frontImage.raycastTarget = true;
        backImage.raycastTarget = true;
    }

    public void OnConfirm()
    {
        if (selectionState == -1) return;

        bool isRoundSide = (selectionState == 0);
        Debug.Log($"최종 결정: {(isRoundSide ? "앞면" : "뒷면")}");

        if (YutManager.Instance != null)
        {
            YutManager.Instance.SubmitChoice(isRoundSide);
        }
        this.gameObject.SetActive(false);
    }
}