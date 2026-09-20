using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Commons.UI
{
    class ToggleHelper
    {
        public static GameObject GetSelection(GameObject[] gos)
        {
            foreach (GameObject go in gos)
            {
                Toggle tog = go.GetComponent<Toggle>();
                if (tog != null && tog.isOn)
                    return go;
            }
            return null;
        }
    }
}
