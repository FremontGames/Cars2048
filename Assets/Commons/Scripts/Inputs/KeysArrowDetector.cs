/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using UnityEngine;
using System;

namespace Commons.Inputs
{
    public class KeysArrowDetector : InputDetector
    {
        void Update()
        {
            if (Right == null || Left == null || Up == null || Down == null)
                return;

            if (Input.GetKeyUp(KeyCode.UpArrow))
                Up();
            else if (Input.GetKeyUp(KeyCode.DownArrow))
                Down();
            else if (Input.GetKeyUp(KeyCode.RightArrow))
                Right();
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
                Left();
        }
    }
}