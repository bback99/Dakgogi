//
//  CartDataManager.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-23.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import Foundation

class BasicOrderItem: BasicMenuData {
    private var QTY_: Int = 0
    var QTY: Int {
        set {
            self.QTY_ = newValue
        }
        get {
            return self.QTY_
        }
    }
    
    init(key: String, value_kor: String, value_eng: String, value_chn: String, QTY: Int) {
        super.init(key: key, value_kor: value_kor, value_eng: value_eng, value_chn: value_chn)
        self.QTY = QTY
    }
}

class OptionOrderItem: BasicMenuData {
    private var options_: [String] = [String]()
    var options: [String] {
        set {
            self.options = newValue
        }
        get {
            return self.options_
        }
    }
    
    init(key: String, value_kor: String, value_eng: String, value_chn: String, options: [String]) {
        super.init(key: key, value_kor: value_kor, value_eng: value_eng, value_chn: value_chn)
        self.options = options
    }
    
    public func addOption(newOption: String) {
        self.options_.append(newOption)
    }
}


class OrderDataManager {

    private static let sharedInstance_ = OrderDataManager()

    class func sharedInstance() -> OrderDataManager {
        return sharedInstance_
    }
    
    private var orders_: [BasicMenuData] = [BasicMenuData]()
    
    init() {
        
    }
    
    func Output() {
        for val in orders_ {
            if val is BasicOrderItem {
                let temp = val as! BasicOrderItem
                print("id: \(temp.key), QTY: \(temp.QTY)")
            }
        }
    }
    
    // add one order by BasicOrderItem
    public func addOrder(newOrder: BasicOrderItem) {
        orders_.append(newOrder)
        Output()
    }
    
    // add one order by parameters
    public func addOrder(dishID: String, QTY: Int) {
        if let menu = DataManager.sharedInstance().getMenuData(key: dishID) {
            let newOrder: BasicOrderItem = BasicOrderItem(key: dishID, value_kor: menu.value_kor, value_eng: menu.value_eng, value_chn: menu.value_chn, QTY: QTY)
            addOrder(newOrder: newOrder)
        }
    }
    
    public func changeOrder(index: Int, changeOrder: BasicMenuData) {
        orders_[index] = changeOrder
    }
    
    // remove all orders
    public func clearOrders() {
        orders_.removeAll()
    }
    
    // functions that related to order details
    public func getOrderCount() -> Int {
        return orders_.count
    }
    
    public func getSubTotal() -> Float {
        var subTotal: Float = 0.0
        for val in orders_ {
            let menu: Menus = DataManager.sharedInstance().getMenuData(key: val.key)!
            if val is BasicOrderItem {
                let temp = val as! BasicOrderItem
                subTotal = subTotal + (menu.price * Float(temp.QTY))
            }
            else if val is OptionOrderItem {
                
            }
        }
        return subTotal
    }
}
