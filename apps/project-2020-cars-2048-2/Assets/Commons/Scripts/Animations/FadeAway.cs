/* Copyright (C) 2020 Damien Fremont - All Rights Reserved
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
    public class FadeAway : MonoBehaviour
    {
        public delegate void Callback();
        public Callback callback;

        float[][] transitionsFadeDelay;
        Color col;
        bool hasCol;

        public FadeAway()
        {
            hasCol = false;
            transitionsFadeDelay = new float[4][];
            transitionsFadeDelay[0] = new float[] { 1f, .2f };
            transitionsFadeDelay[1] = new float[] { .7f, .1f };
            transitionsFadeDelay[2] = new float[] { .4f, .05f };
            transitionsFadeDelay[3] = new float[] { .2f, .05f };
        }

        public void Start()
        {
            StartCoroutine(Coroutine(gameObject));
        }
        
        private IEnumerator Coroutine(GameObject go)
        {
            SpriteRenderer spr = gameObject.GetComponent<SpriteRenderer>();
            if(!hasCol) {
                col = new Color(spr.color.r, spr.color.g, spr.color.b, spr.color.a);
                hasCol = true;
            }
            foreach (float[] trans in transitionsFadeDelay)
            {
                spr.color = new Color(col.r, col.g, col.b, col.a * trans[0]);
                yield return new WaitForSeconds(trans[1]);
            }
            callback();
        }
    }
}