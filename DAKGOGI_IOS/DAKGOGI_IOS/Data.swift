//
//  Data.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-16.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import Foundation

class Data {
    
    init() {}
    
    public let aryLocalization: [String: [String]] = [
        "Language": ["English", "Korean", "Chinese"],
        "DeliveryZone": ["DELIVERY ZONE", "배달 가능지역", "送货范围"],
        "DirectCall": ["DIRECT CALL", "바로 전화", "直接呼叫"]]
    
    // ID, name_kor, name_eng, name_chn, explanation, image_name, main_category, sub_category, price
    public let aryMenuData: [String: [String]] = [
        "1001": ["치킨", "Chicken", "炸鸡", "", "ch01", "CHICKEN", "CHICKEN", "15.99"],
        "1002": ["후라이드", "Fried Chicken", "原味 炸鸡", "Deep fried chicken with Korean style speciall batter.", "ch02", "", "CHICKEN", "15.99"],
        "1003": ["양념 치킨", "Yang Nyum Chicken", "甜味 炸鸡", "Fried chicken toss with Korean style sweet and hot sauce.", "ch01", "", "CHICKEN", "16.99"],
        "1004": ["매운양념통닭", "Hot Yang Nyum Chicken", "甜辣味 炸鸡 （特别辣）", "Fried chicken toss with Korean style hot sauce. Fire on your mouth.", "ch03", "", "CHICKEN", "17.99"],
        "1005": ["덜 매운 양념통닭", "Mild Yang Nyum Chicken", "甜辣味 炸鸡", "Fried chicken toss with Korean style medium hot sauce.", "ch04", "", "CHICKEN", "17.99"],
        "1006": ["간장닭", "Soy Chicken", "酱油味 炸鸡", "Fried chicken toss with Korean style special soy sauce.", "ch05", "", "CHICKEN", "17.99"],
        "1007": ["간장 마늘 닭", "Soy Garlic Chicken", "大蒜酱油味 炸鸡", "fried chicken toss on wok with  special soy garlic sauce.", "ch07", "","CHICKEN", "17.99"],
        "1008": ["매운 간장 마늘 닭", "Spicy Hot Soy Garlic Chicken", "大蒜酱油味 炸鸡", "Spicy Hot Soy Garlic Chicken.", "ch07", "","CHICKEN", "28.99"],
        "1009": ["파닭", "Padak", "葱丝鸡（酸甜味）", "Green onion on deep fried chicken with special sour mustard sweet sauce. Only without bone", "ch06", "", "CHICKEN", "18.99"],
        "1010": ["Half & Half", "Half & Half", "半半鸡", "You can choose half and half(Plain, Sweet, Mild, Hot).", "ch08", "CHICKEN", "", "16.99"],
        "1011": ["윙", "Wings", "鸡翅", "Chicken wings (Except Soy Garlic Sauce)", "ch10", "CHICKEN", "", "12.99"],
        "1012": ["찜닭", "Jjim Dak", "蒸雞", "Chicken vegetables with glass noodle.", "ch13", "CHICKEN", "", "29.99"],
        "1014": ["붉닭", "Buldak", "辣烤鸡", "Spicy chicken with hot sauce.", "ch11", "CHICKEN", "", "16.99"],
        "1013": ["붉닭발", "Hot Chicken Feet", "辣烤鸡爪", "Spicy Chicken Feet.", "ch12", "CHICKEN", "", "16.99"],
    
        "2001": ["콘 샐러드", "Mayo", "玉米沙拉", "Mayo corn on the hot plate.", "ds01", "DISHES", "", "2.99"],
        "2002": ["마른안주", "Dry Plate", "下酒菜(干)", "Dry plate.", "ds03", "DISHES", "", "14.99"],
        "2003": ["소세지 야채볶음", "Sausage with vegetable", "蔬菜炒香肠", "Sausage plate.", "ds04", "DISHES", "", "18.99"],
    
        "3001": ["김치 프라이", "Kim Chi Fries", "泡菜薯条(猪肉)", "Our Korean style fries topped with spicy kimchi.", "fr01", "FRIES", "", "9.99"],
        "3002": ["감자 튀김", "Potato Fries", "炸薯条", "Potato fries.", "fr03", "FRIES", "", "5.99"],
        "3003": ["고구마 튀김", "Sweet Potato Fries", "炸红薯条", "Potato fries.", "fr04", "FRIES", "", "7.99"],
    
        "4001": ["떡볶이", "Spicy Rice Cake", "辣炒年糕", "Spicy rice cake.", "rc01", "RICE CAKE", "", "9.99"],
        "4002": ["해물 떡볶이", "Sea Food Rice Cake", "海鲜辣炒年糕", "Sea food rice cake.", "rc02", "RICE CAKE", "", "14.99"],
        "4003": ["치즈 떡볶이", "Cheese Rice Cake", "奶酪辣炒年糕", "Cheese rice cake.", "rc03", "RICE CAKE", "", "11.99"],
    
        "5001": ["계란탕", "Egg Soup", "鷄卵湯", "Egg Soup.", "sp01", "SOUP", "", "6.99"],
        "5002": ["오뎅탕", "Fishcake Soup", "唐魚餅", "Fishcake Soup.", "sp02", "SOUP", "", "13.99"],
        "5003": ["홍합탕", "Mussel Soup", "貽貝湯", "Mussel Soup.",  "sp03", "SOUP", "", "13.99"],
        "5004": ["부대찌개", "Ham Stew", "部隊燉菜", "Ham Stew.", "sp04", "SOUP", "", "15.99"],
        "5005": ["짬뽕탕", "Spicy Seafood Soup", "辣海鮮湯", "Spicy Seafood Soup.", "sp05", "SOUP", "", "15.99"],
    
        "6001": ["콜라", "Coke", "Coke", "Coke", "dr01", "DRINKS", "", "2.00"],
        "6002": ["사이다", "Sprite", "Sprite", "Sprite", "dr02", "DRINKS", "", "2.00"]]
    
    // ID and opt_ID
    public let aryMenuNOption: [String: [String]] = [
        "1001": ["1"],
        "1002": ["10"],
        "1003": ["11"],
        "1004": ["12"],
        "1005": ["12"],
        "1006": ["12"],
        "1009": ["13"],
        "1010": ["3", "3"],
        "1011": ["6"],
        "1014": ["7"],
        "3001": ["8"]]
    
    // ID, opt_ID, opt_type, opt_name_kor, opt_name_eng, opt_name_chn, price or extra_price
    public let aryOptionInformation: [[String]] = [
        ["1", "SUB", "", "", "", ""],
        ["10", "OPT", "한마리", "Whole", "Whole", "25.99"], ["10", "OPT", "반마리", "Half", "Half", "15.99"],
        ["11", "OPT", "한마리", "Whole", "Whole", "26.99"], ["11", "OPT", "반마리", "Half", "Half", "16.99"],
        ["12", "OPT", "한마리", "Whole", "Whole", "27.99"], ["12", "OPT", "반마리", "Half", "Half", "17.99"],
        ["13", "OPT", "한마리", "Whole", "Whole", "28.99"], ["13", "OPT", "반마리", "Half", "Half", "18.99"],
        ["3", "MULTI_OPT", "양념 치킨", "Yang Nyum Chicken", "甜味 炸鸡", ""], ["3", "MULTI_OPT", "후라이드", "Fried Chicken", "原味 炸鸡", ""],
        ["3", "MULTI_OPT", "매운양념통닭", "Hot Yang Nyum Chicken", "甜辣味 炸鸡 （特别辣）", ""], ["3", "MULTI_OPT", "덜 매운 양념통닭", "Mild Yang Nyum Chicken", "甜辣味 炸鸡", ""],
        ["3", "MULTI_OPT", "파닭", "Padak", "葱丝鸡（酸甜味）", "2.00"],
        ["6", "OPT", "Fried", "Fried", "Fried", ""], ["6", "OPT", "Sweet", "Sweet", "Sweet", ""], ["6", "OPT", "Mild", "Mild", "Mild", ""],
        ["6", "OPT", "Hot", "Hot", "Hot", ""], ["6", "OPT", "Soy", "Soy", "Soy", ""], ["6", "OPT", "Padak", "Padak", "Padak", ""],
        ["7", "OPT", "Cheese", "Cheese", "Cheese", "2.00"], ["7", "OPT", "No Extra", "No Extra", "No Extra", ""],
        ["8", "OPT", "Pork", "Pork", "Pork", "3.00"], ["8", "OPT", "No Extra", "No Extra", "No Extra", ""]]
}
