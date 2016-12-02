//
//  OptMenuViewController.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-21.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

class OptMenuViewController: UIViewController, UITableViewDataSource, UITableViewDelegate {
    
    // basic information
    var dishID_: String = ""
    var dishName_: String = ""
    var options_: [String] = [String]() // from data
    var optionInfo_: [OptionInfo] = [OptionInfo]()
    var selectedCell: OptionTableViewCell = OptionTableViewCell()
    
    // changeable variables from user clicked
    var rowCount = 0
    var pageIndex_ = 0
    var chosenFirstOption: String = ""
    var clickedIndex: [Int] = [0, 0]
    
    @IBOutlet weak var tvOption_: UITableView!
    @IBOutlet weak var barBackToMenu_: UIBarButtonItem!
    @IBOutlet weak var btnPlaceAnOrder_: UIButton!
    @IBAction func btnPlaceAnOrderClicked_(_ sender: UIButton) {
        
        if options_.count > 1 && pageIndex_ == 0 {
            pageIndex_ = pageIndex_ + 1
            viewDidLoad()
            tvOption_.reloadData()
        }
        else {
        }
        
        // run order function
        print(dishID_)
        print(dishName_)
        print(clickedIndex)
        print(chosenFirstOption)
    }
    
    @IBAction func unwindBackToMain(sender: UIStoryboardSegue) {
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
        self.tvOption_.delegate = self
        self.tvOption_.dataSource = self
        
        self.title = "Choose Your Option"
        optionInfo_ = DataManager.sharedInstance().getOptions(key: options_[pageIndex_])
        var temp = optionInfo_
        if (!chosenFirstOption.isEmpty) {
            if let index = temp.index(where: { $0.value_eng == chosenFirstOption }) {
                temp.remove(at: index)
            }
        }
        
        optionInfo_ = temp
        
        // initialize PlaceAnOrder Button
        if options_.count > 1 {
            if (pageIndex_ == 0) {
                self.title = "Choose First Option"
                btnPlaceAnOrder_!.setTitle("NEXT", for: .normal)
            }
            else {
                self.title = "Choose Second Option"
                btnPlaceAnOrder_!.setTitle("Place An Order", for: .normal)
            }
        }
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
        return optionInfo_.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "OptionTableViewCell", for: indexPath as IndexPath) as! OptionTableViewCell
        var option = optionInfo_[indexPath.row].value_eng
        if (optionInfo_[indexPath.row].extraPrice > 0) {
            
            // depends on menu_type
            if let menuType = DataManager.sharedInstance().getMenuType(key: dishID_) {
                if menuType == "OPT" {
                    option = option + " ($" + String(optionInfo_[indexPath.row].extraPrice) + ")"
                }
                else {
                    option = option + " (+ $" + String(optionInfo_[indexPath.row].extraPrice) + ")"
                }
            }
        }
        cell.textLabel?.text = option
        
        if indexPath.row == 0 {
            cell.accessoryType = .checkmark
            selectedCell = cell
        }
        else {
            cell.accessoryType = .none
        }
        return cell
    }
    
    // when clicked row
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        // previous cell will be set .none, clicked cell will be set .checkmark
        if let cell = tableView.cellForRow(at: indexPath) {
            selectedCell.accessoryType = .none
            cell.accessoryType = .checkmark
            selectedCell = cell as! OptionTableViewCell
        }
        
        // save first option's English Name in order to compare with second option
        chosenFirstOption = optionInfo_[indexPath.row].value_eng
        clickedIndex[pageIndex_] = indexPath.row
    }
}
