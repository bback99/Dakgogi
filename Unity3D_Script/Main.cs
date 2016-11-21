using UnityEngine;
using System.Collections;
using System;
using Prime31.TransitionKit;

public class Main : MonoBehaviour {

	// main
	private UIScrollView sv_HotMenu;
	//private int nChildrenCNTofSV;
	private UISprite[] sprites;
	private UISprite imgCartPlus;
	private UIPanel pnMenu, pnCart, pnForSliding;
	private UILabel lbItemCount, lbDeliveryZone, lbDirectCall;
	private Animator aniSlideMenu;
	private bool isPaused = false;
	private long lStartTime = 0, lSlidingTime = 0;
	private Camera camera_;

	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

	// Use this for initialization
	void Awake ()
	{
		// to get handles
		camera_ = GetComponentInChildren<Camera>();
		imgCartPlus = GameObject.Find ("IconCartPlus").GetComponent<UISprite> ();
		lbItemCount = GameObject.Find ("LB_ITEMCOUNT").GetComponent<UILabel> ();
		lbDeliveryZone = GameObject.Find ("LB_DeliveryZone").GetComponent<UILabel>();
		lbDirectCall = GameObject.Find ("LB_DirectCall").GetComponent<UILabel>();

		pnMenu = GameObject.Find ("Main").GetComponent<UIPanel> ();
		pnForSliding = GameObject.Find ("Panel_Sliding").GetComponent<UIPanel> ();

		if (DAKGOGI.NDakgogiManager.Instance.GetCurrentFont () == null)
			DAKGOGI.NDakgogiManager.Instance.SetCurrentFont (GameObject.Find ("NanumBarunGothic").GetComponent<UIFont> ());
		else
			ChangeAllLabelToChangedLanguage ();

		// to initialize data
		DAKGOGI.NDakgogiManager.Instance.OnInit();
	}

	void Start () {

		// for animation
		Time.timeScale = 1;
		aniSlideMenu = pnMenu.GetComponent<Animator>();
		aniSlideMenu.enabled = false;

		pnForSliding.gameObject.SetActive (false);

		// 
		DAKGOGI.NDakgogiManager.Instance.CheckTheCart(imgCartPlus, lbItemCount);
		// for the menu buttons to add deligator
		UIButton [] buttons = GameObject.Find ("Menu").GetComponentsInChildren<UIButton> ();
		foreach (UIButton button in buttons) {

			if (button.name != "SOUP")
				button.onClick.Add (new EventDelegate(ClickButton_Menu));
		}

		// Cart button
		pnCart = GameObject.Find ("Cart").GetComponent<UIPanel> ();
		UIButton btnCart = pnCart.GetComponent<UIButton> ();
		btnCart.onClick.Add (new EventDelegate (ClickButton_CheckOut));

		// Logo Button
		UIButton btnLogo = GameObject.Find ("Logo").GetComponent<UIButton> ();
		btnLogo.onClick.Add (new EventDelegate (ClickButton_Logo));

		// Delivery Zone Button
		UIButton btnDelivery = GameObject.Find ("BtnDeliveryZone").GetComponent<UIButton> ();
		btnDelivery.onClick.Add (new EventDelegate (ClickButton_DeliveryZone));

		// Direct Call
		UIButton btnDirectCall = GameObject.Find ("DirectcallBtn").GetComponent<UIButton> ();
		btnDirectCall.onClick.Add (new EventDelegate (ClieckButton_DirectCall));

		// IconMenu
		UIButton btnSlidePopup = GameObject.Find ("IconMenu").GetComponent<UIButton> ();
		btnSlidePopup.onClick.Add (new EventDelegate (ClickButton_SlideMenu));

		//
		ClickButton_SlideMenu_ChangeLanguage ();

		// For banner
		{
			UIButton btnBanner1 = GameObject.Find ("banner01").GetComponent<UIButton> ();
			btnBanner1.onClick.Add (new EventDelegate (ClieckButton_Banner1));

			UIButton btnBanner2 = GameObject.Find ("banner02").GetComponent<UIButton> ();
			btnBanner2.onClick.Add (new EventDelegate (ClieckButton_Banner2));

//			UIButton btnBanner3 = GameObject.Find ("banner03").GetComponent<UIButton> ();
//			btnBanner3.onClick.Add (new EventDelegate (ClieckButton_Banner3));
//
//			UIButton btnBanner4 = GameObject.Find ("banner04").GetComponent<UIButton> ();
//			btnBanner4.onClick.Add (new EventDelegate (ClieckButton_Banner4));
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (true == Input.GetMouseButtonDown (0))
		{
			Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			
			if (Physics.Raycast (ray.origin, ray.direction * 10, out hit))
			{
				Debug.Log("collider_name : " + hit.collider.name + " / tag_name : " + hit.collider.gameObject.tag);

				if (isPaused && hit.collider.name == "Sliding") 
				{
					ClickButton_SlideMenu();
					lSlidingTime = System.DateTime.Now.Ticks;
				}

				if (DAKGOGI.DishesManager.Instance.GetMainCategory(hit.collider.name) != -1)
					DAKGOGI.NDakgogiManager.Instance.SetCurrentCategory(hit.collider.name);
			}
		}

		{
			if (lStartTime <= 0)
				return;

			long nowTime = System.DateTime.Now.Ticks;
			double timeGap = (nowTime - lStartTime) / 10000.0f;
			
			if (timeGap > 200) 
			{
				lStartTime = 0;
				//lastClickTime = 0;
				NGUITools.Destroy (GameObject.Find ("SlideMenu"));
			}
		}
	}

	public void ClickButton_SlideMenu ()
	{
		if (false == isPaused) 
		{
			lSlidingTime = System.DateTime.Now.Ticks;

			GameObject slide = Resources.Load ("Prefabs/SlideMenu") as GameObject;
			GameObject slideMenuPanel = NGUITools.AddChild(pnMenu.gameObject, slide);
			if (slideMenuPanel == null)
			{
				Debug.Log ("Failed loading panel!");
				return;
			}
			slideMenuPanel.name = "SlideMenu";

			// for sliding menus
			// ShopInfo.
			UIButton btnShopInfo = GameObject.Find ("LabelShopInfo").GetComponent<UIButton> ();
			btnShopInfo.onClick.Add (new EventDelegate (ClickButton_SlideMenu_ShopInfo));
			
			// LabelLanguages
			UIButton btnChangeLanguages = GameObject.Find ("LabelLanguages").GetComponent<UIButton> ();
			btnChangeLanguages.onClick.Add (new EventDelegate (ClickButton_SlideMenu_ChangeLanguage));
			
			//LabelDirectcall
			UIButton btnDirectcall = GameObject.Find ("LabelDirectcall").GetComponent<UIButton> ();
			btnDirectcall.onClick.Add (new EventDelegate (ClieckButton_DirectCall));

			pnForSliding.gameObject.SetActive(true);
			aniSlideMenu.enabled = true; 
			aniSlideMenu.Play ("SlideMenu");
			isPaused = true;
			Time.timeScale = 0;
		} 
		else 
		{
			long nowTime = System.DateTime.Now.Ticks;
			double timeGap = (nowTime - lSlidingTime) / 10000.0f;
			
			if (timeGap <= 200) 
			{
				lSlidingTime = 0;
				return;
			}

			pnForSliding.gameObject.SetActive(false);
			isPaused = false;
			Time.timeScale = 1;
			aniSlideMenu.Play ("SlideMenu0");

			lStartTime = System.DateTime.Now.Ticks;
		}
	}

	public void ClickButton_Menu()
	{
		DAKGOGI.NDakgogiManager.Instance.SetPreviousPage ("Main");
		Application.LoadLevel ("Menulist");

//		var slices = new VerticalSlicesTransition()
//		{
//			nextScene = 1,		// Menulist
//			divisions = 1,
//		};
//		TransitionKit.instance.transitionWithDelegate( slices );
	}

	public void ClickButton_CheckOut ()
	{
		if (false == DAKGOGI.NDakgogiManager.Instance.CheckTheCart (imgCartPlus, lbItemCount))
			return;

		DAKGOGI.NDakgogiManager.Instance.SetPreviousPage ("Main");
		Application.LoadLevel ("Order");
	}
	
	public void ClickButton_LanguageEN()
	{
		DAKGOGI.NDakgogiManager.Instance.SetCurrentFont(GameObject.Find ("NanumBarunGothic").GetComponent<UIFont> ());
		ChangedLanguages ("English");
	}

	public void ClickButton_LanguageKR()
	{
		DAKGOGI.NDakgogiManager.Instance.SetCurrentFont(GameObject.Find ("NanumBarunGothic").GetComponent<UIFont> ());
		ChangedLanguages ("Korean");
	}

	public void ClickButton_LanguageCN()
	{
		DAKGOGI.NDakgogiManager.Instance.SetCurrentFont(GameObject.Find ("Zaozi").GetComponent<UIFont> ());
		ChangedLanguages ("Chinese");
	}

	public void ClickButton_DeliveryZone()
	{
		GameObject deliveryZoneInfo = Resources.Load ("Prefabs/DeliveryZonePopup") as GameObject;
		GameObject pnDeliveryZone = NGUITools.AddChild(pnMenu.gameObject, deliveryZoneInfo);
		if (pnDeliveryZone == null)
		{
			Debug.Log ("Failed loading panel!");
			return;
		}
		pnDeliveryZone.name = "DeliveryZonePopup";

		UIButton btnClose = GameObject.Find ("CloseBtn").GetComponent<UIButton> ();
		btnClose.onClick.Add (new EventDelegate (ClickButton_DeliveryZoneClose));
	}

	public void ClickButton_DeliveryZoneClose()
	{
		NGUITools.Destroy (GameObject.Find ("DeliveryZonePopup"));
	}

	public void ClickButton_Logo()
	{
		Application.OpenURL ("http://www.dakgogi.ca");
	}

	public void ClickButton_Facebook()
	{
		Application.OpenURL ("http://www.facebook.com/Dakgogi-1435646756701535");
	}

	public void ClieckButton_DirectCall()
	{
		Application.OpenURL( "tel://4168347630" );
	}

	public void ChangedLanguages(string strChangedLanguage)
	{
		if (true == DAKGOGI.NDakgogiManager.Instance.SetCurrentLanguage(strChangedLanguage))
			ChangeAllLabelToChangedLanguage ();

		NGUITools.Destroy (GameObject.Find ("LanguageSelectPopup"));
	}

	public void ClickButton_SlideMenu_ShopInfo()
	{
		long nowTime = System.DateTime.Now.Ticks;
		double timeGap = (nowTime - lSlidingTime) / 10000.0f;
		
		if (timeGap <= 200) 
		{
			lSlidingTime = 0;
			return;
		}

		if (!DAKGOGI.NDakgogiManager.Instance.IsSldingMenuFirst())
		{
			if (true == isPaused)
				ClickButton_SlideMenu ();
			else
				return;
		}

		GameObject shopInfo = Resources.Load ("Prefabs/ShopInfoPopup") as GameObject;
		GameObject pnShopInfo = NGUITools.AddChild(pnMenu.gameObject, shopInfo);
		if (pnShopInfo == null)
		{
			Debug.Log ("Failed loading panel!");
			return;
		}
		pnShopInfo.name = "ShopInfoPopup";

		UIButton btnClose = GameObject.Find ("CloseBtn").GetComponent<UIButton> ();
		btnClose.onClick.Add (new EventDelegate(ClickButton_SlideMenu_CloseShopInfo));

		UIButton btnWebsite = GameObject.Find ("Website").GetComponent<UIButton> ();
		btnWebsite.onClick.Add (new EventDelegate(ClickButton_Logo));

		UIButton btnFacebook = GameObject.Find ("Facebook").GetComponent<UIButton> ();
		btnFacebook.onClick.Add (new EventDelegate(ClickButton_Facebook));
	}

	public void ClickButton_SlideMenu_CloseShopInfo()
	{
		NGUITools.Destroy (GameObject.Find ("ShopInfoPopup"));
	}

	public void ClickButton_SlideMenu_ChangeLanguage()
	{
		if (!DAKGOGI.NDakgogiManager.Instance.IsSldingMenuFirst()) 
		{
			if (true == isPaused)
				ClickButton_SlideMenu ();
			else {
				NGUITools.Destroy (GameObject.Find ("LanguageSelectPopup"));
				return;
			}
		}

		GameObject shopInfo = Resources.Load ("Prefabs/LanguageSelectPopup") as GameObject;
		GameObject pnShopInfo = NGUITools.AddChild(pnMenu.gameObject, shopInfo);
		if (pnShopInfo == null)
		{
			Debug.Log ("Failed loading panel!");
			return;
		}
		pnShopInfo.name = "LanguageSelectPopup";

		Component[] objLanguages = pnShopInfo.GetComponentsInChildren <Component>();
		foreach (Component obj in objLanguages) 
		{
			UIButton btnLanguage = obj.gameObject.GetComponentInChildren<UIButton>();

			if (obj.name == "LanguageBtnEN")
			{
				btnLanguage.onClick.Add (new EventDelegate(ClickButton_LanguageEN));
				if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
					btnLanguage.state = UIButtonColor.State.Disabled;
			}
			else if (obj.name == "LanguageBtnKR")
			{
				btnLanguage.onClick.Add (new EventDelegate(ClickButton_LanguageKR));
				if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
					btnLanguage.state = UIButtonColor.State.Disabled;
			}
			else if (obj.name == "LanguageBtnCN")
			{
				btnLanguage.onClick.Add (new EventDelegate(ClickButton_LanguageCN));
				if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
					btnLanguage.state = UIButtonColor.State.Disabled;
			}
		}
	}

	public void ChangeAllLabelToChangedLanguage()
	{
		lbDeliveryZone.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont ();
		lbDirectCall.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont ();

		lbDeliveryZone.text = DAKGOGI.DishesManager.Instance.GetLocalizationByKey(DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage(), "DeliveryZone");
		lbDirectCall.text = DAKGOGI.DishesManager.Instance.GetLocalizationByKey (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage (), "DirectCall");
	}

	public void ClieckButton_Banner1()
	{
		DAKGOGI.NDakgogiManager.Instance.SetPreviousPage ("Main");
		
		Application.LoadLevel ("Detail");

		DAKGOGI.NDakgogiManager.Instance.SetCurrentCategory ("CHICKEN");
		DAKGOGI.NDakgogiManager.Instance.SetCurrentDishIndex (1002);
	}

	public void ClieckButton_Banner2()
	{
		DAKGOGI.NDakgogiManager.Instance.SetPreviousPage ("Main");
		
		Application.LoadLevel ("Menulist");

		DAKGOGI.NDakgogiManager.Instance.SetCurrentCategory ("CHICKEN");
		DAKGOGI.NDakgogiManager.Instance.SetSubCurrentCategory ("CHICKEN");
		DAKGOGI.NDakgogiManager.Instance.SetCurrentDishIndex (1007);
		DAKGOGI.NDakgogiManager.Instance.SetShortCut (800);
	}

	public void ClieckButton_Banner3()
	{
		DAKGOGI.NDakgogiManager.Instance.SetPreviousPage ("Main");
		
		Application.LoadLevel ("Menulist");

		DAKGOGI.NDakgogiManager.Instance.SetCurrentCategory ("CHICKEN");
		DAKGOGI.NDakgogiManager.Instance.SetCurrentDishIndex (1013);
		DAKGOGI.NDakgogiManager.Instance.SetShortCut (71);
	}

	public void ClieckButton_Banner4()
	{
	}
}