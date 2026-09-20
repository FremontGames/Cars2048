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
        public float interval = 2;
        public float[] transStep1 = new float[] {0.4f, 0.1f};
        public float[] transStep2 = new float[] {0.6f, 0.1f};
        public float[] transStep3 = new float[] {0.8f, 0.1f};
        public float[] transStep4 = new float[] {1.0f, 0.1f};

        float[][] transitions;

        public BlinkAnimator()
        {
            UpdateProps();
        }

        public void Start()
        {
            StartCoroutine(Coroutine(gameObject));
        }

        private IEnumerator Coroutine(GameObject go)
        {
            SpriteRenderer sprite = go.GetComponent<SpriteRenderer>();
            while (true)
            {
                #if UNITY_EDITOR
                    UpdateProps();
                #endif
                foreach (float[] trans in transitions)
                {
                    sprite.color = new Color(1f, 1f, 1f, trans[0]);
                    yield return new WaitForSeconds(trans[1]);
                }
                foreach (float[] trans in transitions.Reverse<float[]>())
                {
                    sprite.color = new Color(1f, 1f, 1f, trans[0]);
                    yield return new WaitForSeconds(trans[1]);
                }
                yield return new WaitForSeconds(interval);
            }
        }

        private void UpdateProps() {
            transitions = new float[4][];
            transitions[0] = new float[] { transStep1[0], transStep1[1] };
            transitions[1] = new float[] { transStep2[0], transStep2[1] };
            transitions[2] = new float[] { transStep3[0], transStep3[1] };
            transitions[3] = new float[] { transStep4[0], transStep4[1] };
        }
    }
}