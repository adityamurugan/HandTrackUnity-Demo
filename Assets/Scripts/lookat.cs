// You can also use transform.LookAt

using UnityEngine;
using System.Collections;

public class lookat : MonoBehaviour
{
    public Transform target;
    public Transform target2;

    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        var indexFinger = target.transform.position;
        var middleFinger = target2.transform.position;

        var vectorToMiddle = middleFinger - transform.position;
        var vectorToIndex = indexFinger - transform.position;
        //to get ortho vector of middle finger from index finger
        Vector3.OrthoNormalize(ref vectorToMiddle, ref vectorToIndex);

        //vector normal to wrist
        Vector3 normalVector = Vector3.Cross(vectorToIndex, vectorToMiddle);

        //Debug.DrawRay(wristTransform.position, normalVector, Color.white);
        //Debug.DrawRay(wristTransform.position, vectorToIndex, Color.yellow);
        transform.rotation = Quaternion.LookRotation(normalVector, vectorToIndex);
        // transform.LookAt(target);
        transform.rotation = Quaternion.AngleAxis(92, transform.right) * transform.rotation;

    }
}