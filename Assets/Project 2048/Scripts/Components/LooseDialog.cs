using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Commons;
using Commons.UI;
using Commons.Animations;

class LooseDialog : MonoBehaviour
{
    public Action ConfirmAction;
    public Action CancelAction;

    void Start()
    {
        gameObject.FindChild("ConfirmButton")
            .OnClick(ConfirmAction_internal);
        gameObject.FindChild("CancelButton")
            .OnClick(CancelAction_internal);

        gameObject.FindChild("CloseButton")
         .OnClick(CancelAction_internal);
    }

    void OnEnable()
    {
        Main.Instance.Theme.Apply(gameObject);
        Main.Instance.Lang.Apply(gameObject, true);
    }

    internal void CancelAction_internal()
    {
        gameObject.SetActive(false);
        CancelAction();
    }

    internal void ConfirmAction_internal()
    {
        gameObject.SetActive(false);
        ConfirmAction();
    }
}

