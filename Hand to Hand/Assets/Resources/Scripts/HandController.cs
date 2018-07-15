using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { forwardMove, backwardMove, lightKick, heavyKick, lightPunch, heavyPunch, crouch };

public enum PlayerSide { right, left };

public static class InputTypes {
    static readonly KeyCode[] forwardKeys = new KeyCode[] { KeyCode.K, KeyCode.L };
    static readonly KeyCode[] backwardKeys = new KeyCode[] { KeyCode.M, KeyCode.Period, KeyCode.Slash };
    static readonly KeyCode[] lightKickKeys = new KeyCode[] { KeyCode.I, KeyCode.O, KeyCode.P };
    static readonly KeyCode[] heavyKickKeys = new KeyCode[] { KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
    static readonly KeyCode[] lightPunchKeys = new KeyCode[] { KeyCode.J, KeyCode.Semicolon };
    static readonly KeyCode[] heavyPunchKeys = new KeyCode[] { KeyCode.U, KeyCode.P };
    static readonly KeyCode[] crouchKeys = new KeyCode[] { KeyCode.Space };

    public static KeyCode[] GetTypeKeys(InputType inputType) {
        KeyCode[] ret;
        switch (inputType) {
            case (InputType.forwardMove):
                ret = forwardKeys;
                break;
            case (InputType.backwardMove):
                    ret = backwardKeys;
                    break;
            case (InputType.lightKick):
                ret = lightKickKeys;
                break;
            case (InputType.heavyKick):
                    ret = heavyKickKeys;
                    break;
            case (InputType.lightPunch):
                ret = lightPunchKeys;
                break;
            case (InputType.heavyPunch):
                ret = heavyPunchKeys;
                break;
            case (InputType.crouch):
                ret = crouchKeys;
                break;
            default:
                ret = new KeyCode[0];
                break;
        }
        return ret;
    }

    public static bool KeyInType(KeyCode key, InputType inputType) {
        KeyCode[] typeKeys = InputTypes.GetTypeKeys(inputType);

        bool isIn = false;
        foreach (KeyCode typeKey in typeKeys) {
            isIn = isIn || typeKey == key;
        }
        return isIn;
    }

    public static List<InputType> TypesKeyIsIn(KeyCode key) {
        List<InputType> typesIn = new List<InputType>();

        InputType[] allInputTypes = InputType.GetValues(typeof(InputType)) as InputType[];

        foreach (InputType type in allInputTypes) {
            if (InputTypes.KeyInType(key, type)) {
                typesIn.Add(type);
            }
        }
        return typesIn;
    }
}

public class KeyCombo {
    InputType[] combo;

    public bool ValidKeys(KeyCode[] keys) {

        return false;
    }
}

public class HandController : MonoBehaviour {

    public Animator handimator;



    int executeFrames = 5;
    int framesElapsed = 0;

    bool crouch = false;
	
    // Update is called once per frame
	void Update () {
        bool yes = true;
        if (yes) {
            Debug.Log("bla");
        }
	}
}
