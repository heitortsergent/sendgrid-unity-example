using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
 
public class SG_Email : MonoBehaviour {
 
	string api_user = "YOUR_SENDGRID_USERNAME";
	string api_key = "YOUR_SENDGRID_PASSWORD";
	
	string fromEmail = "FROM_ADDRESS_HERE";
	string toEmail = "TO_ADDRESS_HERE";
	string subject = "Sendgrid Email Awesomeness";
	string body = "Using Sendgrid to send an email from your game. :)";
	string xsmtpapiJSON = "{\"category\":\"unity_game_email\"}";
	
	public void SendSendgridEmailSMTP ()
	{
	    MailMessage mail = new MailMessage();
	
	    mail.From = new MailAddress(fromEmail);
	    mail.To.Add(toEmail);
	    mail.Subject = subject;
	    mail.Body = body;
		mail.Headers.Add("X-SMTPAPI", xsmtpapiJSON);
	
	    SmtpClient smtpServer = new SmtpClient("smtp.sendgrid.net");
	    smtpServer.Port = 587;
	    smtpServer.Credentials = new System.Net.NetworkCredential(api_user, api_key) as ICredentialsByHost;
	    smtpServer.EnableSsl = true;
	    ServicePointManager.ServerCertificateValidationCallback = 
	        delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
	            { return true; };
	    smtpServer.Send(mail);
	 	Debug.Log("Success, email sent through SMTP!");
	}
 
    public void SendSendgridEmailWebAPI () {
        string url = "https://sendgrid.com/api/mail.send.json?";
		
		url += "to=" + toEmail;
		url += "&from=" + fromEmail;
		
		//you have to change every instance of space to %20 or you'll get a 400 error
		string subjectWithoutSpace = subject.Replace(" ", "%20");
		url += "&subject=" + subjectWithoutSpace;
		string bodyWithoutSpace = body.Replace(" ", "%20");
		
		url += "&text=" + bodyWithoutSpace;
		url += "&x-smtpapi=" + xsmtpapiJSON;
		url += "&api_user=" + api_user;
		url += "&api_key=" + api_key;
		
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }
 
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
 
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok! Email sent through Web API: " + www.text);
        } else {
            Debug.Log("WWW Error: "+ www.error);
        }    
    }
}