﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action_State {
    Up_Idle, Low_Idle, Air_Idle, Moving_Forward, Moving_Backward,
    Up_LightPunch, Low_LightPunch, Air_LightPunch,
    Up_HeavyPunch, Low_HeavyPunch, Air_HeavyPunch,
    Up_LightKick, Low_LightKick, Air_LightKick,
    Up_HeavyKick, Low_HeavyKick, Air_HeavyKick,
    Stun, Jump, Grapple
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