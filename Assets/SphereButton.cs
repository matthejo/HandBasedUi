using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereButton : MonoBehaviour
{
    public Transform Disk;
    private Material sphereMat;
    public MeshRenderer Sphere;

    [Range(0, 1)]
    public float Pressure;

    public Transform Finger;

    private void Start()
    {
        sphereMat = Sphere.material;
    }

    private void Update()
    {
        float pressureCos = Mathf.Cos((.5f - Pressure) * Mathf.PI * 2);
        float diskZ = Mathf.Lerp(-.5f, .5f, Pressure);

        float scalePressure = Mathf.Abs(Pressure - .5f) * 2;
        float diskScale = 1 - Mathf.Pow(scalePressure, Mathf.PI);
        Disk.localPosition = new Vector3(0, 0, diskZ);
        Disk.localScale = new Vector3(diskScale, diskScale, diskScale);

        transform.LookAt(Finger);
        transform.Rotate(0, 180, 0, Space.Self);

        sphereMat.SetFloat("_Test", Pressure);
    }
}
