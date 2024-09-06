using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public TMP_InputField feedbackInputField; // Input field for the feedback text
    public Button submitButton; // Button to submit feedback
    public Text feedbackMessage; // Text to display messages (success or error)

    // Replace with your actual email address
    private string email = "3d.ml.learning.environment@gmail.com";

    void Start()
    {
        submitButton.onClick.AddListener(SendEmail);
    }

    public void SendEmail()
    {
        string subject = MyEscapeURL("My Subject");
        string body = MyEscapeURL("Please Enter your message here.");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string URL)
    {
        return Uri.EscapeDataString(URL).Replace("+", "%20");
    }


}
