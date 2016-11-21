//
//  DataManager.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-16.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import Foundation

class BasicMenuData {
    private var key_: String = ""
    var key: String {
        set {
            self.key_ = newValue
        }
        get {
            return self.key_
        }
    }
    private var value_kor_: String = ""
    var value_kor: String {
        set {
            self.value_kor_ = newValue
        }
        get {
            return self.value_kor_
        }
    }
    private var value_eng_: String = ""
    var value_eng: String {
        set {
            self.value_eng_ = newValue
        }
        get {
            return self.value_eng_
        }
    }
    private var value_chn_: String = ""
    var value_chn: String {
        set {
            self.value_chn_ = newValue
        }
        get {
            return self.value_chn_
        }
    }
    
    init(key: String, value_kor: String, value_eng: String, value_chn: String) {
        self.key_ = key
        self.value_kor_ = value_kor
        self.value_eng_ = value_eng
        self.value_chn_ = value_chn
    }
}

class Localization : BasicMenuData {
    override init(key: String, value_kor: String, value_eng: String, value_chn: String) {
        super.init(key: key, value_kor: value_kor, value_eng: value_eng, value_chn: value_chn)
    }
}

class Menus : BasicMenuData {
    private var explanation_: String = ""
    var explanation: String {
        set {
            self.explanation_ = newValue
        }
        get {
            return self.explanation_
        }
    }
    private var imgName_: String = ""
    var imgName: String {
        set {
            self.imgName_ = newValue
        }
        get {
            return self.imgName_
        }
    }
    private var mainCategory_: String? = ""
    var mainCategory: String {
        set {
            self.mainCategory_ = newValue
        }
        get {
            return self.mainCategory_!
        }
    }
    private var subCategory_: String? = ""
    var subCategory: String {
        set {
            self.subCategory_ = newValue
        }
        get {
            return self.subCategory_!
        }
    }
    private var price_: Float = 0.0
    var price: Float {
        set {
            self.price_ = newValue
        }
        get {
            return self.price_
        }
    }
    
    init(key: String, value_kor: String, value_eng: String, value_chn: String, explanation: String, imgName: String, mainCategory: String, subCategory: String, price: Float) {
        super.init(key: key, value_kor: value_kor, value_eng: value_eng, value_chn: value_chn)
        self.explanation = explanation
        self.imgName = imgName
        self.mainCategory = mainCategory
        self.subCategory = subCategory
        self.price = price
    }
}

class OptionInfo: BasicMenuData {
    private var optionIndex_: String = ""
    var optionIndex: String {
        set {
            self.optionIndex_ = newValue
        }
        get {
            return self.optionIndex_
        }
    }
    private var optionType_: String? = ""
    var optionType: String {
        set {
            self.optionType_ = newValue
        }
        get {
            return self.optionType_!
        }
    }
    private var extraPrice_: Float = 0.0
    var extraPrice: Float {
        set {
            self.extraPrice_ = newValue
        }
        get {
            return self.extraPrice_
        }
    }

    init(key: String, optionIndex: String, optionType: String, value_kor: String, value_eng: String, value_chn: String, extraPrice: Float) {
        super.init(key: key, value_kor: value_kor, value_eng: value_eng, value_chn: value_chn)
        self.optionIndex = optionIndex
        self.optionType = optionType
        self.extraPrice = extraPrice
    }
}

class MenuNOption {
    private var key_: String = ""
    var key: String {
        set {
            self.key_ = newValue
        }
        get {
            return self.key_
        }
    }
    private var aryOpt_: [String] = [String]()
    var aryOpt: [String] {
        set {
            self.aryOpt_ = newValue
        }
        get {
            return self.aryOpt_
        }
    }
    
    init(key: String, aryOpt: [String]) {
        self.key = key
        self.aryOpt = aryOpt
    }
}

class DataManager {
    
    private static let sharedInstance_ = DataManager()
    
    class func sharedInstance() -> DataManager {
        return sharedInstance_
    }

    private var Data_: Data

    private var localization_: [String: Localization]?
    private var menus_: [String: Menus]?
    private var optionInfo_: [String: OptionInfo]?
    private var menuNOpt_: [String: MenuNOption]?
    
    init() {
        Data_ = Data()
        localization_ = nil
        menus_ = nil
        optionInfo_ = nil
        menuNOpt_ = nil
    }
    
    public func initialize() {
        initLocalizationData()
        initMenuData()
        initOptionInfo()
        initMenuNOpt()
    }
    
    public func initLocalizationData() {
        self.localization_ = [String: Localization]()
        
        for (key, value) in Data_.aryLocalization {
            let temp = Localization(key: key, value_kor: value[0], value_eng: value[1], value_chn: value[2])
            localization_![key] = temp
        }
    }
    
    public func initMenuData() {
        self.menus_ = [String: Menus]()
        
        for (key, value) in Data_.aryMenuData {
            let temp = Menus(key: key, value_kor: value[0], value_eng: value[1], value_chn: value[2], explanation: value[3], imgName: value[4], mainCategory: value[5], subCategory: value[6], price: Float(value[7])!)
            menus_![key] = temp
        }
    }
    
    public func initOptionInfo() {
        self.optionInfo_ = [String: OptionInfo]()
        
        for (key, value) in Data_.aryOptionInformation {
            var extra_price = Float(value[5]) as Float?
            if extra_price == nil {
                extra_price = 0.0
            }
            
            let temp = OptionInfo(key: key, optionIndex: value[0], optionType: value[1], value_kor: value[2], value_eng: value[3], value_chn: value[4], extraPrice: extra_price!)
            optionInfo_![key] = temp
        }
    }
    
    public func initMenuNOpt() {
        self.menuNOpt_ = [String: MenuNOption]()
        
        for (key, value) in Data_.aryMenuNOption {
            let temp = MenuNOption(key: key, aryOpt: value)
            menuNOpt_![key] = temp
        }
    }
    
    // get functions by conditions
    public func getMenuData(mainCategory: String) -> [Menus] {
        var ret = [Menus]()
        
        for (_, value) in menus_! {
            // compare to Main_Category
            if (value.mainCategory == mainCategory) {
                ret.append(value)
            }
        }
        return ret
    }
    
    public func getMenuType(key: String) -> String? {
        
        if let temp = self.optionInfo_?[key] as OptionInfo? {
            if let ret = temp.optionType as String? {
                return ret
            }
        }
        return nil
    }
}
