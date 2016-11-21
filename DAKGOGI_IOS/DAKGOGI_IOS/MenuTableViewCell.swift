//
//  MenuTableViewCell.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-16.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

class MenuTableViewCell : UITableViewCell {
    
    @IBOutlet weak var ivMenu_: UIImageView!
    @IBOutlet weak var lbMenuName_: UILabel!
    @IBOutlet weak var lbExplanation_: UILabel!
    @IBOutlet weak var lbPrice_: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
    }
}

class MenuTableViewCellEx : UITableViewCell {
    
    @IBOutlet weak var ivMenu_: UIImageView!
    @IBOutlet weak var lbMenuName_: UILabel!
    @IBOutlet weak var lbExplanation_: UILabel!
    @IBOutlet weak var lbPrice_: UILabel!
    @IBOutlet weak var lbSubTotal_: UILabel!
    @IBOutlet weak var lbQTY_: UILabel!
    
    override func awakeFromNib() {
        super.awakeFromNib()
    }
    
    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)
    }
}
