//
//  SubMenuTableViewController.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-21.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

class SubMenuTableViewController: UIViewController, UITableViewDataSource, UITableViewDelegate {
    
    var subCategory_: String = ""
    var dishName_: String = ""
    var optIndex_: [String] = [String]()
    var sub_menus_ = [MenusEx]()
    var dishID_: String = ""
    var QTY_: Int = 0

    @IBOutlet weak var tvSubMenu_: UITableView!
    @IBOutlet weak var barBackToMenu_: UIBarButtonItem!
    @IBAction func stepperValueChanged(_ sender: UIStepper) {
        QTY_ = Int(sender.value)
        tvSubMenu_.reloadData()
        
        if QTY_ <= 0 {
            MenuTableViewCommon().changeButton(btn: btnPlaceAnOder_, setEnabled: false)
        }
        else {
            MenuTableViewCommon().changeButton(btn: btnPlaceAnOder_, setEnabled: true)
        }
    }
    @IBOutlet weak var btnPlaceAnOder_: UIButton!
    @IBAction func btnPlaceAnOrderClicked_(_ sender: UIButton) {
        // initialize QTY, TableView and PlaceAnOrder Button
        OrderDataManager.sharedInstance().addOrder(dishID: dishID_, QTY: QTY_)
        QTY_ = 0
        tvSubMenu_.reloadData()
        MenuTableViewCommon().changeButton(btn: btnPlaceAnOder_, setEnabled: false)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()

        // Uncomment the following line to preserve selection between presentations
        // self.clearsSelectionOnViewWillAppear = false

        // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
        // self.navigationItem.rightBarButtonItem = self.editButtonItem()
        
        self.tvSubMenu_.delegate = self
        self.tvSubMenu_.dataSource = self
        
        barBackToMenu_.title = subCategory_
        
        // to get menus by sub_menu string from mainView
        let ary_temp = DataManager.sharedInstance().getSubMenuData(subKey: subCategory_)
        
        // convert to menu_ex(with expend field) class
        for menu in ary_temp {
            let menu_ex: MenusEx = MenusEx(menus: menu)
            sub_menus_.append(menu_ex)
        }
        
        // initialize PlaceAnOrder Button
        MenuTableViewCommon().changeButton(btn: btnPlaceAnOder_, setEnabled: false)
    }
    
    @IBAction func unwindBackToMain(sender: UIStoryboardSegue) {
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if (segue.identifier == "optMenuSegue") {
            let menuView = segue.destination as! OptMenuViewController
            menuView.dishID_ = self.dishID_
            menuView.dishName_ = self.dishName_
            menuView.options_ = self.optIndex_
        }
    }

    func numberOfSections(in tableView: UITableView) -> Int {
        // #warning Incomplete implementation, return the number of sections
        return 1
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return sub_menus_.count
    }
    
    // add to tableView
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        return MenuTableViewCommon().tableView(tableView, indexPath: indexPath, sub_menus_: sub_menus_, QTY: QTY_)
    }
    
    // when clicked row
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        MenuTableViewCommon().tableView(tableView, didSelectRowAt: indexPath, sub_menus_: sub_menus_, viewController: self, dishID_: &dishID_, dishName_: &self.dishName_, optIndex_: &self.optIndex_)
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
    
    func tableView(_ tableView: UITableView, estimatedHeightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
}
