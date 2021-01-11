using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager playerDaddy = null;

    public PlayerControls player;
    public GameObject playerCameraLocation;
    public StarParticles playerStarParticles;
    public int keyCountCurrent = 0;
    public int keyCountTotal = 0;

    private enum PlayerState{
        LIVING,
        DEAD,
        EDGING
    }

    private enum PlayerMovementState{
        WALKING,
        DASHING,
        PHASING,
        CLIMBING
    }

    private PlayerState playerState = PlayerState.LIVING;
    private PlayerMovementState playerMovementState = PlayerMovementState.WALKING;

    void Awake()
    {
        if (playerDaddy == null)
        {
            playerDaddy = this;
        }
        else if (playerDaddy != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // public void setPlayer(PlayerControls player){
    //     playerDaddy.player = player;
    // }

    public bool PayKeys(int cost)
    {
        if (keyCountCurrent >= cost)
        {
            keyCountCurrent -= cost;
            return true;
        }
        return false;
    }

    public void GainKey()
    {
        keyCountCurrent++;
        keyCountTotal++;
    }

  

    public void setPlayerLiving(){playerState = PlayerState.LIVING;}
    public void setPlayerDead(){playerState = PlayerState.DEAD;}
    public void setPlayerEdging(){playerState = PlayerState.EDGING;} //I intended to name in that

    public void setPlayerWalking(){playerMovementState = PlayerMovementState.WALKING;}
    public void setPlayerClimbing(){playerMovementState = PlayerMovementState.CLIMBING;}
    public void setPlayerPhasing(){playerMovementState = PlayerMovementState.PHASING;}
    public void setPlayerDashing(){playerMovementState = PlayerMovementState.DASHING;}

    public bool isPlayerLiving(){return playerState == PlayerState.LIVING;}
    public bool isPlayerDead(){return playerState == PlayerState.DEAD;}
    public bool isPlayerEdging(){return playerState == PlayerState.EDGING;} //hopefully

    public bool isPlayerWalking(){return playerMovementState == PlayerMovementState.WALKING;}
    public bool isPlayerClimbing(){return playerMovementState == PlayerMovementState.CLIMBING;}
    public bool isPlayerPhasing(){return playerMovementState == PlayerMovementState.PHASING;}
    public bool isPlayerDashing(){return playerMovementState == PlayerMovementState.DASHING;}
}