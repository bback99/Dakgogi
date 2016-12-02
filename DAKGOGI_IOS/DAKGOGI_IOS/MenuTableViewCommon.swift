//
//  IMenuTableView.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-21.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

// to organize exactly the same code MenuTableViewController and SubMenuTableViewController for only two classes
// if the code below need to change, might be changed this class or even eliminated
public class MenuTableViewCommon {
    func tableView(_ tableView: UITableView, indexPath: IndexPath, sub_menus_: [MenusEx], QTY: Int) -> UITableViewCell {
        let row = indexPath.row
        if sub_menus_[row].isExpend == false {
            let cell = tableView.dequeueReusableCell(withIdentifier: "MenuTableViewCell", for: indexPath as IndexPath) as! MenuTableViewCell
            cell.ivMenu_.image = UIImage(named: "thum_list_" + sub_menus_[row].imgName)
            cell.lbMenuName_.text = sub_menus_[row].value_eng
            cell.lbExplanation_.text = sub_menus_[row].explanation
            cell.lbPrice_.text = "From $" + String(sub_menus_[row].price)
            return cell
        }
        else {
            let cell = tableView.dequeueReusableCell(withIdentifier: "MenuTableViewCellEx", for: indexPath as IndexPath) as! MenuTableViewCellEx
            cell.ivMenu_.image = UIImage(named: "thum_list_" + sub_menus_[row].imgName)
            cell.lbMenuName_.text = sub_menus_[row].value_eng
            cell.lbExplanation_.text = sub_menus_[row].explanation
            cell.lbPrice_.text = "From $" + String(sub_menus_[row].price)
            cell.lbSubTotal_.text = "SUB TOTAL: $\(Float(QTY) * sub_menus_[row].price)(X \(QTY))"
            
            if QTY <= 0 {
                cell.resetStepper()
            }
            return cell
        }        
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath, sub_menus_: [MenusEx], viewController: UIViewController, dishID_: inout String, dishName_: inout String, optIndex_: inout [String]) {
        
        dishID_ = sub_menus_[indexPath.row].key
        
        // in order to action depends on menu type, should get menu type from optionInfo_ in DataManager
        if let menuType = DataManager.sharedInstance().getMenuType(key: sub_menus_[indexPath.row].key) {
            
            if menuType == "SUB" {
                viewController.performSegue(withIdentifier: "subMenuSegue", sender: viewController)
            }
            else if menuType == "OPT" {
                dishName_ = sub_menus_[indexPath.row].value_eng
                optIndex_ = DataManager.sharedInstance().getOptionsByMenuIndex(key: sub_menus_[indexPath.row].key)
                viewController.performSegue(withIdentifier: "optMenuSegue", sender: viewController)
            }
            else if menuType == "MULTI_OPT" {
                dishName_ = sub_menus_[indexPath.row].value_eng
                optIndex_ = DataManager.sharedInstance().getOptionsByMenuIndex(key: sub_menus_[indexPath.row].key)
                viewController.performSegue(withIdentifier: "optMenuSegue", sender: viewController)
            }
            else {
                print("Error! Unkowned type: " + menuType)
            }
        }
        else {
            // when clicked cell to toggle in tapped cell, change toggle to before tapped cell
            // only one cell should be selected
            for temp in sub_menus_ {
                
                // if cell chose and cell insde for lopp is the same, then check
                if (temp.key == sub_menus_[indexPath.row].key) {
                    if (true == sub_menus_[indexPath.row].isExpend) {
                        sub_menus_[indexPath.row].isExpend = false
                    } else {
                        sub_menus_[indexPath.row].isExpend = true
                    }
                }
                else {
                    // others should be toggled
                    temp.isExpend = false
                }
            }
            tableView.reloadData()
        }
    }
    
    func changeButton(btn: UIButton, setEnabled: Bool) {
        if setEnabled == true {
            btn.isEnabled = true
            btn.alpha = 1.0
        }
        else {
            btn.isEnabled = false
            btn.alpha = 0.5
        }
    }
}
