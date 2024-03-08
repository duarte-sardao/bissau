using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolderSounds : MonoBehaviour
{
    [SerializeField] private AudioSource drag;
    [SerializeField] private AudioSource open;

    public void PlayDrag()
    {
        drag.Play();
    }

    public void PlayOpen()
    {
        open.Play();
    }

}
