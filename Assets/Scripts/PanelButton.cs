using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButton : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public void Open()
    {
        target.SetActive(true);
    }

    public void Close()
    {
        target.SetActive(false);
    }
}
