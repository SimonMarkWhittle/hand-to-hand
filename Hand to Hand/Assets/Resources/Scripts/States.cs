using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action_State {
    Idle,
    Crouch_Idle,
    Airborne_Idle,      

    Moving_Forward,
    Moving_Backward,
    
    //Up_LightPunch,
    //Up_LightKick,
    //Up_HeavyPunch,
    //Up_HeavyKick,

    LightPunch = 1,
    LightKick = 2,
    HeavyPunch = 3,
    HeavyKick = 4,

    //Air_LightPunch,
    //Air_HeavyPunch,
    //Air_LightKick,
    //Air_HeavyKick,
    
    Stun,
    Jump,
    Grapple,
    Grappled,
    Dead
};

public enum Execution_State {
    Preparing, Executing, Recoiling
};

public class State {

    Action_State action;

    public virtual void Engage() {

    }

    public virtual void Conclude() {
        
    }

}
