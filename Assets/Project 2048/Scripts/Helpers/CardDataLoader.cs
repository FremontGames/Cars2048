using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class CardsData
{
    public CardData[] cards;
}

[Serializable]
public class CardData
{
    public int id;
    public string path;
}

public class CardView : CardData
{
    public bool locked = true;
    public string tilePath;
    public string basePath;
    public int objective;
}

class CardDataLoader
{
    public string JSONPath = "cards";

    public CardsData Load() {
        TextAsset txt = Resources.Load<TextAsset>(JSONPath);
        CardsData data = JsonUtility.FromJson<CardsData>(txt.text);
        return data;
    }

    public static CardView[] LoadAll()
    {
        var CardsData = new CardDataLoader().Load();
        int total = CardsData.cards.Length;
        CardView[] res = new CardView[total * Globals.CARDS_BY_LEVEL];
        int y = 0;
        foreach (CardData card in CardsData.cards)
        {
            string level_id = PlayerPrefsHelper.LEVEL + card.id;
            int maxTile = PlayerPrefs.GetInt(level_id);

            CardView[] ci = Load(card, maxTile);

            for (int o = 0; o < ci.Length; o++)
                res[y + o] = ci[o];
            y += ci.Length;
        }
        return res;
    }

    public static CardView[] Load(CardData card, int maxTile)
    {
        int i = 0;
        CardView[] cc = new CardView[12];
        foreach (int value in Globals.VALUES_TO_SPRITES.Keys)
        {
            cc[i] = new CardView()
            {
                id = card.id,
                locked = !((0 < maxTile) && (maxTile >= value)),
                path = card.path,
                tilePath = (card.path + "/" + Globals.VALUES_TO_SPRITES[value]),
                basePath = (card.path + "/" + "0000"),
                objective = value,
            };
            i++;
        }
        return cc;
    }

}

