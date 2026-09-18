using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Commons.UI
{
    class FocusAnimator : MonoBehaviour
    {
        // static float delayInSeconds = 1f;
        static float[][] transitionsFadeDelay;

        public FocusAnimator()
        {
            transitionsFadeDelay = new float[4][];
            transitionsFadeDelay[0] = new float[] { 1.00f, 1.5f };
            transitionsFadeDelay[1] = new float[] { 1.05f, .1f };
            transitionsFadeDelay[2] = new float[] { 1.09f, .1f };
            transitionsFadeDelay[3] = new float[] { 1.20f, .1f };
        }

        void Start()
        {
            StartCoroutine(BlinkCoroutine(gameObject));
        }

        static IEnumerator BlinkCoroutine(GameObject go)
        {
            Image img = go.GetComponent<Image>();
            Color col = img.color;
            while (true)
            {
                foreach (float[] trans in transitionsFadeDelay)
                {
                    img.color = new Color(col.r * trans[0], col.g * trans[0], col.b * trans[0]);
                    yield return new WaitForSeconds(trans[1]);
                }
                foreach (float[] trans in transitionsFadeDelay.Reverse<float[]>())
                {
                    img.color = new Color(col.r * trans[0], col.g * trans[0], col.b * trans[0]);
                    yield return new WaitForSeconds(trans[1]);
                }
            }
        }
    }
}
