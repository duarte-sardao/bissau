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
    [SerializeField] private GameObject innerCanvas;

    private void Start()
    {
        innerCanvas.SetActive(false);
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
        Invoke(nameof(OpCanv), 0.75f);
    }

    public void Close()
    {
        isOpen = false;
        anim.SetBool("open", false);
        innerCanvas.SetActive(false);
    }

    private void OpCanv()
    {
        innerCanvas.SetActive(true);
    }

}
