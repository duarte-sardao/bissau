using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : TriggeredModifiers
{
    static Dictionary<string, bool> isCompleted;

    public string identifier;
    public List<string> preReqs;
    public int monCost;
    public int polCost;
    public string triggeredMod;

    public string localizedDesc;
    private Button buyButton;
    private TMPro.TextMeshPro descText;

    private ResourceManager resources;

    void Start()
    {
        isCompleted.Add(identifier, false);
        resources = FindObjectOfType<ResourceManager>();
        //get desc butt ticks and stuff (list of children and then traverse?)
    }

    private void Select()
    {
        //update text

        //if bought put tick else
        if(Purchaseable())
        {
            buyButton.gameObject.SetActive(true);
            buyButton.onClick.AddListener(delegate { Purchase(); });
        }
    }

    bool Purchaseable()
    {
        bool reqsFulfilled = true;
        foreach(string req in preReqs)
        {
            reqsFulfilled = reqsFulfilled && isCompleted[req];
        }
        return resources.money >= monCost && resources.politic >= polCost && reqsFulfilled;
    }

    void Purchase()
    {
        if (!Purchaseable())
            return;
        resources.ModifyMoney(-monCost);
        resources.ModifyPolitic(-polCost);
        isCompleted[identifier] = true;
        trigger_modifier(triggeredMod);
    }
}
