using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;


public static class QuaternionExt
 {
     public static Quaternion GetNormalized(this Quaternion q)
     {
         float f = 1f / Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
         return new Quaternion(q.x*f, q.y*f, q.z*f, q.w*f);
     }
 }

public class RotateHand : MonoBehaviour
{
    WebSocket ws;
    MeshFilter mf;
    Vector3[] origVerts;
    Vector3[] newVerts;
    public Vector3 scale = new Vector3(1, 1, 1);
    private float qW,qX,qY,qZ;
    public Vector3 translation  = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {   
        mf = GetComponent<MeshFilter> ();
        origVerts = mf.mesh.vertices;
        newVerts = new Vector3[origVerts.Length];
        qW = 0;
		qX = 0;
        qY = 0;
        qZ = 0;
        // Debug.Log(origVerts.Length);
        ws = new WebSocket("ws://localhost:7000");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            // Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
            //dest = float.Parse(e.Data);
			string[] data = JsonConvert.DeserializeObject<string[]>(e.Data);
			qZ = float.Parse(data[0]);
			qX = float.Parse(data[1]);
            qY = float.Parse(data[2]);
            // qZ = float.Parse(data[2]);
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Quaternion oldQuaternion = new Quaternion();
        Quaternion newQuaternion = new Quaternion();
        // oldQuaternion.Set(qX, qY, qZ, qW);
        // Debug.Log(oldQuaternion.eulerAngles[2]);
        Debug.Log(qX);
        // newQuaternion = Quaternion.Euler(0,0,90);
        // // newQuaternion = newQuaternion.GetNormalized();
        // // Debug.Log(newQuaternion);
        // // transform.rotation = newQuaternion;
        //             // Set the translation, rotation and scale parameters.
        // Matrix4x4 m = Matrix4x4.TRS(translation, newQuaternion, scale);

        // // For each vertex...
        // for (int i = 0; i < origVerts.Length; i++)
        // {
        //     // Apply the matrix to the vertex.
        //     newVerts[i] = m.MultiplyPoint3x4(origVerts[i]);
        // }

        // // Copy the transformed vertices back to the mesh.
        // mf.mesh.vertices = newVerts;
        transform.localRotation = Quaternion.Euler(0, qX, 0);

    }

    void OnApplicationQuit()
    {
        ws.Close();
    }
}
