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
    crouch, blank
};

public enum MoveType {
    doubleKick,
    forwardMoveLeft, fowardMoveMid, forwardMoveRight,
    backwardMoveLeft, backwardMoveMid, backwardMoveRight,
    heavyPunchLeft, heavyPunchRight,
    heavyKickLeft, heavyKickRight,
    lightPunchLeft, lightPunchRight,
    lightKickLeft, lightKickRight,
    blank
}

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

    public PlayerSide side = PlayerSide.left;

    public KeyCode[] actualKeys;

    MoveHandler moveHandler = new MoveHandler();

    bool crouch = false;
    bool grounded = true;

    private void Start() {
        actualKeys = (side == PlayerSide.left) ? KeySets.leftKeys : KeySets.rightKeys;
    }

    // Update is called once per frame
    void Update() {
        moveHandler.Update();

        foreach (KeyCode key in actualKeys) {
            if (Input.GetKeyDown(key)) {
                moveHandler.HandleInput(InputTypes.TypeKeyIsIn(key));
            }
        }
    }
}

public class MoveHandler {
    bool foundMove = false;
    bool foundMoveLocal = false;

    bool filledMove = false;
    bool filledMoveLocal = false;

    int frameTimer = 0;
    const int FRAME_TIME = 5;

    ComboHandler comboHandler = new ComboHandler();

    List<InputType> inputList = new List<InputType>();
    Move[] moveList = new Move[] {
new Move(new HashSet<InputType>() { InputType.lightKickLeft, InputType.lightKickRight}, MoveType.doubleKick),

new Move(new HashSet<InputType>() { InputType.forwardMoveLeft}, MoveType.forwardMoveLeft),
new Move(new HashSet<InputType>() { InputType.forwardMoveRight}, MoveType.forwardMoveRight),

new Move(new HashSet<InputType>() { InputType.backwardMoveLeft}, MoveType.backwardMoveLeft),
new Move(new HashSet<InputType>() { InputType.backwardMoveMid}, MoveType.backwardMoveMid),
new Move(new HashSet<InputType>() { InputType.backwardMoveRight}, MoveType.backwardMoveRight),

new Move(new HashSet<InputType>() { InputType.heavyPunchLeft}, MoveType.heavyPunchLeft),
new Move(new HashSet<InputType>() { InputType.heavyPunchRight}, MoveType.heavyPunchRight),

new Move(new HashSet<InputType>() { InputType.heavyKickLeft}, MoveType.heavyKickLeft),
new Move(new HashSet<InputType>() { InputType.heavyKickRight}, MoveType.heavyKickRight),

new Move(new HashSet<InputType>() { InputType.lightPunchLeft}, MoveType.lightPunchLeft),
new Move(new HashSet<InputType>() { InputType.lightPunchRight}, MoveType.lightPunchRight),

new Move(new HashSet<InputType>() { InputType.lightKickLeft}, MoveType.lightKickLeft),
new Move(new HashSet<InputType>() { InputType.lightKickRight}, MoveType.lightKickRight)
};

    public void Update() {
        if (frameTimer > 0) {
            frameTimer--;

            if (frameTimer == 0 && inputList.Count > 0) {
                Trigger();
            }

        }

        comboHandler.Update();
    }

    //This came to me in a dream
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
            if (!filledMove || !move.ignore) {

                if (!filledMove) {
                    move.ResetFilled();
                }

                foundMoveLocal = move.CheckMatch(input);
                foundMove |= foundMoveLocal;

                if (foundMoveLocal) {
                    //If filled, move is internally flagged as ignored
                    filledMoveLocal = move.CheckFilled(inputList.Count);
                    filledMove |= filledMoveLocal;
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
                    if (!comboHandler.CheckCombo(move)) {
                        Debug.Log(move.moveType.ToString());
                    }
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

    public MoveType moveType;

    public Move(HashSet<InputType> _combination, MoveType _moveType = MoveType.blank) {
        combination = _combination;
        count = combination.Count;

        moveType = _moveType;
    }

    public bool CheckMatch(InputType input) {
        return combination.Contains(input);
    }

    public bool CheckFilled(int moveCount) {
        return filled = ignore = count == moveCount;
    }

    public void ResetFilled() {
        filled = ignore = false;
    }
}


public class ComboHandler {
    int frameTimer = 0;
    const int FRAME_TIME = 10;
    List<Combo> comboList = new List<Combo>() {
        new Combo(new MoveType[] { MoveType.lightPunchLeft, MoveType.lightPunchLeft}, "the ol' razzle dazzle"),
        new Combo(new MoveType[] { MoveType.lightPunchLeft, MoveType.lightPunchLeft, MoveType.lightPunchRight, MoveType.lightPunchRight}, "the new razzle dazzle"),
        new Combo(new MoveType[] { MoveType.heavyKickRight, MoveType.heavyKickRight, MoveType.heavyKickLeft, MoveType.heavyKickLeft}, "the one-two kick")
    };
    Combo currentCombo;
    int comboCount = 0;
    bool comboFound = false;

    public void Update() {
        if (frameTimer > 0) {
            frameTimer--;

            if (frameTimer == 0) {
                currentCombo = null;
                comboCount = 0;
                foreach (Combo combo in comboList) {
                    combo.ResetCombo();
                }
            }
        }
    }

    public bool CheckCombo(Move move) {
        if (frameTimer == 0) {
            frameTimer = FRAME_TIME;
            currentCombo = null;
            comboCount = 0;
        }

        comboFound = false;

        foreach (Combo combo in comboList) {
            if (!combo.ignore) {
                if (combo.HasCombo(move, comboCount)) {
                    frameTimer = FRAME_TIME;
                    if (comboCount == combo.moveCount) {
                        currentCombo = combo; //perform combo move
                        Debug.Log("COMBO: " + combo.name);
                        comboFound = true;
                    }
                }
                else {
                    combo.ignore = true;
                }
            }
        }

        comboCount++;
        return comboFound;
    }
}

public class Combo {
    MoveType[] moveCombo;
    public int moveCount;
    public bool ignore = false;
    public string name;

    public Combo(MoveType[] _moveCombo, string _name = "combo") {
        moveCombo = _moveCombo;
        moveCount = moveCombo.Length - 1;
        name = _name;
    }

    public void ResetCombo() {
        ignore = false;
    }

    public bool HasCombo(Move move, int comboCount) {
        if (comboCount < moveCombo.Length)
            return moveCombo[comboCount] == move.moveType;

        return false;
    }
}
