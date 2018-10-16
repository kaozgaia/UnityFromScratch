using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPool : BasePool {

    //Static Instance
    // Statics
    private static ShootPool _instance = null;
    public static ShootPool instance
    {
        get
        {
            if (_instance == null)
                _instance = (ShootPool)FindObjectOfType(typeof(ShootPool));
            return _instance;
        }
    }

}
