using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{

    public Transform carmeraPosition;

    private void Update()
    {
        transform.position = carmeraPosition.position;
    }
}
