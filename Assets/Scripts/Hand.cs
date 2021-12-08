using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    public SteamVR_Action_Boolean m_grabAction = null;

    private SteamVR_Behaviour_Pose m_pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    private List<Interactable> m_ContactInteractable = new List<Interactable>();

    private void Awake()
    {
        m_pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Down
        if(m_grabAction.GetStateDown(m_pose.inputSource))
        {
            print(m_pose.inputSource + " Trigger Down");
            Pickup();
        }
        // Up
        if (m_grabAction.GetStateUp(m_pose.inputSource))
        {
            print(m_pose.inputSource + " Trigger Up");
            Pickup();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Interactable"))
        {
            return;
        }

        m_ContactInteractable.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {

    }

    public void Pickup()
    {
        // Get nearest
        m_CurrentInteractable = GetNearestInteractable();

        if (!m_CurrentInteractable)
            return;

        // already held check
        if (m_CurrentInteractable.m_ActiveHand)
        {
            m_CurrentInteractable.m_ActiveHand.Drop();
        }

        // position to controller
        m_CurrentInteractable.transform.position = transform.position;

        // attach to controller
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        // set active hand
        m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {
        if (!m_CurrentInteractable)
            return;

        // give velocity
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_pose.GetVelocity();
        targetBody.angularVelocity = m_pose.GetAngularVelocity();

        // detach
        m_Joint.connectedBody = null;

        // clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach(Interactable interactable in m_ContactInteractable)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
    }
}
