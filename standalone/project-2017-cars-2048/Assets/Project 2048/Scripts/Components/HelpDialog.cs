using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Commons;
using Commons.UI;
using Commons.Animations;

class Helps : MonoBehaviour
{
    void Start()
    {
    }
}

class NextDialog : MonoBehaviour
{
    public GameObject Next;

    void Start()
    {
        gameObject.FindChild("CloseButton")
            .OnClick(CloseAction);
        gameObject.FindChild("NextButton")
            .OnClick(NextAction);
        gameObject.FindChild("NextButton")
            .AddComponent<TiltAnimator>();
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

    internal void NextAction()
    {
        gameObject.SetActive(false);
        Next.SetActive(true);
    }
}

class NextPrevDialog : MonoBehaviour
{
    public GameObject Next;
    public GameObject Prev;

    void Start()
    {
        gameObject.FindChild("CloseButton")
            .OnClick(CloseAction);
        gameObject.FindChild("NextButton")
            .OnClick(NextAction);
        gameObject.FindChild("NextButton")
            .AddComponent<TiltAnimator>();
        gameObject.FindChild("PrevButton")
            .OnClick(PrevAction);
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

    internal void NextAction()
    {
        gameObject.SetActive(false);
        Next.SetActive(true);
    }

    internal void PrevAction()
    {
        gameObject.SetActive(false);
        Prev.SetActive(true);
    }
}

class PrevDialog : MonoBehaviour
{
    public GameObject Prev;

    void Start()
    {
        gameObject.FindChild("CloseButton")
            .OnClick(CloseAction);

        var OkButton = gameObject.FindChild("OkButton");
        OkButton.OnClick(CloseAction);
        OkButton.OnClick(SavePrefHideAtStartupAction);
        OkButton.AddComponent<TiltAnimator>();

        gameObject.FindChild("PrevButton")
            .OnClick(PrevAction);
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

    internal void PrevAction()
    {
        gameObject.SetActive(false);
        Prev.SetActive(true);
    }

    internal void SavePrefHideAtStartupAction()
    {
        PlayerPrefsHelper.SetBool(PlayerPrefsHelper.HELP_AT_STARTUP, false);
    }
}
