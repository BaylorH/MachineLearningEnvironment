using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackManager : MonoBehaviour
{
    public TMP_InputField feedbackInputField;
    public Button submitButton;
    public TextMeshProUGUI feedbackMessage;

    private readonly string email = "3d.ml.learning.environment@gmail.com";
    private readonly int smtpPort = 587; // gmail SMTP port
    private readonly string smtpPassword = "awnujokciuwpgskd"; // password for the email account

    void Start()
    {
        submitButton.onClick.AddListener(SendEmail);
    }

    public void SendEmail()
    {
        try
        {
            var mail = new MailMessage
            {
                From = new MailAddress(email),
                Subject = "Feedback from Unity App",
                Body = $"Feedback:\n{feedbackInputField.text}\n\n" +
                   $"________\n\n" +
                   $"Model: {SystemInfo.deviceModel}\n" +
                   $"OS: {SystemInfo.operatingSystem}\n" +
                   $"________"
            };
            mail.To.Add(email);

            // Configure the SMTP client
            var smtpServer = new SmtpClient("smtp.gmail.com")
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(email, smtpPassword),
                EnableSsl = true
            };

            // Bypass SSL certificate validation (not recommended for production)
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };

            // Send the email
            smtpServer.Send(mail);

            feedbackMessage.text = "Feedback sent successfully!";
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to send email: " + ex.Message);
            feedbackMessage.text = "Error: Unable to send feedback.";
        }
    }
}