using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : TriggeredModifiers
{

    [SerializeField] private string identifier;
    [SerializeField] private List<UpgradeButton> preReqs;
    [HideInInspector] public List<UpgradeButton> children;
    [SerializeField] private int monCost;
    [SerializeField] private int polCost;
    [SerializeField] private string triggeredMod;

    [SerializeField] private string localizedDesc;
    private Button buyButton;
    private TMPro.TextMeshPro descText;

    private ResourceManager resources;

    [HideInInspector] public bool isBought;

    void Start()
    {
        resources = FindObjectOfType<ResourceManager>();
        foreach (var req in preReqs)
        {
            req.children.Add(this);
        }
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

    public void CheckButtonAvailability()
    {
        if(ReqsFulfilled())
        {
            //colour and enabled
        } else
        {
            //b&w and disabled
        }
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
