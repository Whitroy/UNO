using System;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class AuthUIManager : MonoBehaviour
{
    [Header("Auth UI")]
    [SerializeField] private Canvas signInUICanvas = null;
    [SerializeField] private Canvas signUpUICanvas = null;
    [SerializeField] private Canvas errorUICanvas = null;

    [Header("sign Up")]
    [SerializeField] private TMP_InputField signUpPasswordInput = null;
    [SerializeField] private TMP_InputField signUpCnfPasswordInput = null;
    [SerializeField] private TMP_InputField signUpUserNameInput = null;
    [SerializeField] private TMP_InputField signUpEmailIdInput = null;

    [Header("sign In")]
    [SerializeField] private TMP_InputField signInPasswordInput = null;
    [SerializeField] private TMP_InputField signInEmailIdInput = null;

    [Header("Error UI")]
    [SerializeField] private TextMeshProUGUI errorText = null; 


    private Button previousCloseButton = null;

    private readonly string EmailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
    private readonly string PasswordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";

    public void SignIn()
    {
        //Sign to game
        string email = signInEmailIdInput.text.Trim();
        string password = signInPasswordInput.text.Trim();
        bool result = FirebaseManager.Instance.SignInUser(email,password);

        if (result)
        {
            Debug.Log($"<color=green>Sign In Successfully.</color>");
        }
    }

    public void Close()
    {
        //back to mainMain
    }

    public void ErrorClose()
    {
        if (previousCloseButton != null)
            previousCloseButton.interactable = true;

        previousCloseButton = signInUICanvas.transform.GetChild(0).GetComponentInChildren<Button>();
        if (!signInUICanvas.enabled)
            previousCloseButton = null;

        errorUICanvas.enabled = false;
    }

    public void SignUp()
    {
        //move to SignUpPage
        signInUICanvas.enabled = false;
        signUpUICanvas.enabled = true;
        previousCloseButton = signInUICanvas.transform.GetChild(0).GetComponentInChildren<Button>();
    }

    public void Register()
    {
        RegisterData();
    }

    private void RegisterData()
    {
        string userName = signUpUserNameInput.text.Trim();
        string password = signUpPasswordInput.text.Trim();
        string cnfpassword = signUpCnfPasswordInput.text.Trim();
        string email = signUpEmailIdInput.text.Trim();

        bool validatationResult = Validate(userName,password,cnfpassword,email);

        if (validatationResult)
        {
            bool result = FirebaseManager.Instance.SignUpNewUser(email,password);
            Debug.Log($"<color=green>Sign In Successfully.</color>");
            FirebaseManager.Instance.SaveData(userName, email, password);
            if (result)
            {
                //auto signin
                bool signInResult = FirebaseManager.Instance.SignInUser(email,password);
                if (signInResult)
                {

                }
            }
        }

        signUpUserNameInput.text = "";
        signUpPasswordInput.text = "";
        signUpCnfPasswordInput.text = "";
        signUpEmailIdInput.text = "";
    }

    private bool Validate(string userName, string password, string cnfPassword, string email)
    {
        previousCloseButton = signUpUICanvas.transform.GetChild(0).GetComponentInChildren<Button>();
        if (userName.Length == 0)
        {
            showErrorUI("Username must be a alphanumeric value!",previousCloseButton);
            return false;
        }
        
        /*if(email.Length < 5 || validateEmail(email))
        {
            showErrorUI("Invalid Email ID!",previousCloseButton);
            return false;
        }
        */

        if(password.Length < 6 || validatePassword(password))
        {
            showErrorUI("Weak Password! (Use special Charcters and alphanumeric value and length must be greater than 8)",previousCloseButton);
            return false;
        }

        if(cnfPassword.Length != password.Length || password.Equals(signUpCnfPasswordInput))
        {
            showErrorUI("Password doesn't match!",previousCloseButton);
            return false;
        }

        return true;
    }

    private bool validateEmail(string email)
    {
        return Regex.IsMatch(email, EmailRegex);
    }

    private bool validatePassword(string password)
    {
        return Regex.IsMatch(password, PasswordRegex);
    }

    private void showErrorUI(string message, Button previousCloseButton)
    {
        errorUICanvas.enabled = true;
        if (previousCloseButton != null)
            previousCloseButton.interactable = false;

        errorText.text = message;
    }

    public void Back()
    {
        //move to signUpPage
        signUpUICanvas.enabled = false;
        signInUICanvas.enabled = true;
        previousCloseButton = null;
    }

}
