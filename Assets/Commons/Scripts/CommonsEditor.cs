// https://support.unity3d.com/hc/en-us/articles/208456906-Excluding-Scripts-and-Assets-from-builds
#if (UNITY_EDITOR)
using UnityEditor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace Commons
{
    // EDITOR TOOLS ***********************************************************

    // http://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html
    [InitializeOnLoad]
    public class EditorGameObjectActiveToggler
    {
        // https://unity3d.com/fr/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items

        [MenuItem("Tools/GameObject/Toggle active state %&X")]
        private static void ApplyThemeOption()
        {
            var selected = Selection.activeObject;
            if (selected == null)
            {
                Debug.Log("ERROR: Please select something!");
                return;
            }
            if (selected.GetType() != typeof(GameObject))
            {
                Debug.Log("ERROR: Please select a GameObject!");
                return;
            }
            GameObject go = (GameObject)selected;
            bool active = go.activeSelf;
            go.SetActive(!active);
        }
    }

    // TODO http://answers.unity3d.com/questions/956123/add-and-select-game-view-resolution.html

}
#endif