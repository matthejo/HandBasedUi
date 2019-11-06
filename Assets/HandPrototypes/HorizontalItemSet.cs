using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HorizontalItemSet : MonoBehaviour
{
    private float timeShown;

    public bool ShowItems;

    public GameObject[] Items;
    private HorizontalItem[] items;

    private void Start()
    {
        items = Items.Select(item => new HorizontalItem(item)).ToArray();
    }

    public void Update()
    {
        if(ShowItems)
        {
            timeShown += Time.deltaTime;
        }
        else
        {
            timeShown -= Time.deltaTime;
        }
        timeShown = Mathf.Clamp(timeShown, 0, HandPrototypeA.Instance.SummonTime);
        float fadeVal = timeShown / HandPrototypeA.Instance.SummonTime;
        foreach (HorizontalItem item in items)
        {
            item.Obj.SetActive(timeShown > float.Epsilon);
            item.Renderer.material.SetFloat("_Fade", fadeVal);
        }
    }

    private class HorizontalItem
    {
        public GameObject Obj { get; }
        public MeshRenderer Renderer { get; }

        public HorizontalItem(GameObject obj)
        {
            Obj = obj;
            Renderer = obj.GetComponentInChildren<MeshRenderer>();
        }
    }
}
