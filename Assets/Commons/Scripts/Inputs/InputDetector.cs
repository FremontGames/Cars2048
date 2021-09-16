/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using UnityEngine;
using System;

namespace Commons.Inputs
{
    public class InputDetector : MonoBehaviour
    {
        public delegate void Move();
        public Move Left;
        public Move Right;
        public Move Up;
        public Move Down;
    }
}