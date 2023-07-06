using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UpgradeButton : TriggeredModifiers
{

    [SerializeField] private string identifier;
    [SerializeField] private List<UpgradeButton> preReqs;
    [HideInInspector] public List<UpgradeButton> children;
    [SerializeField] private int monCost;
    [SerializeField] private int polCost;
    [SerializeField] private string triggeredMod;

    private GameObject upinfo;
    private Button buyButton;
    private TMPro.TMP_Text descText;
    private TMPro.TMP_Text effectsText;
    private TMPro.TMP_Text titleText;

    public bool isBought;
    private Button ourButton;
    [SerializeField] private GameObject childImage;

    void Start()
    {
        ourButton = this.GetComponent<Button>();
        foreach (var req in preReqs)
        {
            req.children.Add(this);
        }
        //get desc butt ticks and stuff (list of children and then traverse?)
        upinfo = GameObject.FindGameObjectWithTag("upgradeinfo");
        var objs = upinfo.GetComponentsInChildren<TMPro.TMP_Text>();
        titleText = objs[0]; descText = objs[1]; effectsText = objs[2];
        buyButton = upinfo.GetComponentInChildren<Button>();

        childImage.SetActive(isBought);
    }

    public void Select()
    {
        //update text
        upinfo.SetActive(true);
        titleText.text = GetStr("up_" + identifier + "_title");
        descText.text = GetStr("up_" + identifier + "_desc");
        var effectString = GetStr("EFFECTS") + "\n" + GetStr("up_" + identifier + "_effect") + "\n\n";

        if (!isBought)
        {
            effectString += GetStr("COST") + "\n";
            if (monCost > 0)
                effectString += monCost + "  <sprite=0>" + "\n";
            if (polCost > 0)
                effectString += polCost + "  <sprite=1>"+"\n";
        }
        effectsText.text = effectString;


        buyButton.gameObject.SetActive(!isBought);
        buyButton.interactable = Purchaseable();
        buyButton.onClick.AddListener(delegate { Purchase(); });
    }

    private string GetStr(string str)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString("Upgrades", str);
    }

    public void CheckButtonAvailability()
    {
        if(ReqsFulfilled())
            ourButton.interactable = true;
        ourButton.interactable = false;
    }

    bool ReqsFulfilled()
    {
        bool reqsFulfilled = true;
        foreach (var req in preReqs)
        {
            reqsFulfilled = reqsFulfilled && req.isBought;
        }
        return reqsFulfilled;
    }

    bool Purchaseable()
    {
        return g_res.money >= monCost && g_res.politic >= polCost && ReqsFulfilled();
    }

    void Purchase()
    {
        if (!Purchaseable())
            return;

        g_res.ModifyMoney(-monCost);
        g_res.ModifyPolitic(-polCost);
        isBought = true;
        childImage.SetActive(true);
        Select();
        trigger_modifier(triggeredMod);

        foreach(var chld in children)
        {
            chld.CheckButtonAvailability();
        }
    }
}
