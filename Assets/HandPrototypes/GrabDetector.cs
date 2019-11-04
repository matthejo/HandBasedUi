using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDetector : MonoBehaviour
{
    public static GrabDetector Instance;

    public GameObject IndexSphere;
    public GameObject ThumbSphere;

    private Material mat;

    public bool Grabbing { get; private set; }

    public Transform GrabPoint { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mat = ThumbSphere.GetComponent<MeshRenderer>().sharedMaterial;
        GrabPoint = new GameObject("Pinch Point").transform;
    }

    private void Update()
    {
        float tipDistance = (IndexSphere.transform.position - ThumbSphere.transform.position).magnitude;
        if(Grabbing)
        {
            Grabbing = tipDistance < (ThumbSphere.transform.localScale.x * 3);
        }
        else
        {
            Grabbing = tipDistance < ThumbSphere.transform.localScale.x;
        }

        UpdateGrabPoint();
        mat.SetColor("_Color", Grabbing ? Color.green : Color.gray);
    }

    private void UpdateGrabPoint()
    {
        Vector3 grabPos = (IndexSphere.transform.position + ThumbSphere.transform.position) / 2;
        Quaternion grabRot = Quaternion.Lerp(IndexSphere.transform.rotation, ThumbSphere.transform.rotation, .5f);

        GrabPoint.position = grabPos;
        GrabPoint.rotation = grabRot;
    }
}
