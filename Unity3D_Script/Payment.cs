using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class Payment : MonoBehaviour {

	public UILabel lbResult;

	// Use this for initialization
	void Start () {

		//Screen.SetResolution (800, 1280, true);

		lbResult = GameObject.Find ("LB_RESULT").GetComponent<UILabel> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*public void ClickToSend () {
		
		Debug.Log ("Send To Email");
		

		
		Debug.Log ("Success");

		lbResult.text = "Success";
	}*/

	public void ClickToSend () {

		Debug.Log ("Send To Email");
		
		MailMessage mail = new MailMessage ();
		mail.From = new MailAddress ("bback0724@gmail.com");
		mail.IsBodyHtml = true;
		mail.To.Add ("bback0724@gmail.com");
		mail.Subject = "Dakgogi Order_" + System.DateTime.Now.ToString("yyyy.MM.dd  h:mm:ss tt");

/*		foreach (KeyValuePair<int, OrderedProduct> order in DAKGOGI.NDakgogiManager.Instance.getListOfOredered()) {

			mail.Body += "Order Index : " + order.Value.nIndex_ + "<br>";
			mail.Body += "Product Name : " + order.Value.orderdProduct_.strProductKOR_ + "<br>";
			if (order.Value.nWholeQTY_ > 0)	mail.Body += "WHOLE_QTY : " + order.Value.nWholeQTY_ + "<br>";
			if (order.Value.nHalfQTY_ > 0)	mail.Body += "HALF_QTY : " + order.Value.nHalfQTY_ + "<br>";
			mail.Body += "<br>";
		}
		mail.Body += "Tot_price : " + DakgogiManager.Instance.dTotalMoney_ + "<br>";*/
		
		SmtpClient smtpServer = new SmtpClient ("smtp.gmail.com");
		//smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential ("bback0724@gmail.com", "!Samsung9") as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		//smtpServer.UseDefaultCredentials = false;
		//smtpServer.Timeout = 2000;
		ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
		{ return true; };
		smtpServer.Send (mail);

		Debug.Log ("Success");

		lbResult.text = "Success";
	}

	public void LoadOrderlistScene()
	{
		Debug.Log ("Click LoadOrderlistScene");
		
		Application.LoadLevel ("Order");
	}
}
