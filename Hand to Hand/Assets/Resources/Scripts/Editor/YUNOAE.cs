using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(YUNOAnim))]
public class YUNOAE : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        YUNOAnim obj = (YUNOAnim)target;

        Animator animator = obj.GetComponent(typeof(Animator)) as Animator;
        if (animator != null) {
            EditorGUILayout.LabelField("Has Animator");
            Avatar avatar = animator.avatar;
            if (avatar != null)
                EditorGUILayout.LabelField("Has Avatar");
            if(avatar.isValid)
                EditorGUILayout.LabelField("Avatar is valid");
            if (avatar.isHuman)
                EditorGUILayout.LabelField("Avatar is human");
        }
    }
}
