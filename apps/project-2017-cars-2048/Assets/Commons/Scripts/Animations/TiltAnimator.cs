/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Commons.Animations
{
    public class TiltAnimator : MonoBehaviour
    {
        public float ScaleMax = 1.05f;

        static float[][] transitionsFadeDelay;

        void Start()
        {
            transitionsFadeDelay = new float[3][];
            transitionsFadeDelay[0] = new float[] { 1f, 0.7f };
            transitionsFadeDelay[1] = new float[] { 1f + ((ScaleMax - 1) / 3), .03f };
            transitionsFadeDelay[2] = new float[] { ScaleMax, .1f };
            StartCoroutine(Coroutine(gameObject));
        }

        static IEnumerator Coroutine(GameObject go)
        {
            Transform tr = go.transform;
            while (true)
            {
                foreach (float[] trans in transitionsFadeDelay)
                {
                    tr.localScale = new Vector3(
                        trans[0], trans[0], trans[0]);
                    yield return new WaitForSeconds(
                        trans[1]);
                }
                foreach (float[] trans in transitionsFadeDelay.Reverse<float[]>())
                {
                    tr.localScale = new Vector3(
                        trans[0], trans[0], trans[0]);
                    yield return new WaitForSeconds(
                        trans[1]);
                }
            }
        }
    }

    public class TiltPlayerAnimator : MonoBehaviour
    {
        public float ScaleMax = 1.05f;

        static float[][] transitionsFadeDelay;

        void Start()
        {
            transitionsFadeDelay = new float[3][];
            transitionsFadeDelay[0] = new float[] { 1f, 0.7f };
            transitionsFadeDelay[1] = new float[] { 1f + ((ScaleMax - 1) / 3), .03f };
            transitionsFadeDelay[2] = new float[] { ScaleMax, .1f };
            StartCoroutine(Coroutine(gameObject));
        }

        public void Play()
        {
            StartCoroutine(Coroutine(gameObject));
        }

        static IEnumerator Coroutine(GameObject go)
        {
            Transform tr = go.transform;
            foreach (float[] trans in transitionsFadeDelay)
            {
                tr.localScale = new Vector3(
                    trans[0], trans[0], trans[0]);
                yield return new WaitForSeconds(
                    trans[1]);
            }
            foreach (float[] trans in transitionsFadeDelay.Reverse<float[]>())
            {
                tr.localScale = new Vector3(
                    trans[0], trans[0], trans[0]);
                yield return new WaitForSeconds(
                    trans[1]);
            }
        }
    }
}