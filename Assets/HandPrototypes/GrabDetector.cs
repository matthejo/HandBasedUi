using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDetector : MonoBehaviour
{
    public float GrabDist = 0.03f;
    public float UngrabDist = 0.09f;
    private HandPrototypeProxies proxies;
    public static GrabDetector Instance;

    public bool Grabbing { get; private set; }

    public Transform GrabPoint { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        proxies = HandPrototypeProxies.Instance;
        GrabPoint = new GameObject("Pinch Point").transform;
    }

    private void Update()
    {
        float tipDistance = (proxies.RightIndex.transform.position - proxies.RightThumb.transform.position).magnitude;
        if(Grabbing)
        {
            Grabbing = tipDistance < UngrabDist;
        }
        else
        {
            Grabbing = tipDistance < GrabDist;
        }

        UpdateGrabPoint();
    }

    private void UpdateGrabPoint()
    {
        Vector3 grabPos = (proxies.RightThumb.transform.position + proxies.RightIndex.transform.position) / 2;

        GrabPoint.position = grabPos;
        GrabPoint.rotation = proxies.RightPalm.rotation;
    }
}
