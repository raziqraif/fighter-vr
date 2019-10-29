using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    public GameObject kunai;
    public GameObject shuriken;
    public GameObject weaponsHideLocation;

    // Kunai setup
    public GameObject[] kunais;
    const int MAX_KUNAIS = 18;
    public int currentKunai;

    // Shuriken setup
    public GameObject[] shurikens;
    const int MAX_SHURIKENS = 10;
    public int currentShuriken;

    // Trigger
    public SteamVR_Action_Boolean m_GrabAction = null;

    // Middle Button
    public SteamVR_Action_Boolean m_MidButton = null;

    // Controller stuff
    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    
    // Start is called before the first frame update
    private void Awake()
    {
        // set up kunais
        currentKunai = 0;
        kunais = new GameObject[MAX_KUNAIS];
        for (int i = 0; i < MAX_KUNAIS; i++)
        {
            kunais[i] = Instantiate(kunai, kunai.transform.position, kunai.transform.rotation);
        }

        print("Init Kun Length: " + kunais.Length);

        // set up Shuriken
        currentKunai = 0;

        shurikens = new GameObject[MAX_SHURIKENS];
        for (int i = 0; i < MAX_SHURIKENS; i++)
        {
            shurikens[i] = Instantiate(shuriken, shuriken.transform.position, shuriken.transform.rotation);
        }
    

        print("Init Kun Length: " + kunais.Length);
        print("Init Shur Length: " + shurikens.Length);

        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    private void Update()
    {
        // trigger down
        if (m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + " Trigger Down");
            Aim();
        }

        // Mid button down
        if (m_MidButton.GetStateDown(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + " MidButton Down");
            holdShuriken();
        }

        // trigger up
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + " Trigger Up");
            Shoot();
        }

        // Mid button up
        if (m_MidButton.GetStateUp(m_Pose.inputSource))
        {
            print(m_Pose.inputSource + " MidButton Up");
            throwShuriken();
        }

    }

   
    public void Aim()
    {
        // returns kunai # we're on and set curr kunai var | Increment KunaiCount
        currentKunai = getKunaiCount();

        print("CurrKunai: " + currentKunai);
        print("Length: " + kunais.Length);

        // Attaches kunai to hand
        kunais[currentKunai].transform.position = transform.position;
        kunais[currentKunai].transform.rotation = transform.rotation;

        kunais[currentKunai].transform.Rotate(90, 0, 0);

        Debug.Log(transform.eulerAngles);

        Rigidbody targetBody = kunais[currentKunai].GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;
    }

    public void Shoot()
    {
        //Vector3 fwd = transform.TransformDirection(Vector3.forward);

        Rigidbody targetBody = kunais[currentKunai].GetComponent<Rigidbody>();

        targetBody.velocity = transform.TransformDirection(Vector3.forward) * 20;

        //targetBody.AddRelativeForce(transform.TransformDirection(Vector3.forward));

        m_Joint.connectedBody = null;

        currentKunai++;
    }


    // Returns what Kunai Number we are on
    public int getKunaiCount()
    {
        if (currentKunai < kunais.Length)
        {
            return currentKunai;
        } else
        {
            currentKunai = 0;
            return 0;
        }
        
    }

    public void holdShuriken()
    {
        // returns kunai # we're on and set curr kunai var | Increment KunaiCount
        currentShuriken = getShurikenCount();

        print("currShur: " + currentShuriken);
        print("Length: " + shurikens.Length);

        // Attaches kunai to hand
        shurikens[currentShuriken].transform.position = transform.position;
        shurikens[currentShuriken].transform.rotation = transform.rotation;

        shurikens[currentShuriken].transform.Rotate(90, 0, 0);


        Debug.Log(transform.eulerAngles);

        Rigidbody targetBody = shurikens[currentShuriken].GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;
    }

    public void throwShuriken()
    {
        
        Rigidbody targetBody = shurikens[currentShuriken].GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity() * 5.0f;
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();

        
        m_Joint.connectedBody = null;

        currentShuriken++;
    }

    public int getShurikenCount()
    {
        if (currentShuriken < shurikens.Length)
        {
            return currentShuriken;
        }
        else
        {
            currentShuriken = 0;
            return 0;
        }

    }

}
