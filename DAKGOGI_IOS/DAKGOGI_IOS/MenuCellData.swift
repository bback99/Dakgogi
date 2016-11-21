//
//  MenuCell.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-16.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import Foundation

class MenuCellData {
        let imgName_: String
        let menuName_: String
        let explanation_: String
        let price_: Float
    
        init (imgName: String, menuName: String, explanation: String, price: Float) {
            self.imgName_ = imgName
            self.menuName_ = menuName
            self.explanation_ = explanation
            self.price_ = price
        }
    }
