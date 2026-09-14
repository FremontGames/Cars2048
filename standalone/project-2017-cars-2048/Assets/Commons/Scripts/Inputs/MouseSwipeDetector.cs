/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using UnityEngine;
using System;
using System.Collections;

namespace Commons.Inputs
{
    public class MouseSwipeDetector : InputDetector
    {
        private Vector2 initialPos;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                initialPos = Input.mousePosition;
            if (Input.GetMouseButtonUp(0))
                Calculate(Input.mousePosition);
        }

        void Calculate(Vector3 finalPos)
        {
            float disX = Mathf.Abs(initialPos.x - finalPos.x);
            float disY = Mathf.Abs(initialPos.y - finalPos.y);
            if (disX > 0 || disY > 0)
                if (disX > disY)
                    if (initialPos.x > finalPos.x)
                        Left();
                    else
                        Right();
                else
                    if (initialPos.y > finalPos.y)
                    Down();
                else
                    Up();
        }
    }

    public class MouseSwipeDetectorDiagonal : InputDetector
    {
        public float Margin = 30;

        private Vector2 initialPos;
        private Vector2 detectionAreaBegin;
        private Vector2 detectionAreaEnd;
        float scaleFactor;

        private void Start()
        {
            ResizeDetectionArea();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Input.mousePosition;
                if (IsInsideArea(pos))
                    initialPos = pos;
                else
                    initialPos = new Vector2(-1, -1);
            }
            if (Input.GetMouseButtonUp(0))
                if (initialPos.x != -1)
                {
                    IEnumerator coroutine = CalculateRoutine(
                        new Vector2(
                            initialPos.x, 
                            initialPos.y));
                    initialPos = new Vector2(-1, -1);
                    StartCoroutine(coroutine);
                }
        }

        private IEnumerator CalculateRoutine(Vector2 initialPos)
        {
            yield return new WaitForSeconds(0.0f);
            Calculate(initialPos, Input.mousePosition);
        }

        public void ResizeDetectionArea()
        {
            scaleFactor = gameObject
                .GetComponentInParent<Canvas>()
                    .scaleFactor;

            Vector2 pos = gameObject.transform.localPosition;
            Rect rect = (gameObject.GetComponent<RectTransform>() ??
                gameObject.AddComponent<RectTransform>())
                    .rect;

            detectionAreaBegin = new Vector2(
                    pos.x - rect.width / 2,
                    pos.y - rect.height / 2);
            detectionAreaEnd = new Vector2(
                    pos.x + rect.width / 2,
                    pos.y + rect.height / 2);
        }

        private void Calculate(Vector3 initialPos, Vector3 finalPos)
        {
            Vector2 disDiff = new Vector2(
                finalPos.x - initialPos.x,
                finalPos.y - initialPos.y);

            if (Mathf.Abs(disDiff.x) < Margin &&
                Mathf.Abs(disDiff.y) < Margin)
                return;

            float angle = ToAngle(disDiff);

            if (angle > 0 && angle <= 90)
                Up();
            else if (angle > 90 && angle <= 180)
                Left();
            else if (angle > 180 && angle <= 270)
                Down();
            else if (angle > 270 && angle <= 360)
                Right();
        }

        private float ToAngle(Vector2 disDiff)
        {
            float angle = Vector2.Angle(disDiff, Vector2.right);
            Vector3 cross = Vector3.Cross(disDiff, Vector2.right);
            if (cross.z > 0)
                angle = 360 - angle;
            return angle;
        }

        private bool IsInsideArea(Vector2 testPosition)
        {
            Vector2 pos2 = ToCanvasPosition(testPosition);
            bool res = (detectionAreaBegin.x < pos2.x &&
                pos2.x < detectionAreaEnd.x) &&
                  (detectionAreaBegin.y < pos2.y &&
                pos2.y < detectionAreaEnd.x);
            return res;
        }

        private Vector2 ToCanvasPosition(Vector2 screenPosition)
        {
            return new Vector2(
                    (screenPosition.x - (Screen.width / 2)) / scaleFactor,
                     (screenPosition.y - (Screen.height / 2)) / scaleFactor);
        }

    }
}