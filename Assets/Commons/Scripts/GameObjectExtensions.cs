using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Commons
{
    public static class GameObjectExtensions
    {
        /**
        * http://answers.unity3d.com/questions/890636/find-an-inactive-game-object.html
        */
        public static GameObject FindChild(this GameObject parent, string nameOrPrefix, bool includeInactive = false)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(includeInactive);
            foreach (Transform t in trs)
            {
                if (t.name.StartsWith(nameOrPrefix))
                    return t.gameObject;
            }
            throw new NullReferenceException(nameOrPrefix);
        }

        public static void OnClick(this GameObject go, UnityAction action)
        {
            Button btn;
            btn = go.GetComponent<Button>();
            btn.onClick.AddListener(action);
        }

        public static bool IsInSameArea(this GameObject go, Vector2 positionInPixel)
        {
            Vector2 zone_coord = go.ScreenPosition();
            Vector2 zone_area = go.ScreenSize();
            Vector2 start = new Vector2(
                zone_coord.x - zone_area.x / 2,
                zone_coord.y - zone_area.y / 2);
            Vector2 end = new Vector2(
                zone_coord.x + zone_area.x / 2,
                zone_coord.y + zone_area.y / 2);
            bool xLimit = (start.x < positionInPixel.x) && (positionInPixel.x < end.x);
            bool yLimit = (start.y < positionInPixel.y) && (positionInPixel.y < end.y);
            return xLimit && yLimit;
        }

        private static Vector2 ScreenSize(this GameObject go)
        {
            float ratio = ScreenRatio();
            Vector2 zone_area = go.GetComponent<RectTransform>().sizeDelta;
            Vector2 objectScreen = new Vector2(
                (float)zone_area.x * ratio,
                (float)zone_area.y * ratio);
            return objectScreen;
        }

        private static Vector2 ScreenPosition(this GameObject go)
        {
            Vector3 zone_coord = go.transform.position;
            Vector3 objectScreen = Camera.main.WorldToScreenPoint(zone_coord);
            return objectScreen;
        }

        private static float ScreenRatio()
        {
            var worldScreenHeight = Camera.main.orthographicSize * 2.0;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
            Vector2 realScreen = new Vector2(
                (float)Screen.height,
                (float)Screen.width);
            Vector2 worldScreen = new Vector2(
                (float)worldScreenHeight,
                (float)worldScreenWidth);
            float ratio = realScreen.y / worldScreen.y;
            return ratio;
        }

    }

}