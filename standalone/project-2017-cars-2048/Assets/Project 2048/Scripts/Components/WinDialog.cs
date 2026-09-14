using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using Commons.UI;
using Commons.Animations;

class WinDialog1 : MonoBehaviour
{
    public Action Action;

    void Start()
    {
        gameObject.FindChild("CloseButton")
            .OnClick(ConfirmAction);

        GameObject go;
        go = gameObject.FindChild("ConfirmButton");
        go.OnClick(ConfirmAction);
        go.AddComponent<TiltAnimator>();
    }

    void OnEnable()
    {
        Main.Instance.Theme.Apply(gameObject);
        Main.Instance.Lang.Apply(gameObject, true);
    }

    internal void ConfirmAction()
    {
        gameObject.SetActive(false);
        Action();
    }
}