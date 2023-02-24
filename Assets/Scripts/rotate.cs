using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject bone = GameObject.Find("Bone02");
        Transform trans = bone.GetComponent<Transform>();
        trans.localRotation = Quaternion.identity;
        GameObject bone2 = GameObject.Find("Bone03");
        Transform trans2 = bone2.GetComponent<Transform>();
        trans2.localRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
