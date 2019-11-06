using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeSwitcher : MonoBehaviour
{
    private PrototypeStyle lastStyle;
    public PrototypeStyle Style;

    public GameObject PrototypeA;
    public GameObject PrototypeB;

    public enum PrototypeStyle
    {
        StyleA,
        StyleB,
        StyleC,
        StyleD
    }

    private void Update()
    {
        PrototypeA.SetActive(Style == PrototypeStyle.StyleA);
        PrototypeB.SetActive(Style == PrototypeStyle.StyleB);
    }
}
