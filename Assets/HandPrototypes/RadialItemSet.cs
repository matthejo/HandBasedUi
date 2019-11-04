using System;
using System.Linq;
using UnityEngine;

public class RadialItemSet : MonoBehaviour
{
    private float currentSubSummonTime;
    private float subSummonedness;

    private RadialItem[] items;
    public Transform[] Items;
    
    public float Radius;
    public float AngleSpread;
    public float AngleOffset;

    private void Start()
    {
        items = Items.Select(item => new RadialItem(item)).ToArray();
    }

    private void Update()
    {
        UpdateSubSummonedness();
        for (int i = 0; i < Items.Length; i++)
        {
            DoItemUpdate(Items[i], i);
        }
        UpdateCanvasAlpha();
    }

    private void UpdateSubSummonedness()
    {
        if (HandPrototypeA.Instance.State == HandPrototypeA.UiState.BrowsingTools)
        {
            currentSubSummonTime += Time.deltaTime;
        }
        else
        {
            currentSubSummonTime -= Time.deltaTime;
        }
        currentSubSummonTime = Mathf.Clamp(currentSubSummonTime, 0, HandPrototypeA.Instance.SummonTime);
        subSummonedness = currentSubSummonTime / HandPrototypeA.Instance.SummonTime;
    }

    public void DoItemUpdate(Transform item, int index)
    {
        item.gameObject.SetActive(subSummonedness > Mathf.Epsilon);
        SetItemPosition(item, index);
        item.rotation = GetRotation(item.position);
    }
    private void UpdateCanvasAlpha()
    {
        float alpha = Mathf.Pow(subSummonedness, 2);
        foreach (RadialItem item in items)
        {
            item.Canvas.alpha = alpha;
        }
    }

    private Quaternion GetRotation(Vector3 itemPosition)
    {
        Vector3 rootPos = HandPrototypeProxies.Instance.LeftPinkyKnuckle.position;
        Vector3 toRoot = rootPos - itemPosition;
        Vector3 up = Vector3.Cross(toRoot, transform.forward);
        return Quaternion.LookRotation(transform.forward, up);
    }

    private void SetItemPosition(Transform item, int index)
    {
        float angle = index * AngleSpread + AngleOffset;
        Vector3 rootPos = HandPrototypeProxies.Instance.LeftPinkyKnuckle.position;
        Vector3 toRoot = transform.position - rootPos;
        Quaternion rotator = Quaternion.AngleAxis(angle, transform.forward);

        Vector3 basePosition = toRoot.normalized * Radius + rootPos;
        item.position = basePosition;

        item.RotateAround(rootPos, transform.forward, angle);

    }

    public void ToggleIsOpen()
    {
        HandPrototypeA.Instance.OnToolsPressed();
    }

    private class RadialItem
    {
        public Transform ObjTransform { get; }
        public CanvasGroup Canvas { get; }

        public RadialItem(Transform objTransform)
        {
            ObjTransform = objTransform;
            Canvas = objTransform.gameObject.GetComponentInChildren<CanvasGroup>();
        }
    }
}