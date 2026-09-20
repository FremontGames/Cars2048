using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using Commons.UI;

class QuitDialog : MonoBehaviour
{
    public Action QuitAction;

    void Start()
    {
        gameObject.FindChild("CloseButton")
            .OnClick(CloseAction);
        gameObject.FindChild("CancelButton")
            .OnClick(CloseAction);
        gameObject.FindChild("ConfirmButton")
            .OnClick(ConfirmAction);
    }

    void OnEnable()
    {
        Main.Instance.Theme.Apply(gameObject);
        Main.Instance.Lang.Apply(gameObject, true);
    }

    internal void CloseAction()
    {
        gameObject.SetActive(false);
    }

    internal void ConfirmAction()
    {
        gameObject.SetActive(false);
        QuitAction();
    }
}
