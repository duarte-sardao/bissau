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

    [SerializeField] private GameObject upinfo;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMPro.TMP_Text descText;
    [SerializeField] private TMPro.TMP_Text effectsText;
    [SerializeField] private TMPro.TMP_Text titleText;

    private ResourceManager resources;

    public bool isBought;
    private Button ourButton;

    void Start()
    {
        resources = FindObjectOfType<ResourceManager>();
        ourButton = this.GetComponent<Button>();
        foreach (var req in preReqs)
        {
            req.children.Add(this);
        }
        //get desc butt ticks and stuff (list of children and then traverse?)
        upinfo = GameObject.FindGameObjectWithTag("upgradeinfo");
        var objs = upinfo.GetComponentsInChildren<TMPro.TMP_Text>();
        Debug.Log(objs.Length);
        titleText = objs[0]; descText = objs[1]; effectsText = objs[2];
        buyButton = upinfo.GetComponentInChildren<Button>();
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
        return resources.money >= monCost && resources.politic >= polCost && ReqsFulfilled();
    }

    void Purchase()
    {
        if (!Purchaseable())
            return;

        resources.ModifyMoney(-monCost);
        resources.ModifyPolitic(-polCost);
        isBought = true;
        trigger_modifier(triggeredMod);

        foreach(var chld in children)
        {
            chld.CheckButtonAvailability();
        }
    }
}
