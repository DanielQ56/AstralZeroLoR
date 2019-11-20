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
    private const string UpdateUser = "https://shielded-journey-60422.herokuapp.com/user_update";
    #endregion


    public static UserManager instance = null;

    [SerializeField] GameObject inputPanel;
    [SerializeField] ErrorPanel error;

    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject loadedSuccess;

    #region Input Fields for both Logging In and Signing Up
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_InputField newUsername;
    [SerializeField] TMP_InputField newPassword;
    #endregion

    public PlayerData player;

    private void Awake()
    {
        instance = this;
    }

    //Guest so player is null and tells input manager that the player should not be able to update the amount of cards
    public void Guest()
    {
        ResetInput();
        player = null;
        InputManager.instance.ShouldAllowUpdate();
        inputPanel.SetActive(false);
        CardManager.instance.LoadAllCards();
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
        string getUserURL = GetUser + string.Format("{0}/{1}", newUsername.text, newPassword.text);
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
                Debug.Log(newUser.downloadHandler.text);
                yield break;
            }
        }
        CardManager.instance.LoadAllCards();
        string[] allCards = CardManager.instance.GetAllCardsAsStrings();
        CreateNewPlayer(allCards);
        using (UnityWebRequest newUser = UnityWebRequest.Post(RegisterNewUser, CreatePlayerForm(allCards, newUsername.text, newPassword.text)))
        {
            newUser.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = newUser.SendWebRequest();
            loadingPanel.SetActive(true);
            while (!request.isDone)
            {
                yield return null;
            }
            loadingPanel.SetActive(false);
            if (newUser.responseCode == 500)
            {
                ErrorOccurred("Unexpected Error Occurred");
            }
            else
            {
                Debug.Log("User Created Succesfully!");
                ResetInput();
                InputManager.instance.ShouldAllowUpdate();
                inputPanel.SetActive(false);
            }
        }
    }

    void CreateNewPlayer(string[] allCards)
    {
        player = new PlayerData(newUsername.text, newPassword.text, allCards[0], allCards[1], allCards[2], allCards[3], allCards[4], allCards[5]);
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
            loadingPanel.SetActive(true);
            while (!request.isDone)
            {
                yield return null;
            }
            loadingPanel.SetActive(false);
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
                InputManager.instance.ShouldAllowUpdate();
                inputPanel.SetActive(false);
            }
        }
    }
    #endregion

    #region Update Existing User
    public void UpdateExistingUser()
    {
        if(player != null)
        {
            Debug.Log("Updating");
            StartCoroutine(update());
        }
        else
        {
            Debug.Log("You can't do that!");
        }
    }

    IEnumerator update()
    {
        string[] allCards = CardManager.instance.GetAllCardsAsStrings();
        
        using (UnityWebRequest updateUser = UnityWebRequest.Post(UpdateUser, CreatePlayerForm(allCards, player.id, player.password)))
        {
            updateUser.chunkedTransfer = false;
            UnityWebRequestAsyncOperation request = updateUser.SendWebRequest();
            loadingPanel.SetActive(true);
            while (!request.isDone)
            {
                yield return null;
            }
            loadingPanel.SetActive(false);
            if (updateUser.responseCode == 500)
            {
                ErrorOccurred("Unexpected Error Occurred");
            }
            else
            {
                StartCoroutine(Success());
                Debug.Log("Updated successfully");
            }
        }
    }

    IEnumerator Success()
    {
        loadedSuccess.SetActive(true);
        SendLoadedDecks();
        yield return new WaitForSeconds(1.5f);
        loadedSuccess.SetActive(false);
    }

    public void UpdateSavedDecks(List<string> deckstrings)
    {
        player.d1 = deckstrings[0];
        player.d2 = deckstrings[1];
        player.d3 = deckstrings[2];
        UpdateExistingUser();
    }

    #endregion


    #region Helper Functions

    //This checks the input of Username, Password the two input fields on the menu
    bool CheckForEmptyInput(TMP_InputField user, TMP_InputField pass)
    {
        if (user.text == "" || pass.text == "")
        {
            ErrorOccurred("Username and Password cannot be empty");
            return true;
        }
        return false;
    }


    //Function that is called if the response status from webrequest is 500
    void ErrorOccurred(string text)
    {
        error.gameObject.SetActive(true);
        error.SetText(text);
        ResetInput();
    }

    //Empties the input fields
    void ResetInput()
    {
        username.text = "";
        password.text = "";
        newUsername.text = "";
        newPassword.text = "";
    }


    //Opens the login panel again
    public void BackToMenu()
    {
        InputManager.instance.ClearAllEntries();
        inputPanel.SetActive(true);
        player = null;
    }

    public void SendLoadedDecks()
    {
        List<string> decks = new List<string> { player.d1, player.d2, player.d3 };
        InputManager.instance.LoadSavedDecks(decks);
    }

    WWWForm CreatePlayerForm(string[] allCards, string id, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("password", password);
        for (int i = 0; i < allCards.Length; ++i)
        {
            form.AddField("r" + (i + 1).ToString(), allCards[i]);
        }
        form.AddField("d1", player.d1);
        form.AddField("d2", player.d2);
        form.AddField("d3", player.d3);
        return form;
    }
    #endregion
}
