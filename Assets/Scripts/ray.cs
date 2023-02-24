using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, -transform.up*10, Color.green);
        Ray ray = new Ray(transform.position, -transform.up*10);
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData))
        {
            Debug.Log("Hit "+hitData.collider.gameObject.name, hitData.collider.gameObject);
        }
    }
}
