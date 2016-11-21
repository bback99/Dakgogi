//
//  TopNavigationBar.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-13.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

class TopNavigationBar: UINavigationBar {

    override init(frame: CGRect) {
        super.init(frame: frame)
        initLogo()
    }
    
    required init(coder aDecoder: NSCoder){
        super.init(coder: aDecoder)!
        initLogo()
    }
    
    func initLogo(){
        let btnLogo = UIButton.init(type: .custom)
        btnLogo.setImage(UIImage.init(named: "img_logo"), for: UIControlState.normal)
        btnLogo.addTarget(self, action:#selector(TopNavigationBar.callbackHomepage), for: UIControlEvents.touchUpInside)
        btnLogo.frame = CGRect.init(x: 85, y: 3, width: 150, height: 40)
        addSubview(btnLogo)
    }
    
    // when clicked Logo button, call homepage
    func callbackHomepage() {
        UIApplication.shared.open(NSURL(string: "http://www.dakgogi.ca")! as URL)
    }
    
    func callbackMenu() {
        print("Menu")
    }
    
    func callbackCart() {
        print("Cart")
    }
}
