using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Commons
{
    class GameObjectUtils
    {
        /**
         * http://answers.unity3d.com/questions/620699/scaling-my-background-sprite-to-fill-screen-2d-1.html
         */
        public static void ResizeSpriteToScreen(GameObject go)
        {
            const float MARGIN = 1.05f;

            if (go == null) return;
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr == null) return;

            go.transform.localScale = new Vector3(1, 1, 1);

            var width = sr.sprite.bounds.size.x;
            var height = sr.sprite.bounds.size.y;

            var worldScreenHeight = Camera.main.orthographicSize * 2.0;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            float x = (float)worldScreenWidth / width;
            float y = (float)worldScreenHeight / height;

            go.transform.localScale = new Vector3(x* MARGIN, y * MARGIN, 0);
        }

        public static void ResizeViewToScreen(int LandscapeSize, int PortraitSize)
        {
            Camera.main.orthographicSize = IsScreenLandscape() ? LandscapeSize : PortraitSize;
        }

        public static bool IsScreenLandscape()
        {
            return Screen.width > Screen.height;
        }

        internal static void ResizeSpriteToScreenRight(GameObject go)
        {
            if (go == null) return;
            var sr = go.GetComponent<SpriteRenderer>();
            if (sr == null) return;

            var width = sr.sprite.bounds.size.x;
            var height = sr.sprite.bounds.size.y;

            float ratio = width / height;

            var worldScreenHeight = Camera.main.orthographicSize * 2.0;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            float s = (float)worldScreenHeight / height;
            go.transform.localScale = new Vector3(s, s, 0);

            float px = (float)(worldScreenWidth / 2.0 - ((worldScreenHeight * ratio) / 2));
            go.transform.localPosition = new Vector3(px, 0, 0);
        }
    }

}

