using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DAKGOGI
{
	class DishesManager : Singleton<DishesManager>
	{
		private string[] strMainCategory = { "CHICKEN", "DISHES", "FRIES", "RICE CAKE", "SOUP", "DRINKS" };
		
		private SortedDictionary<string, List<MainDish>> dicDishesByCategory_;
		private SortedDictionary<string, SortedDictionary<string, string>> dicLocalization_;
		private SortedDictionary<int, MainDish> dicDishesByIndex_;
		private SortedDictionary<int, List<string>> dicOptions_;
		private SortedDictionary<string, List<Option>> dicOptionInformation_;
		private const int nProductCount = 24;
		private const int nROWCount = 9;
		private const int nCountOptionInformation = 23;		// OptionInformation
		private const int nCountMenuOption = 11;			// MenuOption

		
		public int GetMainCategory(string category)
		{
			for (int i=0; i<strMainCategory.Length; i++) {
				if (strMainCategory[i] == category)
					return i;
			}
			return -1;
		}

		public string GetMainCategory(int nIndex)
		{
			if (nIndex < 0 || nIndex >= strMainCategory.Length)
				return "";

			return strMainCategory [nIndex];
		}

		public bool SerializeFromData()
		{
			if (dicDishesByCategory_ != null)
				return true;
			
			// Dishes
			dicDishesByCategory_ = new SortedDictionary<string, List<MainDish>> ();
			dicDishesByIndex_ = new SortedDictionary<int, MainDish> ();

			if (null == dicDishesByCategory_ || null == dicDishesByIndex_)
				return false;
			
			for (int i=0; i<nProductCount; i++) 
			{
				MainDish newDish = new MainDish();
				{
					int index;
					double price;
					System.Int32.TryParse (aryDishData[i, 0], out index);
					System.Double.TryParse (aryDishData[i, 8], out price);

					newDish.OnInit(index, aryDishData[i, 1], aryDishData[i, 2], aryDishData[i, 3], aryDishData[i, 4], aryDishData[i, 5], aryDishData[i, 6], aryDishData[i, 7], price);
				}
				
				List<MainDish>	lstTemp;
				if (true == dicDishesByCategory_.TryGetValue(aryDishData[i, 6], out lstTemp))
					lstTemp.Add (newDish);
				else {
					List<MainDish> newList = new List<MainDish>();
					newList.Add (newDish);
					dicDishesByCategory_.Add (aryDishData[i, 6], newList);
				}

				dicDishesByIndex_.Add (newDish.getIndex(), newDish);
			}

			// Option Information
			dicOptionInformation_ = new SortedDictionary<string, List<Option>> ();
			for (int i=0; i<nCountOptionInformation; i++) {

				Option newFood = new Option();
				{	
					int index = 0;
					double price;
					System.Int32.TryParse (aryOptionInformation [i, 0], out index);
					System.Double.TryParse (aryOptionInformation[i, 6], out price);
					newFood.OnInit(index, aryOptionInformation [i, 3], aryOptionInformation [i, 4], aryOptionInformation [i, 5], price);
					newFood.setOptionType(aryOptionInformation[i, 2]);
				}

				List<Option> lstTemp;
				if (true == dicOptionInformation_.TryGetValue(aryOptionInformation [i, 1], out lstTemp))
					lstTemp.Add ((Option)newFood);
				else{
					List<Option> newList = new List<Option>();
					newList.Add ((Option)newFood);
					dicOptionInformation_.Add (aryOptionInformation [i, 1], newList);
				}
			}

			// Options
			dicOptions_ = new SortedDictionary<int, List<string>> ();
			
			for(int i=0; i<nCountMenuOption; i++)
			{
				int j = 0, nIndex = 0;
				System.Int32.TryParse (aryMenuNOption [i, j], out nIndex);
				
				List<string> lstTemp;
				if (true == dicOptions_.TryGetValue(nIndex, out lstTemp))
					lstTemp.Add (aryMenuNOption [i, j+1]);
				else{
					List<string> newList = new List<string>();
					newList.Add (aryMenuNOption [i, j+1]);
					dicOptions_.Add (nIndex, newList);
				}
			}
			
			// Localization
			string [] languages = new string[4];
			dicLocalization_ = new SortedDictionary<string, SortedDictionary<string, string>>();
			
			for (int i=0; i<3; i++) 
			{
				for(int j=1; j<4; j++)
				{
					if ("Language" == aryLocalization[i, 0])
					{
						languages[j] = aryLocalization[i, j];		// to get language's index
						SortedDictionary<string, string> newLanguage = new SortedDictionary<string, string>();
						dicLocalization_.Add (aryLocalization[i, j], newLanguage);
					}
					else
					{
						SortedDictionary<string, string> dicTemp;
						if (true == dicLocalization_.TryGetValue(languages[j], out dicTemp))
						{
							string key = aryLocalization[i, 0];	
							string value = aryLocalization[i, j];
							
							dicTemp.Add (key, value);
						}
					}
				}
			}
			
			Debug.Log ("Loading Data... count :" + dicDishesByCategory_.Count);
			return true;
		}
		
		public string GetLocalizationByKey(string language, string key)
		{
			string value = "";
			SortedDictionary<string, string>	dicTemp;
			if (true == dicLocalization_.TryGetValue (language, out dicTemp)) 
			{
				
				dicTemp.TryGetValue(key, out value);
			}
			return value;
		}

		public void GetMainDishesByCategory(string category, out List<MainDish> lstDishes)
		{
			List<MainDish> lstFoods;
			lstDishes = new List<MainDish> ();

			if (NDakgogiManager.Instance.GetSubCurrentCategory () != "") {

				foreach(KeyValuePair<int, MainDish> food in dicDishesByIndex_)
				{
					if (food.Value.getSubCategory() == NDakgogiManager.Instance.GetSubCurrentCategory())
						if (food.Value.getCategory() == -1)
						lstDishes.Add ((MainDish)food.Value.ShallowCopy ());
				}

			} else {

				if (!dicDishesByCategory_.TryGetValue (category, out lstFoods)) {
					Debug.Log ("GetProductExByCategory, Couldn't find the key " + category);
					return;
				}

				foreach (MainDish food in lstFoods)
					lstDishes.Add ((MainDish)food.ShallowCopy ());
			}
		}

		public MainDish GetDishInformationByIndex(int nDishIndex)
		{
			return (MainDish)dicDishesByIndex_[nDishIndex];
		}

		public bool IfOptionsAreExistByIndex(int nDishIndex)
		{
			List<string> lstOptions = new List<string> ();
			return dicOptions_.TryGetValue(nDishIndex, out lstOptions);
		}

		public List<string> GetOptionsByIndex(int nDishIndex)
		{
			List<string> lstOptions = new List<string> ();
			dicOptions_.TryGetValue(nDishIndex, out lstOptions);
			return lstOptions;
		}

		public string GetOptionCategoryByIndex(int nDishIndex, int nOptionPageIndex, out int nOptionCNT)
		{
			List<string> lstOptions = GetOptionsByIndex (nDishIndex);
			nOptionCNT = lstOptions.Count;

			int nCNT = 0;
			foreach (string option in lstOptions) 
			{
				if (nCNT++ == nOptionPageIndex)
					return option;
			}
			return null;
		}

		public string GetOptionType(int nDishIndex)
		{
			string strOptionIndex = "";
			List<string> lstMenuNOption = GetOptionsByIndex (nDishIndex);
			if (lstMenuNOption == null)
				return "";

			foreach (string op in lstMenuNOption) {
				strOptionIndex = op;
				break;
			}

			string strOptionType = "";
			List<Option> lstOption = GetOptionInformations (strOptionIndex);
			if (lstOption == null)
				return "";

			foreach (Option op in lstOption) {
				strOptionType = op.getOptionType();
				break;
			}
			return strOptionType;
		}

		public Option GetOptionInformation(string category, string strIndex)
		{
			List<Option> lstOption = GetOptionInformations (category);
			foreach (Option op in lstOption) 
			{
				if (op.getIndex().ToString() == strIndex)
					return op;
			}
			return null;
		}

		public List<Option> GetOptionInformations(string category)
		{
			List<Option> lstOptions = new List<Option> ();
			dicOptionInformation_.TryGetValue(category, out lstOptions);
			return lstOptions;
		}

		public bool NextOptionPage(int nCurrentItemIndex)
		{
			List<string> lstOptions = GetOptionsByIndex (nCurrentItemIndex);
			if (lstOptions == null || (lstOptions.Count-1) <= NDakgogiManager.Instance.GetOptionPageIndex ()) 
			{
				return false;
			}

			return true;
		}
		
		private string[ , ] aryLocalization = {
			{"Language", "English", "Korean", "Chinese"},
			{"DeliveryZone", "DELIVERY ZONE", "배달 가능지역", "送货范围"},
			{"DirectCall", "DIRECT CALL", "바로 전화", "直接呼叫"},
		};
		
		private string[ , ] aryDishData = {
			{"1001", "치킨", "Chicken", "炸鸡", "", "ch01", "CHICKEN", "CHICKEN", "25.99"},
			{"1002", "후라이드", "Fried Chicken", "原味 炸鸡", "Deep fried chicken with Korean style speciall batter.", "ch02", "", "CHICKEN", "25.99"},
			{"1003", "양념 치킨", "Yang Nyum Chicken", "甜味 炸鸡", "Fried chicken toss with Korean style sweet and hot sauce.", "ch01", "", "CHICKEN", "26.99"},
			{"1004", "매운양념통닭", "Hot Yang Nyum Chicken", "甜辣味 炸鸡 （特别辣）", "Fried chicken toss with Korean style hot sauce. Fire on your mouth.", "ch03", "", "CHICKEN", "27.99"},
			{"1005", "덜 매운 양념통닭", "Mild Yang Nyum Chicken", "甜辣味 炸鸡", "Fried chicken toss with Korean style medium hot sauce.", "ch04", "", "CHICKEN", "27.99"},
			{"1006", "간장닭", "Soy Chicken", "酱油味 炸鸡", "Fried chicken toss with Korean style special soy sauce.", "ch05", "", "CHICKEN", "27.99"},
			{"1007", "간장 마늘 닭", "Soy Garlic Chicken", "大蒜酱油味 炸鸡", "fried chicken toss on wok with  special soy garlic sauce.", "ch07", "","CHICKEN", "27.99"},
			{"1008", "매운 간장 마늘 닭", "Spicy Hot Soy Garlic Chicken", "大蒜酱油味 炸鸡", "Spicy Hot Soy Garlic Chicken.", "ch07", "","CHICKEN", "28.99"},
			{"1009", "파닭", "Padak", "葱丝鸡（酸甜味）", "Green onion on deep fried chicken with special sour mustard sweet sauce. Only without bone", "ch06", "", "CHICKEN", "28.99"},
			{"1010", "Half & Half", "Half & Half", "半半鸡", "You can choose half and half(Plain, Sweet, Mild, Hot).", "ch08", "CHICKEN", "", "26.99"},
			{"1011", "윙", "Wings", "鸡翅", "Chicken wings (Except Soy Garlic Sauce)", "ch10", "CHICKEN", "", "12.99"},
			{"1012", "붉닭", "Buldak", "辣烤鸡", "Spicy chicken with hot sauce.", "ch11", "CHICKEN", "", "16.99"},
			{"1013", "붉닭발", "Hot Chicken Feet", "辣烤鸡爪", "Spicy Chicken Feet.", "ch12", "CHICKEN", "", "16.99"},
			//{"1014", "찜닭", "Jjim Dak", "Chicken vegetables with glass noodle.", "ch13", "CHICKEN", "", "29.99"},

			{"2001", "콘 샐러드", "Mayo", "玉米沙拉", "Mayo corn on the hot plate.", "ds01", "DISHES", "", "2.99"},
			{"2002", "마른안주", "Dry Plate", "下酒菜(干)", "Dry plate.", "ds03", "DISHES", "", "14.99"},
			{"2003", "소세지 야채볶음", "Sausage with vegetable", "蔬菜炒香肠", "Sausage plate.", "ds04", "DISHES", "", "18.99"},
			
			{"3001", "김치 프라이", "Kim Chi Fries", "泡菜薯条(猪肉)", "Our Korean style fries topped with spicy kimchi.", "fr01", "FRIES", "", "9.99"},
			{"3002", "감자 튀김", "Potato Fries", "炸薯条", "Potato fries.", "fr03", "FRIES", "", "5.99"},
			{"3003", "고구마 튀김", "Sweet Potato Fries", "炸红薯条", "Potato fries.", "fr04", "FRIES", "", "7.99"},
			
			{"4001", "떡볶이", "Spicy Rice Cake", "辣炒年糕", "Spicy rice cake.", "rc01", "RICE CAKE", "", "9.99"},
			{"4002", "해물 떡볶이", "Sea Food Rice Cake", "海鲜辣炒年糕", "Sea food rice cake.", "rc02", "RICE CAKE", "", "14.99"},
			{"4003", "치즈 떡볶이", "Cheese Rice Cake", "奶酪辣炒年糕", "Cheese rice cake.", "rc03", "RICE CAKE", "", "11.99"},

			/*{"5001", "계란탕", "Egg Soup", "Egg Soup.", "sp01", "SOUP", "", "6.99"},
			{"5002", "오뎅탕", "Fishcake Soup", "Fishcake Soup.", "sp02", "SOUP", "", "13.99"},
			{"5003", "홍합탕", "Mussel Soup", "Mussel Soup.", "sp03", "SOUP", "", "13.99"},
			{"5004", "부대찌개", "Ham Stew", "Ham Stew.", "sp04", "SOUP", "", "15.99"},
			{"5005", "짬뽕탕", "Spicy Seafood Soup", "Spicy Seafood Soup.", "sp05", "SOUP", "", "15.99"},*/
			
			{"6001", "콜라", "Coke", "Coke", "Coke", "dr01", "DRINKS", "", "2.00"},
			{"6002", "사이다", "Sprite", "Sprite", "Sprite", "dr02", "DRINKS", "", "2.00"},
		};
		
		private string [ , ] aryOptionInformation = {
			{"1002", "10", "SUB", "한마리", "Whole", "Whole", "25.99"}, {"1001", "10", "SUB", "반마리", "Half", "Half", "15.99"},
			{"1004", "11", "SUB", "한마리", "Whole", "Whole", "26.99"}, {"1003", "11", "SUB", "반마리", "Half", "Half", "16.99"}, 
			{"1006", "12", "SUB", "한마리", "Whole", "Whole", "27.99"}, {"1005", "12", "SUB", "반마리", "Half", "Half", "17.99"}, 
			{"1008", "13", "SUB", "한마리", "Whole", "Whole", "28.99"}, {"1007", "13", "SUB", "반마리", "Half", "Half", "18.99"}, 

			//{"2", "Bone", ""}, {"2", "Boneless", ""},
			
			{"3000", "3", "ADD", "양념 치킨", "Yang Nyum Chicken", "甜味 炸鸡", ""}, {"3001", "3", "ADD", "후라이드", "Fried Chicken", "原味 炸鸡", ""}, 
			{"3002", "3", "ADD", "매운양념통닭", "Hot Yang Nyum Chicken", "甜辣味 炸鸡 （特别辣）", ""}, {"3003", "3", "ADD", "덜 매운 양념통닭", "Mild Yang Nyum Chicken", "甜辣味 炸鸡", ""}, 
			{"3004", "3", "ADD", "파닭", "Padak", "葱丝鸡（酸甜味）", "2.00"},
			
			//{"5000", "5", "", "Yang Nyum Chicken", ""}, {"5001", "5", "", "Fried Chicken", ""}, {"5002", "5", "", "Hot Yang Nyum Chicken", ""}, {"5003", "5", "", "Mild Yang Nyum Chicken", ""}, 
			//{"5004", "5", "", "Soy Chicken", ""}, {"5005", "5", "", "Padak", ""}, {"5006", "5", "", "Soy Garlic Chicken", ""},
			
			{"6000", "6", "", "Fried", "Fried", "Fried", ""}, {"6001", "6", "", "Sweet", "Sweet", "Sweet", ""}, {"6002", "6", "", "Mild", "Mild", "Mild", ""}, 
			{"6003", "6", "", "Hot", "Hot", "Hot", ""}, {"6004", "6", "", "Soy", "Soy", "Soy", ""}, {"6005", "6", "", "Padak", "Padak", "Padak", ""},
			
			{"7000", "7", "ADD", "Cheese", "Cheese", "Cheese", "2.00"}, {"7001", "7", "ADD", "No Extra", "No Extra", "No Extra", ""},
			
			{"8000", "8", "ADD", "Pork", "Pork", "Pork", "3.00"}, {"8001", "8", "ADD", "No Extra", "No Extra", "No Extra", ""},
			
			//{"9000", "9", "", "Slightly Spicy", ""}, {"9001", "9", "", "Medium Spicy", ""}, {"9002", "9", "", "Very Spicy", ""},
		};
		
		private string [ , ] aryMenuNOption = {
			{"1002", "10"},
			{"1003", "11"},
			{"1004", "12"},
			{"1005", "12"},
			{"1006", "12"},
			{"1009", "13"},
			{"1010", "3"}, {"1010", "3"},
			{"1011", "6"},
			{"1012", "7"},
			{"3001", "8"},
		};
	}
}