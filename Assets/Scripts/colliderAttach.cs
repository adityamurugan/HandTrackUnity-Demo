using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class colliderAttach : MonoBehaviour
{
    private bool collided;
    public GameObject command;
    private RotatePitch receivedCommand;
    public TextMeshProUGUI my_text;
    private bool rotationStatus;
    private GameObject pills;
    private GameObject otherButton;
    private GameObject midFinger;
    private GameObject wheel;
    private GameObject buttonPressed;
    //Detect collisions between the GameObjects with Colliders attached
    void Start(){
        midFinger = GameObject.Find("parentHand/SK_VR_Latex_Glove_R/VRHand_Right/hand_r/index_01_r");
        wheel = GameObject.Find("Wheel");
        rotationStatus = false;
        my_text.text = "Stopped";
        receivedCommand  = command.GetComponent<RotatePitch>();
    }
    void OnTriggerEnter(Collider  collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject

            if (collision.gameObject.tag == "pick")
            {
                collided =true;
                pills = collision.gameObject;
                // pills.transform.eulerAngles = new Vector3(-180,-90,0);
                pills.transform.SetParent(gameObject.transform);
                pills.GetComponent<Rigidbody>().useGravity = false;
                //If the GameObject's name matches the one you suggest, output this message in the console
            }


        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "press")
        {
            buttonPressed = collision.gameObject;
            //If the GameObject has the same tag as specified, output this message in the console
            if(buttonPressed.transform.root.name == "START"){
                otherButton = GameObject.Find("EMERGENCY/Cylinder001");
                if(!rotationStatus){
                    buttonPressed.transform.localPosition += new Vector3(0, -0.025f, 0);
                    otherButton.transform.localPosition += new Vector3(0, 0.025f, 0);
                }   
                rotationStatus = true;
            }
            if(buttonPressed.transform.root.name == "EMERGENCY"){
                otherButton = GameObject.Find("START/Cylinder001");
                if(rotationStatus){
                    buttonPressed.transform.localPosition += new Vector3(0, -0.025f, 0);
                    otherButton.transform.localPosition += new Vector3(0, 0.025f, 0);
                }                
                rotationStatus = false;
            }
        }
    }
 void Update(){
    Debug.Log(receivedCommand.recCommand);
    if(rotationStatus){
        wheel.transform.Rotate(new Vector3(0, 0, -40) * Time.deltaTime);
        my_text.text = "Running";
    }
    else{
        wheel.transform.Rotate(new Vector3(0, 0, 0) * Time.deltaTime);
        my_text.text = "Stopped";
    }
    if(midFinger.transform.eulerAngles.z<180){
     if(midFinger.transform.eulerAngles.z>40){
        collided = false;
        if(pills){
            pills.transform.SetParent(null);
            pills.GetComponent<Rigidbody>().useGravity = true;
        }
     }
     }
     if (collided){
           pills.transform.localEulerAngles = new Vector3(-180,0,0);
           pills.transform.localPosition = new Vector3(-0.09f,-0.05f,0);
        //    pills.transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z);
    
     }
    //  Debug.Log(midFinger.transform.eulerAngles.z);
    }
 }
     