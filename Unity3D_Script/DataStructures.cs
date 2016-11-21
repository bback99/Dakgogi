using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DAKGOGI_DEFINES
{
	public enum MAIN_CATEGORY { CHICKEN=0, DISHES, FRIES, RICECAKE, SOUP, DRINKS };
	public enum SPICY_LEVEL { SLIGHTLY=0, MEDIUM, VERY };
}

namespace DAKGOGI
{
	class IFood 
	{
		private int nIndex_;
		private double dPrice_;
		private string strKorName_;
		private string strEngName_;
		private string strChnName_;

		public virtual void OnInit (int index, string kor, string eng, string chn, double price)
		{
			nIndex_ = index;
			dPrice_ = price;
			strKorName_ = kor;
			strEngName_ = eng;
			strChnName_ = chn;
		}

		public int getIndex() {
			return nIndex_;
		}

		public double getPrice() {
			return dPrice_;
		}

		public string getKorName() {
			return strKorName_;
		}

		public string getEngName() {
			return strEngName_;
		}

		public string getChnName() {
			return strChnName_;
		}

		public object ShallowCopy()
		{
			return this.MemberwiseClone ();
		}
	}

	class Option : IFood
	{
		private string strOptionType_;

		public void OnInit(int index, string kor, string eng, string chn, double price)
		{
			base.OnInit (index, kor, eng, chn, price);
		}

		//public string getCategory() { return strCategory_; }

		public void setOptionType(string type)
		{
			strOptionType_ = type;
		}

		public string getOptionType() { return strOptionType_; }

		public Option()
		{
			base.OnInit (0, "", "", "", 0);
		}
	}

	class MainDish : IFood
	{
		private DAKGOGI_DEFINES.MAIN_CATEGORY main_category_;
		private string sub_category_;
		private string strExplanation_;
		private string strImageName_;
		private bool bSpread_;

		public void OnInit(int index, string kor, string eng, string chn, string explain, string img, string category, string subcategory, double price)
		{
			base.OnInit (index, kor, eng, chn, price);
			main_category_ = (DAKGOGI_DEFINES.MAIN_CATEGORY)DishesManager.Instance.GetMainCategory(category);
			sub_category_ = subcategory;
			strExplanation_ = explain;
			strImageName_ = img;
			bSpread_ = false;
		}

		public void SetSpread(bool bSpread) { bSpread_ = bSpread; }
		public bool getSpread() { return bSpread_; }
		public int getCategory() { return (int)main_category_; }
		public string getSubCategory() { return sub_category_; }
		public string getImageName() { return strImageName_; }
		public string getExplanation() { return strExplanation_; }

		public object ShallowCopy()
		{
			base.ShallowCopy ();
			return this.MemberwiseClone ();
		}
	}
	
	class OrderedDish
	{
		private int nOrderIndex_;
		private MainDish mainDish_;
		private List<Option> lstOptions_;		// for halfNhalf or Special
		private string strOptionType_;
		private int nOrderAmount_;

		public List<Option> getOptions() {
			return lstOptions_;
		}

		public MainDish getMainDish() { return mainDish_; }
		public int getOrderAmount() { return nOrderAmount_; }
		public string getOptionType() { return strOptionType_; }
		public int getOrderIndex() { return nOrderIndex_; }

		public void RemoveAllOptions()
		{
			lstOptions_.Clear ();
		}

		public OrderedDish(int nOrderIndex, MainDish dish, List<Option> options, string optionType, int nAmount)
		{
			nOrderIndex_ = nOrderIndex;
			mainDish_ = dish;
			lstOptions_ = options;
			strOptionType_ = optionType;
			nOrderAmount_ = nAmount;
		}

		public void AddOption(Option newOption)
		{
			lstOptions_.Add (newOption);
		}

		public void AddUpAmount(int nAmount) { nOrderAmount_ += nAmount; }
		public bool DecreaseAmount()
		{
			if (--nOrderAmount_ <= 0) {
				nOrderAmount_ = 1;
				return true;
			}
			return false;
		}

		public void Output()
		{
			if (lstOptions_ == null)
				Debug.Log ("ListOptions is null");

			if (lstOptions_ != null && lstOptions_.Count > 0)
			{
				foreach(Option op in lstOptions_)
				{
					Debug.Log ("OptionIndex : " + op.getIndex());
				}
			}

			if (nOrderAmount_ > 0)
				Debug.Log ("Amount : " + nOrderAmount_);
		}
	}
}