using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class Delivery : MonoBehaviour {

	GameObject delivery, pickup;
	UIPanel parent;
	UILabel lbPaymentMethod, lbOrder;
	UIToggle radioCASH, radioCREDIT;
	UIInput inputName, inputEmail, inputPhoneNumber;
	UIInput inputAddress, inputProvince, inputCity, inputPostalCode;
	UIInput inputInstructions;
	UIButton btnSendToEmail;

	string[] aryPersonalInformation = new string[15];

	void Awake() {
		delivery = GameObject.Find ("Delivery");
		pickup = GameObject.Find ("Pickup");

		parent = GameObject.Find ("DeliveryPickup").GetComponent<UIPanel> ();
		UIButton btnBack = GameObject.Find ("BackTitle").GetComponent<UIButton>();
		btnBack.onClick.Add (new EventDelegate (ClickButton_Order));

		btnSendToEmail = GameObject.Find ("BtnNext").GetComponent<UIButton>();
		btnSendToEmail.onClick.Add (new EventDelegate (SendToOrders));

		lbPaymentMethod = GameObject.Find ("LB_PAYMENT_METHOD").GetComponent<UILabel> ();
		lbOrder = GameObject.Find ("LB_ORDER").GetComponent<UILabel> ();

		// for PersonalInformation
		inputName = GameObject.Find ("TextBoxName").GetComponent<UIInput> ();
		inputEmail = GameObject.Find ("TextBoxEMail").GetComponent<UIInput> ();
		inputPhoneNumber = GameObject.Find ("TextBoxPhoneNumber").GetComponent<UIInput> ();

		if (DAKGOGI.NDakgogiManager.Instance.GetIsDelivery() == true)
		{
			// for Delivery
			inputAddress = GameObject.Find ("TextBoxAddress").GetComponent<UIInput> ();
			inputProvince = GameObject.Find ("TextBoxProvince").GetComponent<UIInput> ();
			inputCity = GameObject.Find ("TextBoxCity").GetComponent<UIInput> ();
			inputPostalCode = GameObject.Find ("TextBoxPostalCode").GetComponent<UIInput> ();

			radioCASH = GameObject.Find ("RadioCASH").GetComponent<UIToggle> ();
			radioCREDIT = GameObject.Find ("RadioCREDIT").GetComponent<UIToggle> ();

			radioCASH.value = true;
			radioCREDIT.value = false;
		} 
		else 
		{
			// for Pickup
			inputInstructions = GameObject.Find ("TextBoxInstructions").GetComponent<UIInput> ();
		}
	}

	// Use this for initialization
	void Start () {

		if (DAKGOGI.NDakgogiManager.Instance.GetIsDelivery() == true) {
			delivery.SetActive (true);
			pickup.SetActive (false);
			lbPaymentMethod.text = "DELIVERY";

		} else {
			delivery.SetActive(false);
			pickup.SetActive(true);
			lbPaymentMethod.text = "PICKUP";
		}

		ReadPersonalDataFromFile ();
			
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReadPersonalDataFromFile()
	{
		List<string> lstReadData;

		if (!readStringFromFile ("Dakgogi.txt", out lstReadData)) {
			Debug.Log ("Failed loading Dakgogi.txt");

			for (int i=0; i<8; i++)
				aryPersonalInformation[i] = "";

			return;
		} else {

			int nCNT=0;
			foreach (string line in lstReadData)
			{
				string remain = line.Substring(14);
				aryPersonalInformation[nCNT] = remain;

				if (DAKGOGI.NDakgogiManager.Instance.GetIsDelivery())
				{
					switch(nCNT++)
					{
					case 0: inputName.value = remain; break;
					case 1: inputEmail.value = remain; break;
					case 2: inputPhoneNumber.value = remain; break;
					case 3: inputAddress.value = remain; break;
					case 4: inputProvince.value = remain; break;
					case 5: inputCity.value = remain; break;
					case 6: inputPostalCode.value = remain; break;
					case 7: 
						{
							if (remain == "True") {
								radioCASH.value = true;
								radioCREDIT.value = false;
							}
							else {
								radioCASH.value = false;
								radioCREDIT.value = true;
							}
							break;
						}
					}
				}
				else
				{
					switch(nCNT++)
					{
					case 0: inputName.value = remain; break;
					case 1: inputEmail.value = remain; break;
					case 2: inputPhoneNumber.value = remain; break;
					}
				}
			}
		}
	}

	private static bool RemoveAll(string s)
	{
		return s.ToLower().EndsWith("");
	}

	public void SavePersonalDataToFile()
	{
		string strFileData = "";

		aryPersonalInformation[0] = inputName.value;
		aryPersonalInformation[1] = inputEmail.value;
		aryPersonalInformation[2] = inputPhoneNumber.value;

		if (true == DAKGOGI.NDakgogiManager.Instance.GetIsDelivery()) {
			aryPersonalInformation[3] = inputAddress.value;
			aryPersonalInformation[4] = inputProvince.value;
			aryPersonalInformation[5] = inputCity.value;
			aryPersonalInformation[6] = inputPostalCode.value;

			if (radioCASH.value == true)
				aryPersonalInformation[7] = "True";
			else
				aryPersonalInformation[7] = "False";
		}

		// warning in order to modify these forms, you should chkeck them.
		strFileData += "Name        : " + aryPersonalInformation[0] + "\n";
		strFileData += "Email       : " + aryPersonalInformation[1] + "\n";
		strFileData += "Phone       : " + aryPersonalInformation[2] + "\n";
		strFileData += "Address     : " + aryPersonalInformation[3] + "\n";
		strFileData += "Province    : " + aryPersonalInformation[4] + "\n";
		strFileData += "City        : " + aryPersonalInformation[5] + "\n";
		strFileData += "PostalCode  : " + aryPersonalInformation[6] + "\n";
		strFileData += "CASH        : " + aryPersonalInformation[7] + "\n";

		writeStringToFile (strFileData, "Dakgogi.txt");
	}

	// File Access
	public void writeStringToFile(string str, string filename)
	{
#if !WEB_BUILD
		string path = pathForDocumentsFile( filename );
		FileStream file = new FileStream (path, FileMode.Create, FileAccess.Write);
		 
		StreamWriter sw = new StreamWriter( file );
		sw.WriteLine( str );

		sw.Close();
		file.Close();
#endif 
	}

	public bool readStringFromFile(string filename, out List<string> lstReadData)//, int lineIndex )
	{
#if !WEB_BUILD
		lstReadData = new List<string>();
		string path = pathForDocumentsFile( filename );

		if (File.Exists(path))
		{
			FileStream file = new FileStream (path, FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader( file );
			 
			while(!sr.EndOfStream)
			{
				string strRead = sr.ReadLine();
				if (strRead.Length > 0)
					lstReadData.Add (strRead);
			}
			 
			sr.Close();
			file.Close();

			return true;
		}
		else
		{
		    return false;
		}
#else
		return false;
#endif 
	}

	public string pathForDocumentsFile(string filename) 
	{ 
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//string path = Application.dataPath.Substring( 0, Application.dataPath.Length - 5 );
			string path = Application.persistentDataPath; 
			path = path.Substring( 0, path.LastIndexOf( '/' ) );
			return Path.Combine( Path.Combine( path, "Documents" ), filename );
		}
		else if(Application.platform == RuntimePlatform.Android)
		{
			string path = Application.persistentDataPath; 
			path = path.Substring(0, path.LastIndexOf( '/' ) ); 
			return Path.Combine (path, filename);
		} 
		else 
		{
			string path = Application.dataPath; 
			path = path.Substring(0, path.LastIndexOf( '/' ) );
			return Path.Combine (path, filename);
		}
	}

	// Send to Orders via E-mail
	public void SendToOrders()
	{
		// check each of the input boxes
		if (!CheckInputBoxes ())
			return;

		// To write the body
		string mail_body = null;

		// personal information
		mail_body += "<font size='5' color='Green'>Personal Information</font><br>";
		mail_body += "------------------------------------------------------------<br>";
		mail_body += "Name : " + inputName.value + "<br>";
		mail_body += "Email : " + inputEmail.value + "<br>";
		mail_body += "Phone Number : (" + inputPhoneNumber.value + ") <br>";
		mail_body += "<br><br>";
		
		// delivery or pickup
		if (true == DAKGOGI.NDakgogiManager.Instance.GetIsDelivery()) {
			mail_body += "<font size='5' color='Green'>Delivery Information</font><br>";
			mail_body += "------------------------------------------------------------<br>";
			mail_body += " Address : " + inputAddress.value + " <br>";
			mail_body += "Province : " + inputProvince.value + " / City : " + inputCity.value + "<br>";
			mail_body += "Postal Code : " + inputPostalCode.value + "<br>";
			mail_body += "CASH or CREDIT : ";
			if (radioCASH.value == true)
				mail_body += "CASH <br>";
			if (radioCREDIT.value == true)
				mail_body += "CREDIT <br>";
		} else {
			mail_body += "<font size='5' color='Green'>Pickup Information</font><br>";
			mail_body += "------------------------------------------------------------<br>";
			mail_body += "Instructions : " + inputInstructions.value + "<br>";
		}
		mail_body += "<br><br>";
		
		mail_body += "<font size='5' color='Green'>Order Contents</font><br>";
		mail_body += "------------------------------------------------------------<br>";

		double subTotalPrice = 0.0;
		foreach (KeyValuePair<int, List<DAKGOGI.OrderedDish>> ordered in DAKGOGI.NDakgogiManager.Instance.getListOfOredered()) 
		{
			foreach(DAKGOGI.OrderedDish dish in ordered.Value)
			{
				double subPrice = 0.0;
				mail_body += "Dish Name : " + dish.getMainDish().getKorName () + "<br>";

				///
				if (dish.getOrderAmount() > 0)
					mail_body += "Quantity : " + dish.getOrderAmount() + "<br>";

				int nCount = dish.getOrderAmount();
				if (dish.getOptionType() != "SUB" && nCount <= 0)
					nCount = 1;
				subPrice = nCount * dish.getMainDish().getPrice();
				subTotalPrice += subPrice;

				///
				foreach(DAKGOGI.Option op in dish.getOptions())
				{
					mail_body += "Option : " + op.getKorName() + "<br>";
					subPrice += op.getPrice();
					subTotalPrice += op.getPrice ();
				}

				mail_body += "Price : $" + subPrice.ToString() + "<br><br>";
			}
			mail_body += "<br>";
		}
		
		mail_body += "<font size='5' color='Green'>Invoice</font><br>";
		mail_body += "------------------------------------------------------------<br>";
		mail_body += "SubTotalPrice : $ " + subTotalPrice  + "<br>";
		mail_body += "Tax : $ " + DAKGOGI.NDakgogiManager.Instance.CalculateTax(subTotalPrice) + "<br>";
		mail_body += "Delivery Charge : $ " + DAKGOGI.NDakgogiManager.Instance.GetDeliveryCharge() + "<br>";
		//mail_body += "Discount Coupon : $ 0.00" + "<br>";
		mail_body += "TotalPrice : $ " + DAKGOGI.NDakgogiManager.Instance.CalculateTotalPrice(subTotalPrice) + "<br>";
		mail_body += "<br><br>";

		// Send to an email by each OS
		MailMessage mail = new MailMessage ();
		mail.From = new MailAddress ("bback0724@gmail.com");
		mail.IsBodyHtml = true;
		mail.To.Add ("bback0724@gmail.com");
		mail.Subject = "Dakgogi Order_" + System.DateTime.Now.ToString ("yyyy.MM.dd  h:mm:ss tt");
		mail.Body = mail_body;

		SmtpClient smtpServer = new SmtpClient ("smtp.gmail.com");
		//smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential ("bback0724@gmail.com", "!Samsung9") as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		//smtpServer.UseDefaultCredentials = false;
		//smtpServer.Timeout = 2000;

		ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;//delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 

		bool bIsError = false;
		try {
		
			smtpServer.Send (mail);

			// for saving file
			SavePersonalDataToFile ();
			
			DAKGOGI.NDakgogiManager.Instance.getListOfOredered ().Clear ();

			lbOrder.text = "COMPLETED!";

			btnSendToEmail.defaultColor = new Color32(127, 127, 127, 255);
			btnSendToEmail.state = UIButtonColor.State.Disabled;
			btnSendToEmail.enabled = false;

			ShowMessageBox (false, "", "");

		} catch (Exception ex) {
			ShowMessageBox (true, "Ooops!!", "Please, check your network connection.");
		}
	}

	private static bool CertificateValidationCallBack(
		object sender,
		System.Security.Cryptography.X509Certificates.X509Certificate certificate,
		System.Security.Cryptography.X509Certificates.X509Chain chain,
		System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{
		return true;

		/*// If the certificate is a valid, signed certificate, return true.
		if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
		{
			return true;
		}
		
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
		{
			if (chain != null && chain.ChainStatus != null)
			{
				foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
				{
					if ((certificate.Subject == certificate.Issuer) &&
					    (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
					{
						// Self-signed certificates with an untrusted root are valid. 
						continue;
					}
					else
					{
						if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
						{
							// If there are any other errors in the certificate chain, the certificate is invalid,
							// so the method returns false.
							return false;
						}
					}
				}
			}
			
			// When processing reaches this line, the only errors in the certificate chain are 
			// untrusted root errors for self-signed certificates. These certificates are valid
			// for default Exchange server installations, so return true.
			return true;
		}
		else
		{
			// In all other cases, return false.
			return false;
		}*/
	}

	string MyEscapeURL (string url)
	{
		return WWW.EscapeURL(url).Replace("+","%20");
	}

	public void ClickButton_Order() {

		if (DAKGOGI.NDakgogiManager.Instance.GetOrderedTotalCount() <= 0)
			Application.LoadLevel ("Main");
		else
			Application.LoadLevel ("Order");
	}

	public void ClickButton_OK() 
	{
		NGUITools.Destroy (GameObject.Find ("CompletePopup"));
	}

	public bool CheckInputBoxes()
	{
		bool isError = false;
		string strTitle = "Sorry!";
		string strMsg = "";

		if (inputName.value.Length <= 0) {
			strMsg = "Please, check your name";
			isError = true;
		}
		if (inputPhoneNumber.value.Length <= 0) {
			strMsg = "Please, check your phonenumber";
			isError = true;
		}
		if (DAKGOGI.NDakgogiManager.Instance.GetIsDelivery()) {
			if (inputAddress.value.Length <= 0) {
				strMsg = "Please, check your address";
				isError = true;
			} else if (inputPostalCode.value.Length <= 5) {
				strMsg = "Please, check your postal code";
				isError = true;
			} else {
				string strZip = inputPostalCode.value.Substring (0, 3);
				if (string.Compare(strZip, "M2M", true) == 0 || string.Compare(strZip, "M2K", true) == 0 || 
					string.Compare(strZip, "M2N", true) == 0 || string.Compare(strZip, "M2L", true) == 0 || 
					string.Compare(strZip, "M2P", true) == 0) 
				{
					isError = false;
				}
				else
				{
					strMsg = "Your area is not in service!";
					isError = true;
				}
			}
		}

		if (isError) {
			ShowMessageBox(true, strTitle, strMsg);
			return false;
		}
		return true;
	}

	public void ShowMessageBox(bool bIsError, string strTitle, string strMsg)
	{
		GameObject shopInfo = Resources.Load ("Prefabs/CompletePopup") as GameObject;
		GameObject pnShopInfo = NGUITools.AddChild(parent.gameObject, shopInfo);
		if (pnShopInfo == null)
		{
			Debug.Log ("Failed loading panel!");
			return;
		}
		pnShopInfo.name = "CompletePopup";
		
		if (bIsError)
		{
			UILabel lbTitle = GameObject.Find ("TItle").GetComponent<UILabel> ();
			lbTitle.text = strTitle;
			
			UILabel lbMsg = GameObject.Find ("Explanation").GetComponent<UILabel> ();
			lbMsg.text = strMsg;
		}
		
		UIButton btnClose = GameObject.Find ("BtnOk").GetComponent<UIButton> ();
		btnClose.onClick.Add (new EventDelegate(ClickButton_OK));
	}
}
