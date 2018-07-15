using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { forwardMove, backwardMove, lightKick, heavyKick, lightPunch, heavyPunch, crouch, blank };

public enum PlayerSide { right, left };

public static class InputTypes {
    public static Dictionary<KeyCode, InputType> keyMapping = new Dictionary<KeyCode, InputType>() {

        //PLAYER ONE
        { KeyCode.K, InputType.forwardMove },
        { KeyCode.L, InputType.forwardMove },

        { KeyCode.M, InputType.backwardMove },
        { KeyCode.Comma, InputType.backwardMove},
        { KeyCode.Period, InputType.backwardMove},

        { KeyCode.I, InputType.lightKick},
        { KeyCode.O, InputType.lightKick},

        { KeyCode.Alpha8, InputType.heavyKick},
        { KeyCode.Alpha9, InputType.heavyKick},
        { KeyCode.Alpha0, InputType.heavyKick},

        { KeyCode.J, InputType.lightPunch},
        { KeyCode.Semicolon, InputType.lightPunch},

        { KeyCode.U, InputType.heavyPunch},
        { KeyCode.P, InputType.heavyPunch},

        { KeyCode.RightAlt, InputType.crouch},
        { KeyCode.Backslash, InputType.crouch},

        //PLAYER TWO
        { KeyCode.S, InputType.forwardMove},
        { KeyCode.D, InputType.forwardMove},

        { KeyCode.Z, InputType.backwardMove},
        { KeyCode.X, InputType.backwardMove},
        { KeyCode.C, InputType.backwardMove},

        { KeyCode.W, InputType.lightKick},
        { KeyCode.E, InputType.lightKick},

        { KeyCode.Alpha2, InputType.heavyKick},
        { KeyCode.Alpha3, InputType.heavyKick},
        { KeyCode.Alpha4, InputType.heavyKick},

        { KeyCode.A, InputType.lightPunch},
        { KeyCode.F, InputType.lightPunch},

        { KeyCode.Q, InputType.heavyPunch},
        { KeyCode.R, InputType.heavyPunch},

        { KeyCode.LeftAlt, InputType.crouch},
    };

    public static InputType TypeKeyIsIn(KeyCode key) {
        InputType ret = InputType.blank;
        keyMapping.TryGetValue(key, out ret);
        return ret;
    }
}

public class KeyCombo {
    InputType[] combo;

    public bool ValidKeys(KeyCode[] keys) {

        return false;
    }
}

public class QueuedInput {
    public int frameTime = 5;
    public InputType inputType;

    public QueuedInput(InputType _inputType) {
        inputType = _inputType;
    }
}

public class HandController : MonoBehaviour {

    public Animator handimator;

    public bool left = true;

    private KeyCode[] rightKeys = {
        KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0,
        KeyCode.U, KeyCode.P,
        KeyCode.I, KeyCode.O,
        KeyCode.K, KeyCode.L,
        KeyCode.J, KeyCode.Semicolon,
        KeyCode.M, KeyCode.Comma, KeyCode.Period,
        KeyCode.RightAlt, KeyCode.Backslash
    };

    private KeyCode[] leftKeys = {
        KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.W, KeyCode.E,
        KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.D,
        KeyCode.A, KeyCode.F,
        KeyCode.Z, KeyCode.X, KeyCode.C,
        KeyCode.LeftAlt
    };

    public KeyCode[] actualKeys;

    public List<QueuedInput> inputDown = new List<QueuedInput>();
    public List<QueuedInput> inputHeld = new List<QueuedInput>();

    int executeFrames = 5;
    int framesElapsed = 0;
    int frameTime = 0;

    bool crouch = false;
    bool grounded = true;

    private void Start() {
        actualKeys = (left) ? leftKeys : rightKeys;
    }

    // Update is called once per frame
    void Update () {
        foreach (KeyCode key in actualKeys) {
            if (Input.GetKeyDown(key)) {
                inputDown.Add(new QueuedInput(InputTypes.TypeKeyIsIn(key)));
            }
        }

        foreach (QueuedInput qI in inputDown) {
            qI.frameTime--;
            if (qI.frameTime == 0) {
                ComboCheck();
                inputDown.Remove(qI);
            }
        }
	}

    void ComboCheck() {
        for (int i = inputDown.Count; i > 0; i--) {
            MoveCheck(inputDown.GetRange(0, i));
        }
    }

    void MoveCheck(List<QueuedInput> inputList) {
        //if statements mapping KeyCode[] to actual moves
    }
}
