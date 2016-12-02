//
//  CartViewController.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-22.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

class CartViewController: UIViewController, UITableViewDataSource, UITableViewDelegate {

    @IBOutlet weak var lblTotalCountTop_: UILabel!
    @IBOutlet weak var lblTotalValueTop_: UILabel!
    @IBOutlet weak var lblSubTotal_: UILabel!
    @IBOutlet weak var lblSubTotalValue_: UILabel!
    @IBOutlet weak var lblDeliveryFee_: UILabel!
    @IBOutlet weak var lblDeliveryFeeValue_: UILabel!
    @IBOutlet weak var lblHST_: UILabel!
    @IBOutlet weak var lblHSTValue_: UILabel!
    @IBOutlet weak var lblTipAndDriver_: UILabel!
    @IBOutlet weak var lblTipAndDriverValue_: UILabel!
    @IBOutlet weak var lblTotal_: UILabel!
    @IBOutlet weak var lblTotalValue_: UILabel!
    @IBOutlet weak var tvSummary_: UITableView!
    @IBOutlet weak var lblLine_: UILabel!
    @IBOutlet weak var lblEmpty_: UILabel!
    
    final private var deliveryFee: Float = 4.00
    final private var HSTRate: Float = 0.13
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        self.tvSummary_.delegate = self
        self.tvSummary_.dataSource = self

        // Do any additional setup after loading the view.
        self.title = "SUMMARY"
        
        if (OrderDataManager.sharedInstance().getOrderCount() <= 0) {
            hiddenAllComponents()
        }
        else {
            setAllComponents()
        }
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    

    // MARK: - Table view data source
    func numberOfSections(in tableView: UITableView) -> Int {
        // #warning Incomplete implementation, return the number of sections
        return 0
    }
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return 0
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "CartTableViewCell", for: indexPath as IndexPath) as! CartTableViewCell
        return cell
    }
    
    // when clicked row
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
    }
    
    
    // initialize components
    func hiddenAllComponents() {
        lblTotalCountTop_.text = String("TOTAL: 0 Items")
        lblTotalValueTop_.text = String("$ 0.00")
        lblSubTotal_.isHidden = true
        lblSubTotalValue_.isHidden = true
        lblDeliveryFee_.isHidden = true
        lblDeliveryFeeValue_.isHidden = true
        lblHST_.isHidden = true
        lblHSTValue_.isHidden = true
        lblTipAndDriver_.isHidden = true
        lblTipAndDriverValue_.isHidden = true
        lblTotal_.isHidden = true
        lblTotalValue_.isHidden = true
        tvSummary_.isHidden = true
        lblLine_.isHidden = true
        lblEmpty_.isHidden = false
    }
    
    func setAllComponents() {
        let HST: Float = ((OrderDataManager.sharedInstance().getSubTotal() * HSTRate) * 100).rounded() / 100
        let totalPrice: Float = ((OrderDataManager.sharedInstance().getSubTotal() + HST + deliveryFee)*100).rounded() / 100
        lblTotalCountTop_.text = String("TOTAL: \(OrderDataManager.sharedInstance().getOrderCount()) Items")
        lblTotalValueTop_.text = String("$ \(totalPrice)")
        lblSubTotalValue_.text = String("$ \(OrderDataManager.sharedInstance().getSubTotal())")
        lblDeliveryFeeValue_.text = String("$ \(deliveryFee)")
        lblHSTValue_.text = String("$ \(HST)")
        lblTotalValue_.text = String("$ \(totalPrice)")
        lblEmpty_.isHidden = true
    }
}
