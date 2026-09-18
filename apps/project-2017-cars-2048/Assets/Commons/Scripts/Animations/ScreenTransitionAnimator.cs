using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Commons.Animations
{
    class ScreenTransitionAnimator : MonoBehaviour
    {
        public static Color shadowColor = new Color(0.5f, 0.5f, 0.5f, 0f);

        public static GameObject InitShade(GameObject go)
        {
            Image img;
            img = go.AddComponent<Image>();
            img.sprite = null;
            img.color = shadowColor;
            go.GetComponent<RectTransform>()
                .sizeDelta = new Vector2(9999, 9999);
            return go;
        }
    }

    class ScreenOpenAnimator : MonoBehaviour
    {
        public float time = 0.5f;

        float timer;

        void Start()
        {
            timer = time;
        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                float alpha = timer / time;
                Image img;
                img = gameObject.GetComponent<Image>();
                img.color = new Color(
                    img.color.r,
                    img.color.g,
                    img.color.b,
                    alpha);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
