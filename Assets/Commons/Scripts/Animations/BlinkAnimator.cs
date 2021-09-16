/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Commons.Animations
{
    public class BlinkAnimator : MonoBehaviour
    {
       // static float delayInSeconds = 1f;
        public float[][] transitionsFadeDelay;

        public BlinkAnimator()
        {
            transitionsFadeDelay = new float[7][];
            transitionsFadeDelay[0] = new float[] { .4f, 1.5f };
            transitionsFadeDelay[1] = new float[] { .5f, .05f };
            transitionsFadeDelay[2] = new float[] { .6f, .05f };
            transitionsFadeDelay[3] = new float[] { .7f, .05f };
            transitionsFadeDelay[4] = new float[] { .8f, .05f };
            transitionsFadeDelay[5] = new float[] { .9f, .1f };
            transitionsFadeDelay[6] = new float[] { 1f, .1f };
        }

        void Start()
        {
            StartCoroutine(Coroutine(gameObject));
        }

        private IEnumerator Coroutine(GameObject go)
        {
            SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();
            while (true)
            {
                foreach (float[] trans in transitionsFadeDelay)
                {
                    sprite.color = new Color(1f, 1f, 1f, trans[0]);
                    yield return new WaitForSeconds(trans[1]);
                }
                foreach (float[] trans in transitionsFadeDelay.Reverse<float[]>())
                {
                    sprite.color = new Color(1f, 1f, 1f, trans[0]);
                    yield return new WaitForSeconds(trans[1]);
                }
            }
        }
    }
}