using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] Image image;

    string code;

    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(ZoomInOnCard);
    }

    public void Setup(string cardCode, int num)
    {
        code = cardCode;
        amount.text = num.ToString();
        string path = "cards/" + cardCode;
        image.sprite = Resources.Load<Sprite>(path);

    }

    void ZoomInOnCard()
    {
        CardPanel.instance.ZoomIn(image.sprite, code);
    }
}
