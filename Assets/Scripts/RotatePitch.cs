using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using System.Linq;


public class RotatePitch : MonoBehaviour
{
    WebSocket ws;
    private GameObject hand,parentHand,handL,parentHandL;
    public Vector3 scale = new Vector3(1, 1, 1);
    private float qX,qY,qZ,qA,iX,iY,iZ;
    private float qXL,qYL,qZL,qAL,iXL,iYL,iZL;
    public float recCommand,recCommandL;
    public Vector3 translation  = new Vector3(0, 0, 0);
    public Transform anchor,anchorL,cubeL;
    Vector3 currentEulerAngles,currentEulerAnglesL;
    public Transform fingerJoint,fingerJointL;
    public Transform indexPos,indexPosL;
    private Vector3 pH,pHL;
    public List<List<string>> dataChar;
    public List<string> data, dataL;
    public List<GameObject> hingeJoints,hingeJointsL = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {   
		qX = qXL = 0;
        qY = qYL = 0;
        qZ = qZL = 0;
		iX = iXL = 0;
        iY = iYL = 0;
        iZ = iZL = 0;
        qA = qAL = 0;
        recCommand = 0;
        recCommandL = 0;
        hand = GameObject.Find("parentHand/SK_VR_Latex_Glove_R/VRHand_Right/hand_r");
        handL = GameObject.Find("parentHandL/SK_VR_Latex_Glove_L/VRHand_Left/hand_l");
        parentHand = GameObject.Find("parentHand");
        parentHandL = GameObject.Find("parentHandL");
        Debug.Log(parentHand.transform.position);
        pH = parentHand.transform.position;
        pHL = parentHandL.transform.position;
        foreach (Transform g in hand.transform.GetComponentsInChildren<Transform>())
        {
            hingeJoints.Add(GameObject.Find(g.name));
        }
        hingeJoints.RemoveAt(0);
        foreach (Transform g in handL.transform.GetComponentsInChildren<Transform>())
        {
            hingeJointsL.Add(GameObject.Find(g.name));
        }
        hingeJointsL.RemoveAt(0);
        // foreach (GameObject hinge in hingeJoints)
        // {
            // Debug.Log(hinge.name);
        // }
        currentEulerAngles = fingerJoint.transform.localEulerAngles;
        currentEulerAnglesL = fingerJointL.transform.localEulerAngles;
        // Debug.Log(origVerts.Length);
        ws = new WebSocket("ws://localhost:7000");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            // Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
            //dest = float.Parse(e.Data);
			dataChar = JsonConvert.DeserializeObject<List<List<string>>>(e.Data);
            // Debug.Log(float.Parse(dataChar[0][25]));
            Debug.Log(dataChar.Count);
            if(dataChar.Count<2){
                if(float.Parse(dataChar[0][25]) == 0){
                    data = dataChar[0];
                }
                else{
                    dataL = dataChar[0];
                }
            }
            if(dataChar.Count>1){
                if(float.Parse(dataChar[0][25]) == 0){
                    data = dataChar[0];
                    dataL = dataChar[1];
                }
                else{
                    dataL = dataChar[0];
                    data = dataChar[1];
                }
            }
			qX = float.Parse(data[3]);
			qY = float.Parse(data[4]);
            qZ = float.Parse(data[5]);
            iX = float.Parse(data[0]);
			iY = float.Parse(data[1]);
            iZ = float.Parse(data[2]);
            // qA = float.Parse(data[6]);
            recCommand = float.Parse(data[24]);

            if(dataChar.Count > 1){
                qXL = float.Parse(dataL[3]);
                qYL = float.Parse(dataL[4]);
                qZL = float.Parse(dataL[5]);
                iXL = float.Parse(dataL[0]);
                iYL = float.Parse(dataL[1]);
                iZL = float.Parse(dataL[2]);
                recCommandL = float.Parse(dataL[24]);
                Debug.Log(recCommandL);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(data!= null){
        string[] angles = data.Skip(9).Take(15).ToArray();
        string[] posData = data.Skip(6).Take(3).ToArray();
        // Debug.Log(recCommand);
        if(angles.Length>0){
            for (int i = 0; i < 15; i++) 
            {
                if(i == 6){
                    // Debug.Log(float.Parse(angles[i]));
                    // Debug.Log(hingeJoints[i].name);
                }
                if(i != 12){
                    float angle = Mathf.MoveTowardsAngle(hingeJoints[i].transform.localEulerAngles.z, float.Parse(angles[i]), 200.0f * Time.deltaTime);
                    // hingeJoints[i].transform.localEulerAngles = new Vector3(hingeJoints[i].transform.localEulerAngles.x,hingeJoints[i].transform.localEulerAngles.y,float.Parse(angles[i]));
                    hingeJoints[i].transform.localEulerAngles = new Vector3(hingeJoints[i].transform.localEulerAngles.x, hingeJoints[i].transform.localEulerAngles.y, angle);
                }
            }
        }
        float step = 1.0f * Time.deltaTime;
        // transform.position = anchor.position + new Vector3(-3*qX-0.05f,-qY,0);
        if(qZ<-0.06f){
            // transform.position = anchor.position + new Vector3(-1.5f*qX,-qY,-2*qZ);
            transform.position = Vector3.MoveTowards(transform.position,anchor.position + new Vector3(-1.5f*qX,-qY,-2*qZ),step);
        }else{
            // transform.position = anchor.position + new Vector3(-1.5f*qX,-2.5f*qY,-4*qZ);
            transform.position = Vector3.MoveTowards(transform.position,anchor.position + new Vector3(-1.5f*qX,-2.5f*qY,-4*qZ),step);
        }
        if(qZ<-0.06f){
            // indexPos.transform.position = anchor.position + new Vector3(-1.5f*iX,-iY,-2*iZ);
            indexPos.transform.position = Vector3.MoveTowards(indexPos.transform.position,anchor.position + new Vector3(-1.5f*iX,-iY,-2*iZ),step);
        }else{
            // indexPos.transform.position = anchor.position + new Vector3(-1.5f*iX,-2.5f*iY,-4*iZ);
            indexPos.transform.position = Vector3.MoveTowards(indexPos.transform.position,anchor.position + new Vector3(-1.5f*iX,-2.5f*iY,-4*iZ),step);
        }
        // fingerJoint.transform.localEulerAngles = new Vector3(currentEulerAngles.x,currentEulerAngles.y,qA);
        if(posData.Length>0){
            float stepPos = 0.8f * Time.deltaTime;
            // Debug.Log(float.Parse(posData[1]));
            parentHand.transform.position = Vector3.MoveTowards(parentHand.transform.position,new Vector3(pH.x+(2f*float.Parse(posData[0])),pH.y+2f*(float.Parse(posData[1])),pH.z+0f*(float.Parse(posData[2]))),stepPos);
        }
        
        }



        if(dataL!= null){
       string[] anglesL = dataL.Skip(9).Take(15).ToArray();
        string[] posDataL = dataL.Skip(6).Take(3).ToArray();
        // Debug.Log(recCommand);
        if(anglesL.Length>0){
            for (int i = 0; i < 15; i++) 
            {
                if(i == 6){
                    // Debug.Log(float.Parse(angles[i]));
                    // Debug.Log(hingeJoints[i].name);
                }
                if(i != 12){
                    float angleL = Mathf.MoveTowardsAngle(hingeJointsL[i].transform.localEulerAngles.z, float.Parse(anglesL[i]), 200.0f * Time.deltaTime);
                    // hingeJoints[i].transform.localEulerAngles = new Vector3(hingeJoints[i].transform.localEulerAngles.x,hingeJoints[i].transform.localEulerAngles.y,float.Parse(angles[i]));
                    hingeJointsL[i].transform.localEulerAngles = new Vector3(hingeJointsL[i].transform.localEulerAngles.x, hingeJointsL[i].transform.localEulerAngles.y, angleL);
                }
            }
        }
        float stepL = 1.0f * Time.deltaTime;
        // transform.position = anchor.position + new Vector3(-3*qX-0.05f,-qY,0);
        if(qZ<-0.06f){
            // transform.position = anchor.position + new Vector3(-1.5f*qX,-qY,-2*qZ);
            cubeL.position = Vector3.MoveTowards(cubeL.position,anchorL.position + new Vector3(-1.5f*qXL,-qYL,-2*qZL),stepL);
        }else{
            // transform.position = anchor.position + new Vector3(-1.5f*qX,-2.5f*qY,-4*qZ);
            cubeL.position = Vector3.MoveTowards(cubeL.position,anchorL.position + new Vector3(-1.5f*qXL,-2.5f*qYL,-4*qZL),stepL);
        }
        if(qZ<-0.06f){
            // indexPos.transform.position = anchor.position + new Vector3(-1.5f*iX,-iY,-2*iZ);
            indexPosL.transform.position = Vector3.MoveTowards(indexPosL.transform.position,anchorL.position + new Vector3(-1.5f*iXL,-iYL,-2*iZL),stepL);
        }else{
            // indexPos.transform.position = anchor.position + new Vector3(-1.5f*iX,-2.5f*iY,-4*iZ);
            indexPosL.transform.position = Vector3.MoveTowards(indexPosL.transform.position,anchorL.position + new Vector3(-1.5f*iXL,-2.5f*iYL,-4*iZL),stepL);
        }
        // fingerJoint.transform.localEulerAngles = new Vector3(currentEulerAngles.x,currentEulerAngles.y,qA);
        if(posDataL.Length>0){
            float stepPosL = 1.0f * Time.deltaTime;
            // Debug.Log(float.Parse(posData[1]));
            parentHandL.transform.position = Vector3.MoveTowards(parentHandL.transform.position,new Vector3(pHL.x+2f*(float.Parse(posDataL[0])),pHL.y+2f*(float.Parse(posDataL[1])),pHL.z),stepPosL);
        }
        }

    }

    void OnApplicationQuit()
    {
        ws.Close();
    }
}
