using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class UserManager : MonoBehaviour
{
    #region URL's for Web Requests
    private const string RegisterNewUser = "https://shielded-journey-60422.herokuapp.com/user_create";
    private const string GetUser = "https://shielded-journey-60422.herokuapp.com/users/";
    #endregion


    public static UserManager instance = null;

    [SerializeField] GameObject inputPanel;
    [SerializeField] ErrorPanel error;

    #region Input Fields for both Logging In and Signing Up
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_InputField newUsername;
    [SerializeField] TMP_InputField newPassword;
    #endregion

    PlayerData player;

    private void Awake()
    {
        instance = this;
    }

    public void Guest()
    {
        ResetInput();
        inputPanel.SetActive(false);
    }
    #region New User
    public void CreateNewUser()
    {
        if(!CheckForEmptyInput(newUsername, newPassword))
           StartCoroutine(NewUser());
    }

    IEnumerator NewUser()
    {
        char[] charsToTrim = new char[] { '[', ']' };
        string getUserURL = GetUser + string.Format("{0}/{1}", username.text, password.text);
        using (UnityWebRequest newUser = UnityWebRequest.Get(getUserURL))
        {
            newUser.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = newUser.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            if (newUser.responseCode == 500)
            {
                ErrorOccurred("Unexpected Error Occurred. Please Try Again.");
            }
            else if (newUser.downloadHandler.text != "[]")
            {
                ErrorOccurred("You Already Have an Existing Account!");
                yield break;
            }
        }
        CardManager.instance.LoadAllCards();
        string[] allCards = CardManager.instance.GetAllCardsAsStrings();
        WWWForm form = new WWWForm();
        form.AddField("id", username.text);
        form.AddField("password", password.text);
        for (int i = 0; i < allCards.Length; ++i)
        {
            form.AddField("r" + (i + 1).ToString(), allCards[i]);
        }
        using (UnityWebRequest newUser = UnityWebRequest.Post(RegisterNewUser, form))
        {
            newUser.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = newUser.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            if (newUser.responseCode == 500)
            {
                ErrorOccurred("Unexpected Error Occurred");
            }
            else
            {
                Debug.Log("User Created Succesfully!");
                CreateNewPlayer(allCards);
                ResetInput();
                inputPanel.SetActive(false);
            }
        }
    }

    void CreateNewPlayer(string[] allCards)
    {
        player = new PlayerData(username.text, password.text, allCards[0], allCards[1], allCards[2], allCards[3], allCards[4], allCards[5]);
    }
    #endregion

    #region Check For Existing User
    public void CheckForUser()
    {
        if(!CheckForEmptyInput(username, password))
            StartCoroutine(CheckUser());

    }

    IEnumerator CheckUser()
    {
        char[] charsToTrim = new char[] { '[', ']' };
        string getUserURL = GetUser + string.Format("{0}/{1}", username.text, password.text);
        using (UnityWebRequest newUser = UnityWebRequest.Get(getUserURL))
        {
            newUser.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = newUser.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }
            if (newUser.responseCode == 500)
            {
                ErrorOccurred("Unexpected Error Occurred. Please Try Again.");
            }
            else if (newUser.downloadHandler.text == "[]")
            {
                ErrorOccurred("Invalid Username of Password");
            }
            else
            {
                player = JsonUtility.FromJson<PlayerData>(newUser.downloadHandler.text.Trim().Trim(charsToTrim));
                CardManager.instance.LoadAllCards(player);
                ResetInput();
                inputPanel.SetActive(false);
            }
        }
    }
    #endregion

    #region Helper Functions
    bool CheckForEmptyInput(TMP_InputField user, TMP_InputField pass)
    {
        if (user.text == "" || pass.text == "")
        {
            ErrorOccurred("Username and Password cannot be empty");
        }
        return false;
    }

    void ErrorOccurred(string text)
    {
        error.gameObject.SetActive(true);
        error.SetText(text);
        ResetInput();
    }

    void ResetInput()
    {
        username.text = "";
        password.text = "";
        newUsername.text = "";
        newPassword.text = "";
    }
    #endregion
}
