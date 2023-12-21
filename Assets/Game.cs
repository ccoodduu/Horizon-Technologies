using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int money;
    public OfficeType OfficeType => officeManager.officeType;
    public OfficeManager officeManager;

    public static Game i;
    void Awake()
    {
        if (i == null) i = this;

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        
    }
}
