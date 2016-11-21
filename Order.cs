using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Order : MonoBehaviour {

	private UIPanel pnOrder;
	private UILabel lbMenuName, lbTotalPrice, lbTotalTitle, lbSubTotalPrice, lbDeliveryPrice, lbHSTPrice, lbTipPrice, lbSummaryTotalPrice;
	private GameObject sv_ordered;
	private GameObject popup, popupWindow, popup2, removeOrderItemWindow;
	private int nSelectedOrderedItemIndex, whetherRemoveIndex;
	private double dSubTotalPrice = 0.00, dTaxPrice = 0.00;
	private int nSelectedOrderIndex = 0;

	void Awake() 
	{
		pnOrder = GameObject.Find ("Order").GetComponent<UIPanel> ();
		lbMenuName = GameObject.Find ("LB_MENU_NAME").GetComponent<UILabel> ();
		lbTotalPrice = GameObject.Find ("TotalPrice").GetComponent<UILabel> ();
		lbTotalTitle = GameObject.Find ("TotalTitle").GetComponent<UILabel> ();
		sv_ordered = GameObject.Find ("SV_ORDERLIST");
	}

	// Use this for initialization
	void Start () {

		//
		DAKGOGI.NDakgogiManager.Instance.InitChangingOptionVariables ();

		lbMenuName.text = "TOP";

		// Change to Previous Page
		UIButton btnBack = GameObject.Find ("BackTitle").GetComponent<UIButton>();
		btnBack.onClick.Add (new EventDelegate (ClickButton_PreviousPage));

		//
		UIPanel pnPay = GameObject.Find ("Total").GetComponent<UIPanel> ();
		UIButton[] btns = pnPay.GetComponentsInChildren<UIButton>();
		foreach (UIButton bt in btns) {
			if (bt.name == "Btn")
				bt.onClick.Add (new EventDelegate (ClickButton_Payment));
		}

		ClickButton_FoldMenu();
	}
	
	// Update is called once per frame
	void Update () {

		//충돌이 감지된 영역
		RaycastHit hit;
		
		GameObject target = null;
		
		if (true == Input.GetMouseButtonDown (0)) {
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100)) {

				target = hit.collider.gameObject;

				if (target != null && target.tag == "Change_Buttons")
					Int32.TryParse(target.name, out nSelectedOrderIndex);
			}
		}
	}

	public void ClickButton_PreviousPage() {

		Application.LoadLevel ("Main");
		//ClickButton_FoldMenu ();
	}

	public void ClickButton_Payment() {

		//DAKGOGI.NDakgogiManager.Instance.getListOfOredered ().Clear();
		//ClickButton_FoldMenu ();

		if (DAKGOGI.NDakgogiManager.Instance.GetOrderedTotalCount() <= 0)
			return;

		// Add Popup Windows
		GameObject parent = GameObject.Find ("Order");

		if (popupWindow) {
			popupWindow.SetActive(true);
			return;
		}
		
		popup = Resources.Load ("Prefabs/PopPickup") as GameObject;
		if (popup == null) {
			Debug.Log ("Failed loading Prefabs/PopPickup!");
			return;
		}

		popup.name = "Popup";
		popupWindow = NGUITools.AddChild (parent, popup);
		if (popupWindow) {

			UILabel[] labels = popupWindow.GetComponentsInChildren<UILabel> ();
			foreach (UILabel label in labels) {
				label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
			}

			UIButton button_delivery = GameObject.Find ("BTN_DELIVERY").GetComponent<UIButton> ();
			if (button_delivery != null) {
				button_delivery.onClick.Add (new EventDelegate (ClickButton_Delivery));
			}

			UIButton button_pickup = GameObject.Find ("BTN_PICKUP").GetComponent<UIButton> ();
			if (button_pickup != null) {
				button_pickup.onClick.Add (new EventDelegate (ClickButton_Pickup));
			}
		}
	}

	public void ClickButton_Delivery() {

		DAKGOGI.NDakgogiManager.Instance.SetIsDelivery(true);
		popupWindow.SetActive (false);
		Application.LoadLevel ("Delivery");
	}

	public void ClickButton_Pickup() {

		DAKGOGI.NDakgogiManager.Instance.SetIsDelivery(false);
		popupWindow.SetActive (false);
		Application.LoadLevel ("Delivery");
	}

	public void PutIntoTop(string strResourcesName, int height, EventDelegate buttonEvent)
	{
		GameObject top = Resources.Load (strResourcesName) as GameObject;
		GameObject topPanel = NGUITools.AddChild (sv_ordered, top);
		if (topPanel == null) {
			Debug.Log ("Failed loading topPanel!");
			return;
		}
		
		UIButton btnTopPanel = topPanel.GetComponentInChildren<UIButton> ();
		btnTopPanel.onClick.Add (buttonEvent);

		topPanel.transform.localPosition = new Vector3 (0, height);
	}

	public void PutIntoSummary(int height)
	{
		GameObject summary = Resources.Load ("Prefabs/Summary") as GameObject;
		GameObject summaryPanel = NGUITools.AddChild (sv_ordered, summary);
		if (summaryPanel == null) {
			Debug.Log ("Failed loading Summary!");
			return;
		}

		summaryPanel.transform.localPosition = new Vector3 (0, height);
	}

	public void ClickButton_FoldMenu()
	{
		ClearScrollView ();
		
		int nHeight = 690, nAmountOfHeight = 0;
		double dSubTotalPrice = 0.00;

		PutIntoTop ("Prefabs/TopInOrder_Fold", nHeight, new EventDelegate(ClickButton_SpreadMenu));
		
		// Body
		nHeight = 550;
		SortedDictionary<int, List<DAKGOGI.OrderedDish>> lstOrdered = DAKGOGI.NDakgogiManager.Instance.getListOfOredered ();
		
		// Recreate or Create All Items in ScrollView
		foreach (KeyValuePair<int, List<DAKGOGI.OrderedDish>> ordered in lstOrdered) 
		{
			foreach (DAKGOGI.OrderedDish dish in ordered.Value) 
			{
				GameObject menu;
				if (dish.getOptions ().Count == 1) {
					menu = Resources.Load ("Prefabs/OrderSummaryItemFold_1") as GameObject;
					nAmountOfHeight = 170;
				} else if (dish.getOptions ().Count == 2) {
					menu = Resources.Load ("Prefabs/OrderSummaryItemFold_2") as GameObject;
					nAmountOfHeight = 200;
				} else {
					menu = Resources.Load ("Prefabs/OrderSummaryItemFold_0") as GameObject;
					nAmountOfHeight = 170;
				}
				
				if (menu == null) {
					Debug.Log ("Failed loading Prefabs/OrderSummaryItemFold_1 or OrderSummaryItemFold_2 or OrderSummaryItemFold_0!");
					return;
				}
				
				GameObject menuPanel = NGUITools.AddChild (sv_ordered, menu);
				if (menuPanel == null) {
					Debug.Log ("Failed loading panel!");
					return;
				}
				
				menuPanel.name = dish.getOrderIndex().ToString();
				menuPanel.SetActive (true);
				menuPanel.transform.localPosition = new Vector3 (0, nHeight);
				
				nHeight -= nAmountOfHeight;
				
				UILabel[] lables = menuPanel.GetComponentsInChildren <UILabel> ();
				if (lables.Length == 0) {
					Debug.Log ("Failed loading Caption!");
					return;
				}
				
				foreach (UILabel label in lables)
				{					
					if (label.name == "OrderItemName") 
					{
						label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
						if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
							label.text = dish.getMainDish().getKorName();
						else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
							label.text = dish.getMainDish().getEngName();
						else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
							label.text = dish.getMainDish().getChnName();
					}
					else if (label.name == "OrderItemPrice")
					{
						int nAmount = dish.getOrderAmount();
						if (nAmount <= 0)
							nAmount = 1;
						
						if (dish.getOptionType() != "SUB")
						{
							dSubTotalPrice += dish.getMainDish().getPrice() * nAmount;
							label.text = "$ " + (dish.getMainDish().getPrice() * nAmount).ToString();
						}
					}
					else if (label.name == "OrderItemQty")
					{
						label.text = dish.getOrderAmount().ToString();
					}
					else if (label.name == "OrderItemOption01") 
					{
						label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 1)
							{
								if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
									label.text = "Option1 : " + op.getKorName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
									label.text = "Option1 : " + op.getEngName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
									label.text = "Option1 : " + op.getChnName();
								break;
							}
						}
					}
					else if (label.name == "OrderItemOption01Price") {
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 1)
							{
								if (op.getPrice() > 0)
								{
									dSubTotalPrice += op.getPrice();
									if (op.getOptionType() == "ADD")
									{
										label.text += " + ";
										label.text += "$ " + op.getPrice().ToString();
									}
									else 
										label.text += "$ " + op.getPrice().ToString();
								}
								else if (op.getPrice() < 0)
								{
									dSubTotalPrice += op.getPrice();
									if (op.getOptionType() == "ADD")
										label.text += " - ";
									label.text += "$ " + (op.getPrice() * -1 ).ToString();
								}
								break;
							}
						}
					}
					else if (label.name == "OrderItemOption02") 
					{
						label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 2)
							{
								if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
									label.text = "Option2 : " + op.getKorName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
									label.text = "Option2 : " + op.getEngName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
									label.text = "Option2 : " + op.getChnName();
								break;
							}
						}
					}
					else if (label.name == "OrderItemOption02Price") {
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 2)
							{
								if (op.getPrice() > 0) {
									dSubTotalPrice += op.getPrice();
									if (op.getOptionType() == "ADD")
										label.text += " + ";
									label.text += "$ " + op.getPrice().ToString();
								}
								break;
							}
						}
					}
				}
			}
		}
		
		PutIntoSummary (nHeight - 30);
		SetTotalPrice (dSubTotalPrice);
	}

	public void ClearScrollView()
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
	}

	public void ClickButton_SpreadMenu()
	{
		ClearScrollView ();
		
		int nHeight = 690, nAmountOfHeight = 0;
		double dSubTotalPrice = 0.00;

		PutIntoTop ("Prefabs/TopInOrder_Spread", nHeight, new EventDelegate(ClickButton_FoldMenu));

		// Body
		nHeight = 485;
		SortedDictionary<int, List<DAKGOGI.OrderedDish>> lstOrdered = DAKGOGI.NDakgogiManager.Instance.getListOfOredered ();
		
		// Recreate or Create All Items in ScrollView
		foreach (KeyValuePair<int, List<DAKGOGI.OrderedDish>> ordered in lstOrdered) 
		{
			foreach (DAKGOGI.OrderedDish dish in ordered.Value) 
			{
				GameObject menu;
				if (dish.getOptions ().Count >= 1)
					menu = Resources.Load ("Prefabs/OrderSummaryItemSpread_1") as GameObject;
				else
					menu = Resources.Load ("Prefabs/OrderSummaryItemSpread_0") as GameObject;

				nAmountOfHeight = 430;
				
				if (menu == null) {
					Debug.Log ("Failed loading Prefabs/OrderSummaryItemSpread_1 or OrderSummaryItemSpread_2 or OrderSummaryItemSpread_0!");
					return;
				}
				
				GameObject menuPanel = NGUITools.AddChild (sv_ordered, menu);
				if (menuPanel == null) {
					Debug.Log ("Failed loading panel!");
					return;
				}

				menuPanel.name = dish.getOrderIndex().ToString();
				menuPanel.SetActive (true);
				menuPanel.transform.localPosition = new Vector3 (0, nHeight);
				
				nHeight -= nAmountOfHeight;

				UIButton[] buttons = menuPanel.GetComponentsInChildren<UIButton>();
				foreach(UIButton button in buttons)
				{
					if (button.name == "ChangeBtn")
						button.onClick.Add(new EventDelegate(ClickButton_ChangeOptions));
					else if (button.name == "ItemDeleteBtn")
						button.onClick.Add(new EventDelegate(ClickButton_DeleteMenu));
					else if (button.name == "ItemPlusBtn")
						button.onClick.Add (new EventDelegate(ClickButton_IncreaseQTY));
					else if (button.name == "ItemMinusBtn")
						button.onClick.Add (new EventDelegate(ClickButton_DecreaseQTY));

					button.name = dish.getOrderIndex().ToString();
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
						sprite.spriteName = "thum_list_" + dish.getMainDish().getImageName();
					}
				}
				
				UILabel[] lables = menuPanel.GetComponentsInChildren <UILabel> ();
				if (lables.Length == 0) {
					Debug.Log ("Failed loading Caption!");
					return;
				}

				foreach (UILabel label in lables)
				{
					if (label.name == "LabelItemTitle")
					{
						label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();

						if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
							label.text = dish.getMainDish().getKorName();
						else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
							label.text = dish.getMainDish().getEngName();
						else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
							label.text = dish.getMainDish().getChnName();
					} 
					else if (label.name == "LabelItemExplanation") {
						label.text = dish.getMainDish().getExplanation();
					} else if (label.name == "LabelItemPrice")
					{
						int nAmount = dish.getOrderAmount();
						if (nAmount <= 0)
							nAmount = 1;
						
						if (dish.getOptionType() != "SUB")
						{
							dSubTotalPrice += dish.getMainDish().getPrice() * nAmount;
							label.text = "$ " + dish.getMainDish().getPrice().ToString();
						}
						else
							label.text = "";
					} 
					else if (label.name == "LabelItemQty") {
						label.text = dish.getOrderAmount().ToString();
					} 
					else if (label.name == "LabelItemTotalPrice") {
						label.text = "Total : $ " + (dish.getOrderAmount() * dish.getMainDish().getPrice()).ToString();
					} else if (label.name == "LabelItemOption01") 
					{
						label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 1)
							{
								if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
									label.text = "Option1 : " + op.getKorName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
									label.text = "Option1 : " + op.getEngName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
									label.text = "Option1 : " + op.getChnName();
								break;
							}
						}
					} 
					else if (label.name == "LabelItemOption01Price") {
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 1)
							{
								if (op.getPrice() > 0)
								{
									dSubTotalPrice += op.getPrice();
									if (op.getOptionType() == "ADD")
									{
										label.text += " + ";
										label.text += "$ " + op.getPrice().ToString();
									}
									else 
										label.text += "$ " + op.getPrice().ToString();
								}
								else if (op.getPrice() < 0)
								{
									dSubTotalPrice += op.getPrice();
									if (op.getOptionType() == "ADD")
										label.text += " - ";
									label.text += "$ " + (op.getPrice() * -1 ).ToString();
								}
								break;
							}
						}
					} else if (label.name == "LabelItemOption02") 
					{
						label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 2)
							{
								if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Korean")
									label.text = "Option2 : " + op.getKorName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "English")
									label.text = "Option2 : " + op.getEngName();
								else if (DAKGOGI.NDakgogiManager.Instance.GetCurrentLanguage() == "Chinese")
									label.text = "Option2 : " + op.getChnName();
								break;
							}
						}
					} 
					else if (label.name == "LabelItemOption02Price") {
						List<DAKGOGI.Option> lstOption = dish.getOptions();
						int nCount = 0;
						foreach(DAKGOGI.Option op in lstOption)
						{
							if (++nCount == 2)
							{
								if (op.getPrice() > 0) {
									dSubTotalPrice += op.getPrice();
									if (op.getOptionType() == "ADD")
										label.text += " + ";
									label.text += "$ " + op.getPrice().ToString();
								}
								break;
							}
						}
					} 
				}
			}
		}
		
		PutIntoSummary (nHeight + 40);
		
		SetTotalPrice (dSubTotalPrice);
	}

	public void SetTotalPrice(double subTotalPrice)
	{
		lbSubTotalPrice = GameObject.Find ("SubTotalPrice").GetComponent<UILabel> ();
		lbDeliveryPrice = GameObject.Find ("DeliveryPrice").GetComponent<UILabel> ();
		lbHSTPrice = GameObject.Find ("HSTPrice").GetComponent<UILabel> ();
		lbTipPrice = GameObject.Find ("TipPrice").GetComponent<UILabel> ();
		lbSummaryTotalPrice = GameObject.Find ("SummaryTotalPrice").GetComponent<UILabel> ();

		lbSubTotalPrice.text = "$ " + String.Format ("{0:0.00}", Math.Round(subTotalPrice, 2));
		lbDeliveryPrice.text = "$ " + String.Format ("{0:0.00}", Math.Round(DAKGOGI.NDakgogiManager.Instance.GetDeliveryCharge(), 2));
		dTaxPrice = DAKGOGI.NDakgogiManager.Instance.CalculateTax (subTotalPrice);
		lbHSTPrice.text = "$ " + String.Format ("{0:0.00}", dTaxPrice);
		//dTipPrice = Math.Round (dTipPrice, 2);
		lbTipPrice.text = "$ " + String.Format("{0:0.00}", 0);

		lbTotalTitle.text = "TOTAL :  " + DAKGOGI.NDakgogiManager.Instance.GetOrderedTotalCount() + "  ITEMS";
		lbTotalPrice.text = "$ " + String.Format("{0:0.00}", DAKGOGI.NDakgogiManager.Instance.CalculateTotalPrice(subTotalPrice));
		lbSummaryTotalPrice.text = "$ " + String.Format("{0:0.00}", DAKGOGI.NDakgogiManager.Instance.CalculateTotalPrice(subTotalPrice));
	}

	public void ClickButton_Yes()
	{
		removeOrderItemWindow.SetActive (false);

		// To get rid of this item from ordered list
		DAKGOGI.NDakgogiManager.Instance.RemoveDishByOrderIndex(nSelectedOrderIndex);

		if (DAKGOGI.NDakgogiManager.Instance.GetOrderedTotalCount () > 0)		// empty orderlist
			ClickButton_SpreadMenu ();
		else
			Application.LoadLevelAsync ("Main");
	}

	public void ClickButton_No()
	{
		removeOrderItemWindow.SetActive (false);
	}

	public void ClickButton_ChangeOptions()
	{
		int nDishIndex = DAKGOGI.NDakgogiManager.Instance.GetDishIndexByOrderIndex (nSelectedOrderIndex);
		if (nDishIndex < 0)
			return;
		DAKGOGI.NDakgogiManager.Instance.SetCurrentDishIndex (nDishIndex);
		DAKGOGI.NDakgogiManager.Instance.SetCurrentOrderedIndex (nSelectedOrderIndex);
		Application.LoadLevelAsync ("Detail");
	}

	public void ClickButton_DeleteMenu()
	{
		// Remove this dish
		// Add Popup Windows
		GameObject parent = GameObject.Find ("Order");
		
		if (removeOrderItemWindow) {
			removeOrderItemWindow.SetActive (true);
			return;
		}
		
		popup2 = Resources.Load ("Prefabs/RemoveOrderItem") as GameObject;
		if (popup2 == null) {
			Debug.Log ("Failed loading Prefabs/RemoveOrderItem!");
			return;
		}
		
		popup2.name = "RemoveOrderItem";
		removeOrderItemWindow = NGUITools.AddChild (parent, popup2);
		if (removeOrderItemWindow) {

			UIButton button_yes = GameObject.Find ("BTN_YES").GetComponent<UIButton> ();
			if (button_yes != null) {
				button_yes.onClick.Add (new EventDelegate (ClickButton_Yes));
			}
			
			UIButton button_no = GameObject.Find ("BTN_NO").GetComponent<UIButton> ();
			if (button_no != null) {
				button_no.onClick.Add (new EventDelegate (ClickButton_No));
			}
		}
	}

	public void ClickButton_IncreaseQTY()
	{
		DAKGOGI.NDakgogiManager.Instance.IncreaseAmount (nSelectedOrderIndex);

		ClickButton_SpreadMenu ();
	}

	public void ClickButton_DecreaseQTY()
	{
		if (true == DAKGOGI.NDakgogiManager.Instance.DecreaseAmount (nSelectedOrderIndex))
		{
			// Remove this dish
			// Add Popup Windows
			GameObject parent = GameObject.Find ("Order");
			
			if (removeOrderItemWindow) {
				removeOrderItemWindow.SetActive (true);
				return;
			}
			
			popup2 = Resources.Load ("Prefabs/RemoveOrderItem") as GameObject;
			if (popup2 == null) {
				Debug.Log ("Failed loading Prefabs/RemoveOrderItem!");
				return;
			}
			
			popup2.name = "RemoveOrderItem";
			removeOrderItemWindow = NGUITools.AddChild (parent, popup2);
			if (removeOrderItemWindow) {
				
				UILabel[] labels = removeOrderItemWindow.GetComponentsInChildren<UILabel> ();
				foreach (UILabel label in labels) {
					label.trueTypeFont = DAKGOGI.NDakgogiManager.Instance.GetCurrentFont();
				}
				
				UIButton button_yes = GameObject.Find ("BTN_YES").GetComponent<UIButton> ();
				if (button_yes != null) {
					button_yes.onClick.Add (new EventDelegate (ClickButton_Yes));
				}
				
				UIButton button_no = GameObject.Find ("BTN_NO").GetComponent<UIButton> ();
				if (button_no != null) {
					button_no.onClick.Add (new EventDelegate (ClickButton_No));
				}
			}
		}

		ClickButton_SpreadMenu ();
	}
}