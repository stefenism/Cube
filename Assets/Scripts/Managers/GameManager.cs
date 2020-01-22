using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    
    public static GameManager gameDaddy = null;

    void Awake() {
        if(gameDaddy == null){
            gameDaddy = this;
        }
        else if(gameDaddy != this){
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}