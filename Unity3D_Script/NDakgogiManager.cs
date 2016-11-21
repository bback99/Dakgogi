using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DAKGOGI
{
	class NDakgogiManager : Singleton<NDakgogiManager>
	{
		private SortedDictionary<int, List<OrderedDish>> dicOrder_;
		
		private string strSelectedDishCategory_;
		private string strSubCategory_;
		private int nSelectedDishIndex_;
		private int nSelectedOrderIndex_;
		private bool bDelivery_;
		private string strPreviousPage_;
		private Font fontCurrent_;
		private string currentLanguage_;
		private bool bIfSldingMenuIsFirst_;
		private int nOptionPageIndex_;
		private Option option_;
		private int nOrderIndex_;
		private double dDeliveryCharge_ = 4;
		private int nShortCutAmount_ = 70;

		public void OnInit() {

			strSelectedDishCategory_ = "";

			if (dicOrder_ != null)
				return;
			
			dicOrder_ = new SortedDictionary<int, List<OrderedDish>>();
			strSubCategory_ = "";
			nSelectedDishIndex_ = 0;
			nSelectedOrderIndex_ = 0;
			bDelivery_ = false;
			strPreviousPage_ = "";
			bIfSldingMenuIsFirst_ = false;
			nOptionPageIndex_ = 0;
			nOrderIndex_ = 0;
			nShortCutAmount_ = 70;
			
			DishesManager.Instance.SerializeFromData ();
		}

		public void SetShortCut(int amount)
		{
			nShortCutAmount_ = amount;
		}

		public int GetShortCut() {
			return nShortCutAmount_;
		}

		public void SetIsDelivery(bool delivery)
		{
			bDelivery_ = delivery;
		}

		public bool GetIsDelivery()
		{
			return bDelivery_;
		}

		public SortedDictionary<int, List<OrderedDish>> getListOfOredered()
		{
			return dicOrder_;
		}

		public void IncreaseOptionPageIndex()
		{
			++nOptionPageIndex_;
		}

		public void DecreaseOptionPageIndex()
		{
			if (--nOptionPageIndex_ <= 0)
				nOptionPageIndex_ = 0;
		}

		public int GetOptionPageIndex()
		{
			return nOptionPageIndex_;
		}

		public void InitOptions()
		{
			nOptionPageIndex_ = 0;
			strSubCategory_ = "";
		}

		public void InitChangingOptionVariables()
		{
			nSelectedOrderIndex_ = 0;
		}

		public void SetCurrentDishIndex(int nDishIndex)
		{
			nSelectedDishIndex_ = nDishIndex;
		}

		public void SetCurrentOrderedIndex(int nOrderIndex)
		{
			nSelectedOrderIndex_ = nOrderIndex;
		}

		public int GetCurrentOrderIndex()
		{
			return nSelectedOrderIndex_;
		}

		public int GetDishIndexByOrderIndex(int nSelectedOrderIndex)
		{
			foreach (KeyValuePair<int, List<OrderedDish>> order in dicOrder_)
			{
				foreach(OrderedDish dish in order.Value)
				{
					if (dish.getOrderIndex() == nSelectedOrderIndex)
					{
						return dish.getMainDish().getIndex();
					}
				}
			}
			return -1;
		}

		public int GetCurrentDishIndex()
		{
			return nSelectedDishIndex_;
		}

		public void SetCurrentFont(UIFont font)
		{
			if (font == null) {
				Debug.Log ("SetCurrentFont, The current font is null");
				return;
			}
			fontCurrent_ = font.dynamicFont;
		}

		public Font GetCurrentFont()
		{
			return fontCurrent_;
		}

		public void SetCurrentCategory(string strCategory)
		{
			strSelectedDishCategory_ = strCategory;
		}

		public string GetCurrentCategory()
		{
			return strSelectedDishCategory_;
		}

		public void SetSubCurrentCategory(string strSubCategory)
		{
			strSubCategory_ = strSubCategory;
		}

		public string GetSubCurrentCategory()
		{
			return strSubCategory_;
		}

		public string GetPreviousPage()
		{
			return strPreviousPage_;
		}

		public void SetPreviousPage(string strPreviousPage)
		{
			strPreviousPage_ = strPreviousPage;
		}

		public bool SetCurrentLanguage(string language)
		{
			if (currentLanguage_ == language)
				return false;
			currentLanguage_ = language;
			return true;
		}

		public string GetCurrentLanguage()
		{
			return currentLanguage_;
		}

		public bool IsSldingMenuFirst()
		{
			if (false == bIfSldingMenuIsFirst_) {
				bIfSldingMenuIsFirst_ = true;
				return true;
			}
			return false;
		}

		public bool ComparePreOption(Option op) {
			if (option_ == null)
				return false;

			return (option_.getIndex () == op.getIndex ());
		}

		public Option GetCurrentOption()
		{
			List<OrderedDish> lstTemp;
			if (true == dicOrder_.TryGetValue (nSelectedDishIndex_, out lstTemp)) 
			{
				foreach(OrderedDish dish in lstTemp)
				{
					if (nSelectedOrderIndex_ == dish.getOrderIndex())
				    {
						int nCount = 0;
						foreach(Option op in dish.getOptions())
						{
							if (nOptionPageIndex_ == nCount++)
								return op;
						}
						break;
					}
				}
			}

			return new Option ();
		}

		public void PrePlaceAnOrder(Option newOption)
		{
			option_ = newOption;
		}
		
		public void PlaceAnOrder(MainDish dish, Option newOption, int nAmount)
		{
			string optionType = null;
			List<Option> lstOption = new List<Option> ();
			if (newOption != null) {
				if (option_ != null)
					lstOption.Add(option_);
				lstOption.Add(newOption);
				optionType = newOption.getOptionType();
			}
		
			List<OrderedDish> lstTemp;
			if(true == dicOrder_.TryGetValue(dish.getIndex(), out lstTemp))
			{
				if (DAKGOGI.DishesManager.Instance.IfOptionsAreExistByIndex (DAKGOGI.NDakgogiManager.Instance.GetCurrentDishIndex ()))
				{
					if (nSelectedOrderIndex_ > 0)
					{
						foreach(OrderedDish op in lstTemp)
						{
							if (op.getOrderIndex() == nSelectedOrderIndex_)
							{
								op.RemoveAllOptions();

								foreach(Option newop in lstOption)
									op.AddOption(newop);
							}
						}
					}
					else
						lstTemp.Add (new DAKGOGI.OrderedDish (++nOrderIndex_, dish, lstOption, optionType, nAmount));

					dicOrder_[dish.getIndex()] = lstTemp;
				}
				else
				{
					foreach(OrderedDish order in lstTemp)
					{
						order.AddUpAmount(nAmount);
					}
				}
			}
			else
			{
				List<OrderedDish> newList = new List<OrderedDish>();
				newList.Add (new DAKGOGI.OrderedDish (++nOrderIndex_, dish, lstOption, optionType, nAmount));
				dicOrder_.Add (dish.getIndex(), newList);
			}

			option_ = null;

			/*Debug.Log ("........... For Debugging start................");

			// for debugging
			foreach (KeyValuePair<int, List<OrderedDish>> order in dicOrder_)
			{
				Debug.Log ("DishIndex : " + order.Key);

				foreach(DAKGOGI.OrderedDish orderedDish in order.Value)
					orderedDish.Output();
			}*/
		}
		
		public bool CheckTheCart(UISprite cart, UILabel itemCount)
		{
			// for showing CheckOut Items
			if (dicOrder_.Count == null || dicOrder_.Count <= 0) {
				cart.gameObject.SetActive(true);
				itemCount.gameObject.SetActive(false);
				return false;
				
			} else {
				cart.gameObject.SetActive(false);
				itemCount.gameObject.SetActive(true);
				itemCount.text = GetOrderedTotalCount().ToString();
			}
			return true;
		}

		public int GetOrderedTotalCount()
		{
			int nCount = 0;
			foreach (KeyValuePair<int, List<OrderedDish>> order in dicOrder_)
				nCount += order.Value.Count;
			return nCount;
		}

		public void IncreaseAmount(int nOrderIndex)
		{
			foreach (KeyValuePair<int, List<OrderedDish>> order in dicOrder_) 
			{
				foreach(OrderedDish dish in order.Value)
				{
					if (dish.getOrderIndex() == nOrderIndex)
					{
						dish.AddUpAmount(1);
					}
				}
			}
		}

		public bool DecreaseAmount(int nOrderIndex)
		{
			foreach (KeyValuePair<int, List<OrderedDish>> order in dicOrder_) 
			{
				foreach(OrderedDish dish in order.Value)
				{
					if (dish.getOrderIndex() == nOrderIndex)
					{
						return dish.DecreaseAmount();
					}
				}
			}

			return false;
		}

		public void RemoveDishByOrderIndex(int nOrderIndex)
		{
			int removedKey = -1;
			foreach (KeyValuePair<int, List<OrderedDish>> order in dicOrder_) 
			{
				OrderedDish removed = null;
				foreach(OrderedDish dish in order.Value)
				{
					if (dish.getOrderIndex() == nOrderIndex)
					{
						removed = dish;
					}
				}

				if (removed != null) 
				{
					order.Value.Remove(removed);

					if (order.Value.Count <= 0)
						removedKey = order.Key;
				}
			}

			if (removedKey != -1)
				dicOrder_.Remove (removedKey);
		}

		public double GetDeliveryCharge()
		{
			return dDeliveryCharge_;
		}

		public double CalculateTax(double subTotalPrice)
		{
			return Math.Round (subTotalPrice * 0.13, 2);
		}

		public double CalculateTotalPrice(double subTotalPrice)
		{
			return Math.Round (subTotalPrice + (subTotalPrice * 0.13) + dDeliveryCharge_);
		}
	}
}
