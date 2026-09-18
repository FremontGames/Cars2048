/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Commons.Inputs
{
    class TouchSwipeDetector : InputDetector
    {
        private const int MARGIN_IN_DEGREE = 5;
        private const int MIN_DIRECTION = 0;

        private bool swiping = false;
        private bool eventSent = false;
        private Vector2 lastPosition;

        void Update()
        {
            if (Right == null || Left == null || Up == null || Down == null)
                return;

            if (Input.touchCount == 0)
                return;

            if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
            {
                if (swiping == false)
                {
                    if (!gameObject.IsInSameArea(Input.GetTouch(0).position))
                        return;
                    swiping = true;
                    lastPosition = Input.GetTouch(0).position;
                    return;
                }
                else
                {
                    if (!eventSent)
                    {
                        Vector2 direction = Input.GetTouch(0).position - lastPosition;

                        double radAngle = Math.Atan2(direction.y, direction.x);
                        if (radAngle < 0)
                            radAngle += 2 * Math.PI;
                        double angle = radAngle * (180.0 / Math.PI);

                        if (Math.Abs(direction.y) > MIN_DIRECTION && Math.Abs(direction.x) > MIN_DIRECTION)
                        {
                            if (angle > 0 + MARGIN_IN_DEGREE && angle <= 90 - MARGIN_IN_DEGREE)
                                Up();
                            else if (angle > 90 + MARGIN_IN_DEGREE && angle <= 180 - MARGIN_IN_DEGREE)
                                Left();
                            else if (angle > 180 + MARGIN_IN_DEGREE && angle <= 270 - MARGIN_IN_DEGREE)
                                Down();
                            else if (angle > 270 + MARGIN_IN_DEGREE && angle <= 360 - MARGIN_IN_DEGREE)
                                Right();
                        }

                        eventSent = true;
                    }

                }
            }
            else
            {
                swiping = false;
                eventSent = false;
            }
        }

    }

}

