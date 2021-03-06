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
    var subCategory_: String = ""
    var dishName_: String = ""
    var optIndex_: [String] = [String]()
    var sub_menus_ = [MenusEx]()
    var dishID_: String = ""
    var QTY_: Int = 0
    
    @IBAction func stepperValueChanged(_ sender: UIStepper) {
        QTY_ = Int(sender.value)
        tvMenu_.reloadData()
        
        if QTY_ <= 0 {
            MenuTableViewCommon().changeButton(btn: btnPlaceAnOder_, setEnabled: false)
        }
        else {
            MenuTableViewCommon().changeButton(btn: btnPlaceAnOder_, setEnabled: true)
        }
    }
    @IBOutlet weak var tvMenu_: UITableView!
    @IBOutlet weak var barBackToMain_: UIBarButtonItem!
    @IBOutlet weak var btnPlaceAnOder_: UIButton!
    @IBAction func btnPlaceAnOrderClicked_(_ sender: UIButton) {
        // initialize QTY, TableView and PlaceAnOrder Button
        OrderDataManager.sharedInstance().addOrder(dishID: dishID_, QTY: QTY_)
        QTY_ = 0
        tvMenu_.reloadData()
        MenuTableViewCommon().changeButton(btn: btnPlaceAnOder_, setEnabled: false)
    }
    
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
        if (segue.identifier == "subMenuSegue") {
            let menuView = segue.destination as! SubMenuTableViewController
            menuView.subCategory_ = self.subCategory_
        }
        else if (segue.identifier == "optMenuSegue") {
            let menuView = segue.destination as! OptMenuViewController
            menuView.dishID_ = self.dishID_
            menuView.dishName_ = self.dishName_
            menuView.options_ = self.optIndex_
        }
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
        return MenuTableViewCommon().tableView(tableView, indexPath: indexPath, sub_menus_: sub_menus_, QTY: QTY_)
    }
    
    // when clicked row
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        subCategory_ = sub_menus_[indexPath.row].subCategory
        MenuTableViewCommon().tableView(tableView, didSelectRowAt: indexPath, sub_menus_: sub_menus_, viewController: self, dishID_: &self.dishID_, dishName_: &self.dishName_, optIndex_: &self.optIndex_)
    }
    
    func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
    
    func tableView(_ tableView: UITableView, estimatedHeightForRowAt indexPath: IndexPath) -> CGFloat {
        return UITableViewAutomaticDimension
    }
}
