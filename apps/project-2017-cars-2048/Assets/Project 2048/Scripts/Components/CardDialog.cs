using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;

class CardDialog : MonoBehaviour
{
    public CardView Card;

    Scrollbar CardDialogScrollbar;
    GameObject[] CardDialogCards = new GameObject[0];

    void Start()
    {
        gameObject.FindChild("CloseButton", true)
            .OnClick(CloseAction);
        gameObject.FindChild("ConfirmButton", true)
            .OnClick(CloseAction);
    }

    void OnEnable()
    {

        if (Card == null)
            return;
        CardDialogScrollbar = gameObject
            .FindChild("Scrollbar Horizontal", true)
                .GetComponent<Scrollbar>();
        CardDialogScrollbar.value = Ratio(Card.objective);
        BuildDialogCards(Card);
        Main.Instance.Theme.Apply(gameObject);
        Main.Instance.Lang.Apply(gameObject, true);
    }

    internal void CloseAction()
    {
        gameObject.SetActive(false);
    }

    float Ratio(int objective)
    {
        int index = Main.Instance.Core.ToTileIndex(objective);
        float ratio = ((float) index / (float) Globals.CARDS_BY_LEVEL);
        return ratio;
    }

    void BuildDialogCards(CardView card)
    {
        GameObject on, off;
        on = gameObject.FindChild("CardDetailsOnPrefab", true);
        on.SetActive(false);

        off = gameObject.FindChild("CardDetailsOffPrefab", true);
        off.SetActive(false);

        foreach (GameObject go in CardDialogCards)
            Destroy(go);
        string level_id = PlayerPrefsHelper.LEVEL + card.id;
        int maxTile = PlayerPrefs.GetInt(level_id);
        var cards = CardDataLoader.Load(card, maxTile);
        CardDialogCards = InstantiateCards(cards, on, off);
    }

    public GameObject[] InstantiateCards(CardView[] cards, GameObject on, GameObject off)
    {
        GameObject go;
        GameObject[] res = new GameObject[cards.Length];
        int i = 0;
        foreach (CardView card in cards)
        {
            // FIXME DUPLICATE : START
            if (card.locked)
            {
                go = Instantiate(off, off.transform.parent);
                go.FindChild("ObjectiveText")
                    .GetComponent<Text>()
                        .text = card.objective.ToString();
                go.SetActive(true);
                res[i] = go;
            }
            else
            {
                go = Instantiate(on, on.transform.parent);
                go.FindChild("BaseImage")
                    .GetComponent<Image>()
                        .sprite = Resources.Load<Sprite>(card
                            .basePath);
                go.FindChild("TileImage")
                    .GetComponent<Image>()
                        .sprite = Resources.Load<Sprite>(card
                            .tilePath);
                go.SetActive(true);
                res[i] = go;
            }
            // FIXME DUPLICATE : END
            i++;
        }
        return res;
    }

}