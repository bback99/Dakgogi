//
//  MenuTableViewController.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-16.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

class MenusEx: Menus {
    // to show expanding area
    private var isExpend_: Bool = false
    var isExpend: Bool {
        set {
            self.isExpend_ = newValue
        }
        get {
            return self.isExpend_
        }
    }
    
    init(menus: Menus) {
        super.init(key: menus.key, value_kor: menus.value_kor, value_eng: menus.value_eng, value_chn: menus.value_chn, explanation: menus.explanation, imgName: menus.imgName, mainCategory: menus.mainCategory, subCategory: menus.subCategory, price: menus.price)
        self.isExpend = false
    }
}

class MenuTableViewController: UIViewController, UITableViewDataSource, UITableViewDelegate {

    var mainCategory_: String = ""
    var sub_menus_ = [MenusEx]()
    
    @IBOutlet weak var tvMenu_: UITableView!
    @IBOutlet weak var barBackToMain_: UIBarButtonItem!
    
    override func viewDidLoad() {
        super.viewDidLoad()

        // Uncomment the following line to preserve selection between presentations
        // self.clearsSelectionOnViewWillAppear = false

        // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
        // self.navigationItem.rightBarButtonItem = self.editButtonItem()
        
        tvMenu_.delegate = self
        tvMenu_.dataSource = self
        
        barBackToMain_.title = mainCategory_
        
        // to get menus by sub_menu string from mainView
        let ary_temp = DataManager.sharedInstance().getMenuData(mainCategory: mainCategory_)
        
        // convert to menu_ex(with expend field) class
        for menu in ary_temp {
            let menu_ex: MenusEx = MenusEx(menus: menu)
            sub_menus_.append(menu_ex)
        }
        
        // sort by ID because menus_ are from dictionary
        sub_menus_ = sub_menus_.sorted(by: {$0.key < $1.key})
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    // MARK: - Table view data source

    func numberOfSections(in tableView: UITableView) -> Int {
        // #warning Incomplete implementation, return the number of sections
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return sub_menus_.count
    }
    
    // add to tableView
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let row = indexPath.row
        if sub_menus_[row].isExpend == false {
            let cell = tableView.dequeueReusableCell(withIdentifier: "MenuTableViewCell", for: indexPath as IndexPath) as! MenuTableViewCell
            cell.ivMenu_.image = UIImage(named: "thum_list_" + sub_menus_[row].imgName)
            cell.lbMenuName_.text = sub_menus_[row].value_eng
            cell.lbExplanation_.text = sub_menus_[row].explanation
            cell.lbPrice_.text = "From " + String(sub_menus_[row].price)
            return cell
        }
        else {
            let cell = tableView.dequeueReusableCell(withIdentifier: "MenuTableViewCellEx", for: indexPath as IndexPath) as! MenuTableViewCellEx
            cell.ivMenu_.image = UIImage(named: "thum_list_" + sub_menus_[row].imgName)
            cell.lbMenuName_.text = sub_menus_[row].value_eng
            cell.lbExplanation_.text = sub_menus_[row].explanation
            cell.lbPrice_.text = "From " + String(sub_menus_[row].price)
            return cell
        }
    }
    
    // when clicked row
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        // in order to action depends on menu type, should get menu type from optionInfo_ in DataManager
        if let menuType = DataManager.sharedInstance().getMenuType(key: sub_menus_[indexPath.row].key) {
            print(menuType)
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
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
    
    func tableView(_ tableView: UITableView, estimatedHeightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
}
