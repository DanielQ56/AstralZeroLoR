using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class UserManager : MonoBehaviour
{
    private const string RegisterNewUser = "https://shielded-journey-60422.herokuapp.com/user_create";


    public static UserManager instance = null;

    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;

    private void Awake()
    {
        instance = this;
    }

    public void CreateNewUser()
    {
        Debug.Log("Sending new user");
        StartCoroutine(NewUser());
    }

    IEnumerator NewUser()
    {
        string[] allCards = CardManager.instance.GetAllCardsAsStrings();
        WWWForm form = new WWWForm();
        form.AddField("id", username.text);
        form.AddField("password", password.text);
        for(int i = 0; i < allCards.Length; ++i)
        {
            form.AddField("r" + (i + 1).ToString(), allCards[i]);
        }
        using (UnityWebRequest newUser = UnityWebRequest.Post(RegisterNewUser, form))
        {
            newUser.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = newUser.SendWebRequest();
            while(!request.isDone)
            {
                yield return null;
            }
            if(newUser.isNetworkError)
            {
                Debug.Log("error");
            }
            else
            {
                Debug.Log("heyo");
            }
        }
        Debug.Log("done");
    }
}
