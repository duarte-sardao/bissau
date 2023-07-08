using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeOpenClose : MonoBehaviour, IPointerClickHandler
     , IPointerEnterHandler
     , IPointerExitHandler
{

    private bool isOpen;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject upinfo;

    private void Start()
    {
        upinfo.SetActive(false);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!isOpen)
            anim.SetBool("raise", true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("raise", false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        isOpen = true;
        anim.SetBool("open", true);
    }

    public void Close()
    {
        isOpen = false;
        anim.SetBool("open", false);
        upinfo.SetActive(false);
    }

}
