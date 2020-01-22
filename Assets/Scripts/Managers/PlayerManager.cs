using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager playerDaddy = null;
    public PlayerControls player;

    private enum PlayerState{
        LIVING,
        DEAD,
        CLIMBING,
        PHASING
    }

    private PlayerState playerState = PlayerState.LIVING;

    void Awake(){
        if(playerDaddy == null){
            playerDaddy = this;
        }
        else if(playerDaddy != this){
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void setPlayer(PlayerControls player){
        playerDaddy.player = player;
    }

    public void setPlayerLiving(){playerState = PlayerState.LIVING;}
    public void setPlayerDead(){playerState = PlayerState.DEAD;}
    public void setPlayerClimbing(){playerState = PlayerState.CLIMBING;}
    public void setPlayerPhasing(){playerState = PlayerState.PHASING;}

    public bool isPlayerLiving(){return playerState == PlayerState.LIVING;}
    public bool isPlayerDead(){return playerState == PlayerState.DEAD;}
    public bool isPlayerClimbing(){return playerState == PlayerState.CLIMBING;}
    public bool isPlayerPhasing(){return playerState == PlayerState.PHASING;}


}