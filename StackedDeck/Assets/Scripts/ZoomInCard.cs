using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ZoomInCard : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] GameObject leftPanel;
    [SerializeField] GameObject rightPanel;
    [SerializeField] TextMeshProUGUI amount;
    [SerializeField] TMP_InputField input;
    [SerializeField] Button update;

    string code;

    void Awake()
    {
        update.onClick.AddListener(UpdateAmount);
    }

    public void SetImage(Sprite s, string cardCode)
    {
        image.sprite = s;
        amount.text = "You have " + CardManager.instance.GetNumCopiesOfCard(cardCode).ToString() + " copies.";
        code = cardCode;
        if(UserManager.instance.player != null)
        {
            rightPanel.SetActive(true);
        }
        else
        {
            rightPanel.SetActive(false);
        }
        leftPanel.SetActive(true);
    }

    void UpdateAmount()
    {
        int amount = int.Parse(input.text);
        if(0<= amount && amount <= 3)
            CardPanel.instance.UpdateAmountOfCard(int.Parse(input.text), code);
        input.text = "";
    }

}
