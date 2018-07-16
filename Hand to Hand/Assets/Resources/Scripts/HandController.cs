using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType {
    forwardMoveLeft, forwardMoveRight,
    backwardMoveLeft, backwardMoveMid, backwardMoveRight,
    lightKickLeft, lightKickRight,
    heavyKickLeft, heavyKickMid, heavyKickRight,
    lightPunchLeft, lightPunchRight,
    heavyPunchLeft, heavyPunchMid, heavyPunchRight,
    crouch, blank };

public enum PlayerSide { right, left };

public static class InputTypes {
    public static Dictionary<KeyCode, InputType> keyMapping = new Dictionary<KeyCode, InputType>() {

        //PLAYER ONE
        { KeyCode.K, InputType.forwardMoveLeft },
        { KeyCode.L, InputType.forwardMoveRight },

        { KeyCode.M, InputType.backwardMoveLeft },
        { KeyCode.Comma, InputType.backwardMoveMid},
        { KeyCode.Period, InputType.backwardMoveRight},

        { KeyCode.I, InputType.lightKickLeft},
        { KeyCode.O, InputType.lightKickRight},

        { KeyCode.Alpha8, InputType.heavyKickLeft},
        { KeyCode.Alpha9, InputType.heavyKickMid},
        { KeyCode.Alpha0, InputType.heavyKickRight},

        { KeyCode.J, InputType.lightPunchLeft},
        { KeyCode.Semicolon, InputType.lightPunchRight},

        { KeyCode.U, InputType.heavyPunchLeft},
        { KeyCode.P, InputType.heavyPunchRight},

        { KeyCode.RightAlt, InputType.crouch},
        { KeyCode.Backslash, InputType.crouch},

        //PLAYER TWO
        { KeyCode.S, InputType.forwardMoveLeft},
        { KeyCode.D, InputType.forwardMoveRight},

        { KeyCode.Z, InputType.backwardMoveLeft},
        { KeyCode.X, InputType.backwardMoveMid},
        { KeyCode.C, InputType.backwardMoveRight},

        { KeyCode.W, InputType.lightKickLeft},
        { KeyCode.E, InputType.lightKickRight},

        { KeyCode.Alpha2, InputType.heavyKickLeft},
        { KeyCode.Alpha3, InputType.heavyKickMid},
        { KeyCode.Alpha4, InputType.heavyKickRight},

        { KeyCode.A, InputType.lightPunchLeft},
        { KeyCode.F, InputType.lightPunchRight},

        { KeyCode.Q, InputType.heavyPunchLeft},
        { KeyCode.R, InputType.heavyPunchRight},

        { KeyCode.LeftAlt, InputType.crouch},
    };

    public static InputType TypeKeyIsIn(KeyCode key) {
        InputType ret = InputType.blank;
        keyMapping.TryGetValue(key, out ret);
        return ret;
    }
}


public static class KeySets {
    public static readonly KeyCode[] rightKeys = {
        KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0,
        KeyCode.U, KeyCode.P,
        KeyCode.I, KeyCode.O,
        KeyCode.K, KeyCode.L,
        KeyCode.J, KeyCode.Semicolon,
        KeyCode.M, KeyCode.Comma, KeyCode.Period,
        KeyCode.RightAlt, KeyCode.Backslash
    };

    public static readonly KeyCode[] leftKeys = {
        KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.W, KeyCode.E,
        KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.D,
        KeyCode.A, KeyCode.F,
        KeyCode.Z, KeyCode.X, KeyCode.C,
        KeyCode.LeftAlt
    };
}

public class HandController : MonoBehaviour {

    public Animator handimator;

    public bool left = true;

    public KeyCode[] actualKeys;

    MoveHandler moveHandler = new MoveHandler();

    bool crouch = false;
    bool grounded = true;

    private void Start() {
        actualKeys = (left) ? KeySets.leftKeys : KeySets.rightKeys;
    }

    // Update is called once per frame
    void Update () {
        moveHandler.Update();

        foreach (KeyCode key in actualKeys) {
            if (Input.GetKeyDown(key)) {
                moveHandler.HandleInput(InputTypes.TypeKeyIsIn(key));
            }
        }
	}
}

public class MoveHandler {
    public bool foundMove = false;
    public bool foundMoveLocal = false;

    public bool filledMove = false;
    public bool filledMoveLocal = false;

    public int frameTimer = 0;
    public const int FRAME_TIME = 5;

    /*
     *     forwardMoveLeft, forwardMoveRight,
    backwardMoveLeft, backwardMoveMid, backwardMoveRight,
    lightKickLeft, lightKickRight,
    heavyKickLeft, heavyKickMid, heavyKickRight,
    lightPunchLeft, lightPunchRight,
    heavyPunchLeft, heavyPunchMid, heavyPunchRight,
    crouch, blank };
     */

    List<InputType> inputList = new List<InputType>();
    Move[] moveList = new Move[] {
new Move(new HashSet<InputType>() { InputType.lightKickLeft, InputType.lightKickRight}, "double kick"),

new Move(new HashSet<InputType>() { InputType.forwardMoveLeft}, "forward move left"),
new Move(new HashSet<InputType>() { InputType.forwardMoveRight}, "forward move right"),

new Move(new HashSet<InputType>() { InputType.backwardMoveLeft}, "backward move left"),
new Move(new HashSet<InputType>() { InputType.backwardMoveRight}, "backward move right"),

new Move(new HashSet<InputType>() { InputType.heavyPunchLeft}, "heavy punch left"),
new Move(new HashSet<InputType>() { InputType.heavyPunchRight}, "heavy punch right"),

new Move(new HashSet<InputType>() { InputType.heavyKickLeft}, "heavy kick left"),
new Move(new HashSet<InputType>() { InputType.heavyKickRight}, "heavy kick right"),

new Move(new HashSet<InputType>() { InputType.lightPunchLeft}, "light punch left"),
new Move(new HashSet<InputType>() { InputType.lightPunchRight}, "light punch right"),

new Move(new HashSet<InputType>() { InputType.lightKickLeft}, "light kick left"),
new Move(new HashSet<InputType>() { InputType.lightKickRight}, "light kick right"),
};

    public void Update() {
        if (frameTimer > 0) {
            frameTimer--;

            if (frameTimer == 0 && inputList.Count > 0) {
                Trigger();
            }
        }
    }

    public bool HandleInput(InputType input) {
        //check new move
        if (frameTimer <= 0) {
            frameTimer = FRAME_TIME;
            filledMove = false;
        }

        foundMove = false;

        if (inputList.Contains(input)) {
            Trigger();
            return false;
        }
        else {
            inputList.Add(input);
        }

        if (!CheckMove(input)) {
            Trigger();
        }

        return false;
    }

    bool CheckMove(InputType input) {
        foreach (Move move in moveList) {         
            if (!filledMove || !move.ignore){
                if (!filledMove) {
                    move.ResetFilled();
                }
                foundMoveLocal = move.CheckMatch(input);
                foundMove |= foundMoveLocal;
                if (foundMoveLocal) {
                    if (foundMove) {
                        filledMoveLocal = move.CheckFilled(inputList.Count);
                        filledMove |= filledMoveLocal;
                    }
                }
                else if (filledMove) {
                    move.ignore = true;
                }
            }
        }
        return foundMove;
    }

    void Trigger() {
        if (filledMove) {
            foreach (Move move in moveList) {
                if (move.filled) {
                    Debug.Log(move.name + ": " + frameTimer);
                    break;
                }
            }
        }

        inputList.Clear();
        frameTimer = 0;
    }
}

public class Move {
    HashSet<InputType> combination = new HashSet<InputType>();
    public int count;
    public bool filled = false;
    public bool ignore = false;

    public string name;

    public Move(HashSet<InputType> _combination, string _name) {
        combination = _combination;
        count = combination.Count;

        name = _name;
    }

    public bool CheckMatch(InputType input) {
        return combination.Contains(input);
    }

    public bool CheckFilled(int moveCount) {
        return filled = ignore = count == moveCount;
    }

    public void ResetFilled() {
        filled = false;
        ignore = false;
    }
}


public class ComboHandler { }
