using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public PlayerControls player;
    public GameObject playerCameraLocation;
    public StarParticles playerStarParticles;

    private enum PlayerState{
        LIVING,
        DEAD,
        CLIMBING,
        PHASING,
        EDGING
    }

    private PlayerState playerState = PlayerState.LIVING;

    void Awake(){
    }

    // public void setPlayer(PlayerControls player){
    //     playerDaddy.player = player;
    // }

  

    public void setPlayerLiving(){playerState = PlayerState.LIVING;}
    public void setPlayerDead(){playerState = PlayerState.DEAD;}
    public void setPlayerClimbing(){playerState = PlayerState.CLIMBING;}
    public void setPlayerPhasing(){playerState = PlayerState.PHASING;}
    public void setPlayerEdging(){playerState = PlayerState.EDGING;} //I intended to name in that

    public bool isPlayerLiving(){return playerState == PlayerState.LIVING;}
    public bool isPlayerDead(){return playerState == PlayerState.DEAD;}
    public bool isPlayerClimbing(){return playerState == PlayerState.CLIMBING;}
    public bool isPlayerPhasing(){return playerState == PlayerState.PHASING;}
    public bool isPlayerEdging(){return playerState == PlayerState.EDGING;} //hopefully
}