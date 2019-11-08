using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPrototypeB2 : HandPrototype
{
    public SummonDetector Summoning;

    public MenuItemButton MainSpawner;
    public Transform ButtonAnchorPoint;
    public Transform PanelSpawnPoint;

    public override bool IsSummoned
    {
        get
        {
            return Summoning.IsSummoned;
        }
    }

    private void Start()
    {
        MainSpawner.Released += MainSpawner_Released;
    }

    private void MainSpawner_Released(object sender, System.EventArgs e)
    {
        PanelSpawnPoint.position = ButtonAnchorPoint.position;
        PanelSpawnPoint.rotation = ButtonAnchorPoint.rotation;
    }

    void Update()
    {
        UpdatePrimaryVisibility();
        PanelSpawnPoint.gameObject.SetActive(MainSpawner.Toggled);
        ButtonAnchorPoint.position = HandPrototypeProxies.Instance.LeftPalm.position;
        ButtonAnchorPoint.rotation = HandPrototypeProxies.Instance.LeftPalm.rotation;
    }

    private void UpdatePrimaryVisibility()
    {
        ButtonAnchorPoint.gameObject.SetActive(Summoning.IsSummoned);
    }
}
