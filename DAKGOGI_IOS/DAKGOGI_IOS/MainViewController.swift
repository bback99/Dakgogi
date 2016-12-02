//
//  MainViewController.swift
//  DAKGOGI_IOS
//
//  Created by shoong on 2016-11-13.
//  Copyright © 2016 SnowBack.com. All rights reserved.
//

import UIKit

class MainViewController: UIViewController {
    
    var idxBanner_: Int = 1
    let maxBannerSize_: Int = 4
    var mainCategory_: String = ""

    @IBOutlet weak var svBanner_: UIScrollView!
    @IBOutlet weak var ivBanner_: UIImageView!
    @IBOutlet weak var barMenu: UIBarButtonItem!
    @IBOutlet weak var barCart: UIBarButtonItem!
    
    @IBOutlet weak var ivChicken_: UIImageView!
    @IBOutlet weak var ivRiceCake_: UIImageView!
    @IBOutlet weak var ivFries_: UIImageView!
    @IBOutlet weak var ivDishes_: UIImageView!
    @IBOutlet weak var ivSoups_: UIImageView!
    @IBOutlet weak var ivDrinks_: UIImageView!
    
    @IBAction func unwindBackToMain(sender: UIStoryboardSegue) {
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        
        // to set initalliy banner images
        let bannerImg: UIImage = UIImage(named: "banner0" + String(idxBanner_))!
        ivBanner_.image = bannerImg
        
        // for swiping
        initGesture()
        
        // for Navigation Bar
        initNavigationBar()
        
        // for imageView each menu
        initMenuImageViews()
        
        // initialize Data from Data.swift
        DataManager.sharedInstance().initialize()
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    // register SwipeGesture directions : left and right
    func initGesture() {
        // in order to get swipeGesture event, isUserInteractionEnabled change to true
        ivBanner_.isUserInteractionEnabled = true
        
        let swipeLeft = UISwipeGestureRecognizer(target: self, action: #selector(self.changeImage))
        swipeLeft.direction = UISwipeGestureRecognizerDirection.left;
        ivBanner_.addGestureRecognizer(swipeLeft)
        
        let swipeRight = UISwipeGestureRecognizer(target: self, action: #selector(self.changeImage))
        swipeRight.direction = UISwipeGestureRecognizerDirection.right;
        ivBanner_.addGestureRecognizer(swipeRight)
    }
    
    // whenever get swipe event, rotate banner images
    func changeImage(gesture: UIGestureRecognizer) {
        if let swipeGesture = gesture as? UISwipeGestureRecognizer {
            
            switch swipeGesture.direction {
            case UISwipeGestureRecognizerDirection.left:
                idxBanner_ += 1
                if (idxBanner_ > maxBannerSize_) {
                    idxBanner_ -= maxBannerSize_
                }
                let bannerImg: UIImage = UIImage(named: "banner0" + String(idxBanner_))!
                ivBanner_.image = bannerImg
            case UISwipeGestureRecognizerDirection.right:
                idxBanner_ -= 1
                if (idxBanner_ <= 0) {
                    idxBanner_ += maxBannerSize_
                }
                let bannerImg: UIImage = UIImage(named: "banner0" + String(idxBanner_))!
                ivBanner_.image = bannerImg
            default:
                break;
            }
        }
    }
    
    // init Navigation Bar
    func initNavigationBar() {
        barMenu!.title = ""
        barMenu!.image = UIImage(named: "ico_menu")!
        
//        barCart!.title = ""
//        barCart!.image = UIImage(named: "ico_cart")!
        
        self.navigationController!.navigationBar.barTintColor = UIColor(red: 1.0, green: 211.0/255.0, blue: 0.0, alpha: 1.0)
        
        let logoView = UIImage(named: "img_logo")
        self.navigationItem.titleView = UIImageView(image: logoView)
    }
    
    func callbackMenu() {
        
    }
    
    // init Imageviews for subMenu
    func initMenuImageViews() {
        // for Chicken
        ivChicken_.isUserInteractionEnabled = true
        let gestureChicken = UITapGestureRecognizer()
        gestureChicken.addTarget(self, action: #selector(self.callbackChichenImageTapped))
        ivChicken_.addGestureRecognizer(gestureChicken)
        
        // for RiceCake
        ivRiceCake_.isUserInteractionEnabled = true
        let gestureRiceCake = UITapGestureRecognizer()
        gestureRiceCake.addTarget(self, action: #selector(self.callbackRiceCakeImageTapped))
        ivRiceCake_.addGestureRecognizer(gestureRiceCake)
        
        // for Fries
        ivFries_.isUserInteractionEnabled = true
        let gestureFries = UITapGestureRecognizer()
        gestureFries.addTarget(self, action: #selector(self.callbackFriesImageTapped))
        ivFries_.addGestureRecognizer(gestureFries)
        
        // for Dishes
        ivDishes_.isUserInteractionEnabled = true
        let gestureDishes = UITapGestureRecognizer()
        gestureDishes.addTarget(self, action: #selector(self.callbackDishesImageTapped))
        ivDishes_.addGestureRecognizer(gestureDishes)
        
        // for Soups
        ivSoups_.isUserInteractionEnabled = true
        let gestureSoups = UITapGestureRecognizer()
        gestureSoups.addTarget(self, action: #selector(self.callbackSoupsImageTapped))
        ivSoups_.addGestureRecognizer(gestureSoups)
        
        // for Drinks
        ivDrinks_.isUserInteractionEnabled = true
        let gestureDrinks = UITapGestureRecognizer()
        gestureDrinks.addTarget(self, action: #selector(self.callbackDrinksImageTapped))
        ivDrinks_.addGestureRecognizer(gestureDrinks)

    }
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if (segue.identifier == "segueMenu") {
            let menuView = segue.destination as! MenuTableViewController
            menuView.mainCategory_ = self.mainCategory_
        }
    }
    
    // whenever clicked sub-menu image view, call segueMenu (in order to create this segue, mainview icon drag + drop into menuView)
    // each of subMenu_'s name is based on Data.swift files as a MainCategory
    func callbackChichenImageTapped() {
        mainCategory_ = "CHICKEN"
        self.performSegue(withIdentifier: "segueMenu", sender: self)
    }
    
    func callbackRiceCakeImageTapped() {
        mainCategory_ = "RICE CAKE"
        self.performSegue(withIdentifier: "segueMenu", sender: self)
    }
    
    func callbackFriesImageTapped() {
        mainCategory_ = "FRIES"
        self.performSegue(withIdentifier: "segueMenu", sender: self)
    }
    
    func callbackDishesImageTapped() {
        mainCategory_ = "DISHES"
        self.performSegue(withIdentifier: "segueMenu", sender: self)
    }
    
    func callbackSoupsImageTapped() {
        mainCategory_ = "SOUP"
        self.performSegue(withIdentifier: "segueMenu", sender: self)
    }
    
    func callbackDrinksImageTapped() {
        mainCategory_ = "DRINKS"
        self.performSegue(withIdentifier: "segueMenu", sender: self)
    }
}

