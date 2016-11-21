using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31.TransitionKit;

public class Detail : MonoBehaviour {

	private UIPanel pnDetail, pnCart;
	private UIPanel[] pnOptions;
	private UILabel lbPlaceToOrder, lbMenuName, lbItemCount;
	private UIButton btnPlaceAnOrder;
	private UISprite imgCartPlus;
	private UIToggle[] toggles;

	private GameObject grid;
	private Animator animation;

	private DAKGOGI.MainDish dishInformation;
	private List<DAKGOGI.Option> lstOption;

	private long lStartTime = 0;

	private string[ , ] aryOptionPage = { {"Choose First Option", "NEXT"}, {"Choose Second Option", "PLACE AN ORDER"}};

	void Awake() {
		pnDetail = GameObject.Find ("Detail").GetComponent<UIPanel> ();
		pnCart = GameObject.Find ("Cart").GetComponent<UIPanel> ();
		imgCartPlus = GameObject.Find ("IMG_CARTPLUS").GetComponent<UISprite> ();
		lbItemCount = GameObject.Find ("LB_ITEMCOUNT").GetComponent<UILabel> ();
		pnOptions = GameObject.Find ("Grid").GetComponent<UIGrid> ().GetComponentsInChildren<UIPanel> ();
		lbPlaceToOrder = GameObject.Find ("LB_ADDTOORDER").GetComponent<UILabel> ();
		btnPlaceAnOrder = GameObject.Find ("BTN_ADDTOORDER").GetComponent<UIButton> ();
		lbMenuName = GameObject.Find ("LB_MENU_NAME").GetComponent<UILabel>();

		grid = GameObject.Find ("Grid");
	}

	// Use this for initialization
	void Start () {

		// for an animation
		Time.timeScale = 1;
		animation = pnCart.GetComponent<Animator>();
		animation.enabled = false;

		DAKGOGI.NDakgogiManager.Instance.CheckTheCart (imgCartPlus, lbItemCount);

		// Change to Previous Page
		UIButton btnBack = GameObject.Find ("BackTitle").GetComponent<UIButton>();
		btnBack.onClick.Add (new EventDelegate (ClickButton_MenuList));
		
		// Place to order button
		btnPlaceAnOrder.onClick.Add (new EventDelegate (ClickButton_PlaceAnOrder));
		
		// Cart button
		UIButton btnCart = pnCart.GetComponent<UIButton> ();
		btnCart.onClick.Add (new EventDelegate (ClickButton_CheckOut));

		if (DAKGOGI.NDakgogiManager.Instance.GetCurrentOrderIndex () > 0) {

			DAKGOGI.MainDish dish = DAKGOGI.DishesManager.Instance.GetDishInformationByIndex (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex ());
			lbMenuName.text = DAKGOGI.DishesManager.Instance.GetMainCategory (dish.getCategory ());

			if (lbMenuName.text.Length <= 0)
				lbMenuName.text = dish.getSubCategory ();
			lbPlaceToOrder.text = "CHANGE YOUR OPTION";
		}

		int nOptionCNT = 0;

		dishInformation = DAKGOGI.DishesManager.Instance.GetDishInformationByIndex (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex ());

		// to set Option panels
		string strCurrentOption = DAKGOGI.DishesManager.Instance.GetOptionCategoryByIndex (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex(), DAKGOGI.NDakgogiManager.Instance.GetOptionPageIndex(), out nOptionCNT);
		if (null == strCurrentOption) 
		{
			Debug.Log ("Current DishIndex is " + DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex());
			return;
		} 

		if (DAKGOGI.NDakgogiManager.Instance.GetOptionPageIndex () >= 1) {
			lbMenuName.text = "Previous Option";
		}
		else 
		{
			lbMenuName.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();

			DAKGOGI.NDakgogiManager.Instance.PrePlaceAnOrder(null);

			if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
				lbMenuName.text = dishInformation.getKorName();
			else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
				lbMenuName.text = dishInformation.getEngName();
			else 
				lbMenuName.text = dishInformation.getChnName();
		}

		// to set Option Labels by OptionPageIndex
		if (nOptionCNT > 1) 
		{
			UILabel label = GameObject.Find ("LabelChoiceStep").GetComponent<UILabel>();
			label.text = aryOptionPage[DAKGOGI.NDakgogiManager.Instance.GetOptionPageIndex(), 0];
			lbPlaceToOrder.text = aryOptionPage[DAKGOGI.NDakgogiManager.Instance.GetOptionPageIndex(), 1];
		}

		lstOption = DAKGOGI.DishesManager.Instance.GetOptionInformations(strCurrentOption);
		
		foreach(DAKGOGI.Option info in lstOption)
		{
			if (DAKGOGI.NDakgogiManager.Instance.ComparePreOption(info))
				continue;

			GameObject op = Resources.Load ("Prefabs/SubMenuItem") as GameObject;
			
			GameObject newOption = NGUITools.AddChild(grid, op);

			newOption.name = info.getIndex().ToString();
			newOption.GetComponentInChildren<UIToggle>().name = info.getIndex().ToString();


			UILabel[] lables = newOption.GetComponentsInChildren <UILabel>();
			foreach (UILabel label in lables)
			{
				if(label.name == "price")
				{
					if (info.getPrice() > 0)
					{
						if (info.getOptionType() != "SUB")
							label.text += "+ ";
						label.text += "$ " + String.Format ("{0,3:0.00}", Math.Round (info.getPrice(), 2));
					}
					else if (info.getPrice() < 0)
						label.text = "$ " + String.Format ("{0,3:0.00}", Math.Round (info.getPrice(), 2));
				}
				else if (label.name == "option_name")
				{
					label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();

					if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
						label.text = info.getKorName();
					else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
						label.text = info.getEngName();
					else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
						label.text = info.getChnName();
				}
			}
		}

		DAKGOGI.Option currentOption = DAKGOGI.NDakgogiManager.Instance.GetCurrentOption ();
		int nCount = currentOption.getIndex ();

		toggles = grid.GetComponentsInChildren<UIToggle> ();
		foreach(UIToggle toggle in toggles)
		{
			if (toggle.name == currentOption.getIndex().ToString())
			{
				toggle.value = true;
			}
			else if (nCount == 0)
			{
				toggle.value = true;
			}
			else 
				nCount++;
		}

		grid.GetComponent<UIGrid> ().Reposition ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (lStartTime <= 0)
			return;

		long nowTime = System.DateTime.Now.Ticks;
		double timeGap = (nowTime - lStartTime) / 10000.0f;
		
		if (timeGap > 500) 
		{
			btnPlaceAnOrder.defaultColor = new Color32(19, 118, 00, 255);
			btnPlaceAnOrder.enabled = true;
			lStartTime = 0;
		}
	}
	
	public void ClickButton_MenuList() {

		int nOptionCNT = 0;
		List<string> lstOptions = DAKGOGI.DishesManager.Instance.GetOptionsByIndex (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex ());

		if (lstOptions.Count > 1 && DAKGOGI.NDakgogiManager.Instance.GetOptionPageIndex() >= 1) {
			DAKGOGI.NDakgogiManager.Instance.DecreaseOptionPageIndex();
			Application.LoadLevel ("Detail");

//			var slices = new VerticalSlicesTransition ()
//			{
//				nextScene = 5,		// Detail
//				divisions = 1,
//				dirOffset = -1
//			};
//			TransitionKit.instance.transitionWithDelegate (slices);
		} else {
			Application.LoadLevel ("Menulist");
//			DAKGOGI.NDakgogiManager.Instance.InitOptions ();
//			
//			var slices = new VerticalSlicesTransition ()
//			{
//				nextScene = 1,		// Menulist
//				divisions = 1,
//				dirOffset = -1
//			};
//			TransitionKit.instance.transitionWithDelegate (slices);
		}
	}

	public void ClickButton_PlaceAnOrder() 
	{
		DAKGOGI.Option newOption = DAKGOGI.NDakgogiManager.Instance.GetCurrentOption();

		foreach(UIToggle toggle in toggles)
		{
			if (true == toggle.value) 
			{
				foreach(DAKGOGI.Option op in lstOption)
				{
					if (op.getIndex().ToString() == toggle.GetComponentInParent<UIPanel>().name)
					{
						newOption = (DAKGOGI.Option)op.ShallowCopy();
						break;
					}
				}
				break;
			}
		}

		if (false == DAKGOGI.DishesManager.Instance.NextOptionPage (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex ())) 
		{
			DAKGOGI.NDakgogiManager.Instance.PlaceAnOrder (dishInformation, newOption, 0);
			DAKGOGI.NDakgogiManager.Instance.CheckTheCart (imgCartPlus, lbItemCount);
			
			DAKGOGI.NDakgogiManager.Instance.InitOptions ();
			
			List<string> lstOptions = DAKGOGI.DishesManager.Instance.GetOptionsByIndex(DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex());
			if (lstOptions.Count >= 2)
			{
				btnPlaceAnOrder.defaultColor = new Color32(127, 127, 127, 255);
				btnPlaceAnOrder.state = UIButtonColor.State.Disabled;
				btnPlaceAnOrder.enabled = false;
			}
			
			PlayAddToCartAnimation ();
		} 
		else 
		{
			DAKGOGI.NDakgogiManager.Instance.PrePlaceAnOrder (newOption);
			DAKGOGI.NDakgogiManager.Instance.IncreaseOptionPageIndex();
			Application.LoadLevel ("Detail");
//			var slices = new VerticalSlicesTransition()
//			{
//				nextScene = 5,		// Detail
//				divisions = 1,
//				dirOffset = 1,
//			};
//			TransitionKit.instance.transitionWithDelegate( slices );
		}
	}

	public void ClickButton_CheckOut ()
	{
		if (false == DAKGOGI.NDakgogiManager.Instance.CheckTheCart (imgCartPlus, lbItemCount))
			return;
		
		DAKGOGI.NDakgogiManager.Instance.SetPreviousPage ("Main");
		Application.LoadLevel ("Order");
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

		lStartTime = System.DateTime.Now.Ticks;

		btnPlaceAnOrder.defaultColor = new Color32(127, 127, 127, 255);
		btnPlaceAnOrder.state = UIButtonColor.State.Disabled;
		btnPlaceAnOrder.enabled = false;
	}

	private static DateTime Delay(int MS)
	{
		DateTime ThisMoment = DateTime.Now;
		TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
		DateTime AfterWards = ThisMoment.Add(duration);
		while (AfterWards >= ThisMoment)
		{
			ThisMoment = DateTime.Now;
		}
		return DateTime.Now;
	}
}
