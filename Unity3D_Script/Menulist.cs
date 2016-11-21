using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Prime31.TransitionKit;

public class Menulist : MonoBehaviour {

	private UIPanel pnList, pnCart;
	private UILabel lbMenuName, count_checkout, lbItemCount;
	private UISprite imgCartPlus;
	private UIButton btnPlaceAnOrder;

	private GameObject sv_menulist;
	private Animator animation;

	private List<DAKGOGI.MainDish> lstDishes;
	private DAKGOGI.MainDish dishInformation;

	private string strCaption;
	private int nSelectedQTY = 0;
	private double dTotalPrice = 0.0f;
	private bool isPaused = false;
	
	void Awake() {
		lbMenuName = GameObject.Find ("LB_MENU_NAME").GetComponent<UILabel> ();
		imgCartPlus = GameObject.Find ("IMG_CARTPLUS").GetComponent<UISprite> ();
		lbItemCount = GameObject.Find ("LB_ITEMCOUNT").GetComponent<UILabel> ();
		btnPlaceAnOrder = GameObject.Find ("BTN_ADDTOORDER").GetComponent<UIButton> ();
		sv_menulist = GameObject.Find ("SV_MENULIST");
		pnList = GameObject.Find ("List").GetComponent<UIPanel> ();
		pnCart = GameObject.Find ("Cart").GetComponent<UIPanel> ();
	}

	// Use this for initialization
	void Start () {

		DAKGOGI.NDakgogiManager.Instance.CheckTheCart (imgCartPlus, lbItemCount);

		string strMenuName = "";
		if (DAKGOGI.NDakgogiManager.Instance.GetSubCurrentCategory () != "") 
		{
			lbMenuName.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
			strMenuName = DAKGOGI.NDakgogiManager.Instance.GetSubCurrentCategory ();

			DAKGOGI.MainDish dish = DAKGOGI.DishesManager.Instance.GetDishInformationByIndex (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex());

			if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
				lbMenuName.text = dish.getKorName();
			else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
				lbMenuName.text = dish.getEngName();
			else 
				lbMenuName.text = dish.getChnName();
		}
		else {
			strMenuName = DAKGOGI.NDakgogiManager.Instance.GetCurrentCategory ();
			lbMenuName.text = DAKGOGI.NDakgogiManager.Instance.GetCurrentCategory ();
		}

		DAKGOGI.DishesManager.Instance.GetMainDishesByCategory (strMenuName, out lstDishes);

		// Change to Previous Page
		UIButton btnBack = GameObject.Find ("BackTitle").GetComponent<UIButton>();
		btnBack.onClick.Add (new EventDelegate (ClickButton_Main));

		//
		btnPlaceAnOrder.onClick.Add (new EventDelegate (ClickButton_PlaceAnOrder));

		//
		UIButton btnCart = pnCart.GetComponent<UIButton> ();
		btnCart.onClick.Add (new EventDelegate (ClickButton_CheckOut));

		SetAllItemsInScrollView ();

		// for an animation
		Time.timeScale = 1;
		animation = pnCart.GetComponent<Animator>();
		animation.enabled = false;

		// for springpanel
//		GameObject springPanel = sv_menulist.GetComponent<SpringPanel>().gameObject;
//		Vector3 pos = new Vector3(0, DAKGOGI.NDakgogiManager.Instance.GetShortCut());
//
//		if (DAKGOGI.NDakgogiManager.Instance.GetShortCut () > 70) {
//			ClickButton_IfSpreadOrSideMenu ();
//		}
//
//		SpringPanel.Begin (springPanel, pos, 10.0f);
	}
	
	// Update is called once per frame
	void Update () {

		//충돌이 감지된 영역
		RaycastHit hit;
		
		GameObject target = null;
		
		if (true == Input.GetMouseButtonDown (0)) {
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100)) {
				Debug.Log ("name : " + hit.collider.name);
				
				target = hit.collider.gameObject;
				UIPanel panel = target.GetComponentInParent<UIPanel> ();

				int nChoicedIndex = -1;
				if (panel.name == "Header" || panel.name == "Btn" || panel.name == "Cart" || panel.name == "BTN_ADDTOORDER")
					return;

				if (!System.Int32.TryParse (panel.name, out nChoicedIndex)) {
					Debug.Log ("Failed convert to int!!!! from string : " + panel.name);
					return;
				}

				if (dishInformation == null || DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex () != nChoicedIndex) 
				{
					if (nSelectedQTY > 0)
						InitOrderedMenu();

					DAKGOGI.NDakgogiManager.Instance.SetCurrentDishIndex (nChoicedIndex);
					dishInformation = DAKGOGI.DishesManager.Instance.GetDishInformationByIndex (nChoicedIndex);
				}
			}
		}
	}

	public void InitOrderedMenu()
	{
		nSelectedQTY = 0;
		dTotalPrice = 0.0;
		SetAllItemsInScrollView();
		DAKGOGI.NDakgogiManager.Instance.SetSubCurrentCategory ("");
		DAKGOGI.NDakgogiManager.Instance.SetShortCut (70);
	}

	public void SetAllItemsInScrollView ()
	{
		// to initialize previous item lists
		List<GameObject> oldItems = new List<GameObject> ();

		foreach (UIDragScrollView ob in gameObject.GetComponentsInChildren<UIDragScrollView>()) {				
			
			ob.gameObject.transform.parent = null;
			oldItems.Add (ob.gameObject);
			ob.gameObject.SetActive (false);
		}
		foreach (GameObject go in oldItems) {
			NGUITools.Destroy (go);
		}

		// Place to Order button
		if (nSelectedQTY <= 0) {
			btnPlaceAnOrder.defaultColor = new Color32(127, 127, 127, 255);
			btnPlaceAnOrder.state = UIButtonColor.State.Disabled;
			btnPlaceAnOrder.enabled = false;

		} else {
			btnPlaceAnOrder.defaultColor = new Color32(19, 118, 00, 255);
			btnPlaceAnOrder.enabled = true;
		}

		int nHeight = 570;

		// Create Menulist by Category
		foreach (DAKGOGI.MainDish dish in lstDishes)
		{
			GameObject menu;
			if (true == dish.getSpread()) {
				menu = Resources.Load ("Prefabs/ListItemSpread") as GameObject;
			} else {
				menu = Resources.Load ("Prefabs/ListItemFold") as GameObject;
			}
			
			GameObject menuPanel = NGUITools.AddChild(sv_menulist, menu);
			if (menuPanel == null)
			{
				Debug.Log ("Failed loading panel!");
				return;
			}
			
			menuPanel.name = dish.getIndex ().ToString();
			menuPanel.SetActive(true);
			menuPanel.transform.localPosition = new Vector3(0, nHeight);
			
			if (true == dish.getSpread()) {
				nHeight -= 423;
			} else {			
				nHeight -= 288;
			}

			UIButton[] buttons = menuPanel.GetComponentsInChildren<UIButton>();
			foreach(UIButton button in buttons)
			{
				if (button.name == "ItemPlusBtn")
					button.onClick.Add(new EventDelegate(ClickButton_IncreaseQTY));
				else if (button.name == "ItemMinusBtn")
					button.onClick.Add(new EventDelegate(ClickButton_DecreaseQTY));
				else
					button.onClick.Add(new EventDelegate(ClickButton_IfSpreadOrSideMenu));
			}
			
			UISprite[] sprites = menuPanel.GetComponentsInChildren<UISprite>();
			if (sprites.Length <= 0)
			{
				Debug.Log ("Failed loading sprites!");
				return;
			}
			
			foreach (UISprite sprite in sprites)
			{
				if (sprite.name == "ThumItem")
				{
					sprite.spriteName = "thum_list_" + dish.getImageName();
				}
			}
			
			UILabel[] lables = menuPanel.GetComponentsInChildren <UILabel>();
			if (lables.Length == 0)
			{
				Debug.Log ("Failed loading Caption!");
				return;
			}
			
			foreach (UILabel label in lables)
			{
				if (label.name == "LabelItemTitle")
				{
					label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();

					if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
						label.text = dish.getKorName();
					else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
						label.text = dish.getEngName();
					else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
						label.text = dish.getChnName();
				}
				else if (label.name == "LabelItemExplanation")
				{
					label.text = dish.getExplanation();
				}
				else if (label.name == "LabelItemPriceW")
				{
					if (DAKGOGI.DishesManager.Instance.GetOptionType(dish.getIndex()) != "" ||
					    dish.getSubCategory() != "")
						label.text = "from $ " + dish.getPrice().ToString();
					else
						label.text = "$ " + dish.getPrice().ToString();
				}
				else if (label.name == "LabelItemTotalPrice")
				{
					label.text = "$ " + dTotalPrice.ToString();
				}
				else if (label.name == "LabelItemQty")
				{
					label.text = nSelectedQTY.ToString();
				}
			}
		}
	}

	public void ClickButton_Main() {

		if (DAKGOGI.NDakgogiManager.Instance.GetSubCurrentCategory () != "") 
		{
			InitOrderedMenu ();
			Application.LoadLevel ("Menulist");

//			var slices = new VerticalSlicesTransition ()
//			{
//				nextScene = 1,		// Menulist
//				divisions = 1,
//				dirOffset = 1,
//			};
//			TransitionKit.instance.transitionWithDelegate (slices);
		} 
		else 
		{
			Application.LoadLevel ("Main");
//			var slices = new VerticalSlicesTransition ()
//			{
//				nextScene = 0,		// Main
//				divisions = 1,
//				dirOffset = -1
//			};
//			TransitionKit.instance.transitionWithDelegate (slices);
		}

		DAKGOGI.NDakgogiManager.Instance.SetShortCut (70);
	}

	public void ClickButton_IfSpreadOrSideMenu()
	{
		DAKGOGI.MainDish maindish = DAKGOGI.DishesManager.Instance.GetDishInformationByIndex (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex ());
		if (maindish.getCategory() != -1 && maindish.getSubCategory() != "")
		{
			DAKGOGI.NDakgogiManager.Instance.SetSubCurrentCategory(maindish.getSubCategory());
			Application.LoadLevel ("Menulist");
//			var slices = new VerticalSlicesTransition()
//			{
//				nextScene = 1,		// Menulist
//				divisions = 1,
//				dirOffset = 1,
//			};
//			TransitionKit.instance.transitionWithDelegate( slices );
		}
		else
		{
			DAKGOGI.NDakgogiManager.Instance.SetShortCut (70);

			if (DAKGOGI.DishesManager.Instance.IfOptionsAreExistByIndex(DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex()))
			{
				Application.LoadLevel ("Detail");
//				var slices = new VerticalSlicesTransition()
//				{
//					nextScene = 5,		// Detail
//					divisions = 1,
//					dirOffset = 1,
//				};
//				TransitionKit.instance.transitionWithDelegate( slices );
			} 
			else 
			{
				DAKGOGI.MainDish choiced_dish = lstDishes.Find (x => x.getIndex() == DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex());
				if (true == choiced_dish.getSpread())
				{
					choiced_dish.SetSpread(false);
				}
				else
				{
					choiced_dish.SetSpread(!choiced_dish.getSpread());
					nSelectedQTY = 0;
					dTotalPrice = 0.0;
				}

				foreach(DAKGOGI.MainDish dish in lstDishes)
				{
					if (true == dish.getSpread() && (dish.getIndex() != DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex()))
					{
						dish.SetSpread(false);
					}
				}

				SetAllItemsInScrollView ();
			}
		}
	}

	public void ClickButton_CheckOut() {

		InitOrderedMenu ();

		if (false == DAKGOGI.NDakgogiManager.Instance.CheckTheCart (imgCartPlus, lbItemCount))
			return;

		DAKGOGI.NDakgogiManager.Instance.SetPreviousPage ("Menulist");
		Application.LoadLevel ("Order");
	}

	public void ClickButton_IncreaseQTY() {
		
		nSelectedQTY++;
		dTotalPrice += dishInformation.getPrice ();
		dTotalPrice = Math.Round (dTotalPrice, 2);

		SetAllItemsInScrollView ();
	}
	
	public void ClickButton_DecreaseQTY() {
		
		if (--nSelectedQTY < 0)
			nSelectedQTY = 0;
		
		dTotalPrice -= dishInformation.getPrice ();
		dTotalPrice = Math.Round (dTotalPrice, 2);
		if (dTotalPrice < 0)
			dTotalPrice = 0.00;

		SetAllItemsInScrollView ();
	}

	public void ClickButton_PlaceAnOrder()
	{
		if (nSelectedQTY <= 0)
			return;

		DAKGOGI.NDakgogiManager.Instance.PlaceAnOrder (dishInformation, null, nSelectedQTY);
		DAKGOGI.NDakgogiManager.Instance.CheckTheCart (imgCartPlus, lbItemCount);
		
		PlayAddToCartAnimation ();

		InitOrderedMenu ();
	}

	public void PlayAddToCartAnimation()
	{
		if (animation.enabled == false) {
			animation.enabled = true;
			animation.Play ("AddToCart");
			Time.timeScale = 0;
		} else {
			animation.enabled = false;
			Time.timeScale = 1;
			
			animation.enabled = true;
			animation.Play ("AddToCart");
			Time.timeScale = 0;
		}
	}
}
