using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GravityGloves : MonoBehaviour
{
    [Header("VR PLAYER STUFF")]
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    public Player player;
    private SteamVR_Input_Sources in_source_buffer;

    [Header("PHYSICS OF GRABBING")]
    public Spell current_spell;
    public enum Spell
    {
        ATTRACT
    }
    [Header("CONTROLLER BINDINGS")]
    public SteamVR_Action_Boolean grabObject;
    public SteamVR_Action_Pose leftControllerPose;
    public SteamVR_Action_Pose rightControllerPose;

    [Header("RAYCAST STUFF")]
    public Transform gravity_tracker;
    public Material highlightedMaterial;
    public Material defaultMaterial;
    private Transform leftSelectionTransform;
    private Transform rightSelectionTransform;
    private Transform attr_left_target;
    private Transform attr_right_target;
    private ComplementarCalculations complementarCalculations;


    void Update()
    {
        //Before highlighting, save the default material
        if(leftSelectionTransform != null)
        {
            var leftSelectionRenderer = leftSelectionTransform.GetComponent<Renderer>();
            leftSelectionRenderer.material = defaultMaterial;
            leftSelectionTransform = null;
        }
        if (rightSelectionTransform != null)
        {
            var rightSelectionRenderer = rightSelectionTransform.GetComponent<Renderer>();
            rightSelectionRenderer.material = defaultMaterial;
            rightSelectionTransform = null;
        }
        //What is each hand pointing to?
        RaycastHit leftHandRaycastHit;
        Physics.Raycast(leftHand.transform.position, leftHand.forward, out leftHandRaycastHit, 150);
        RaycastHit rightHandRaycastHit;
        Physics.Raycast(rightHand.transform.position, rightHand.forward, out rightHandRaycastHit, 150);

        //Highlighting
        var leftHighlight = leftHandRaycastHit.transform;
        if (leftHandRaycastHit.transform.tag == "ALLOW_MANIPULATION")
        {
            var leftSelectionRenderer = leftHighlight.GetComponent<Renderer>();
            if (leftSelectionRenderer != null)
                leftSelectionRenderer.material = highlightedMaterial;
            leftSelectionTransform = leftHighlight;
        }
        var rightHighlight = rightHandRaycastHit.transform;
        if (rightHandRaycastHit.transform.tag == "ALLOW_MANIPULATION")
        {
            var rightSelectionRenderer = rightHighlight.GetComponent<Renderer>();
            if (rightSelectionRenderer != null)
                rightSelectionRenderer.material = highlightedMaterial;
            rightSelectionTransform = rightHighlight;
        }

        //Grabbing code
        switch (current_spell)
        {
            case Spell.ATTRACT:
            {
                in_source_buffer = player.leftHand.handType;

                //If button pressed, change attraction target
                if(grabObject.GetState(SteamVR_Input_Sources.LeftHand) && leftHandRaycastHit.transform.tag == "ALLOW_MANIPULATION")
                {
                    attr_left_target = leftHandRaycastHit.transform;
                    //Move and start particle system
                    Transform tracker = Instantiate(gravity_tracker, attr_left_target.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    tracker.SetParent(attr_left_target);
                }
                //Check for flick of left hand
                //Debug.Log(Vector3.Dot(leftControllerPose[in_source_buffer].velocity.normalized, (Camera.main.transform.forward - Camera.main.transform.up).normalized));
                if (attr_left_target != null && Vector3.Dot(leftControllerPose[in_source_buffer].velocity, (Camera.main.transform.forward - Camera.main.transform.up)) < -0.5)
                {
                    //Calculate velocity and apply it to the target
                    Vector3 calculate_velocity = ComplementarCalculations.CalculateParabola(attr_left_target.transform.position, leftHand.transform.position);
                    attr_left_target.GetComponent<Rigidbody>().velocity = calculate_velocity;
                    //Destroy particle system
                    Destroy(attr_left_target.GetChild(0).gameObject);
                    attr_left_target = null;
                }

                in_source_buffer = player.rightHand.handType;

                //If button pressed, change attraction target
                if (grabObject.GetState(SteamVR_Input_Sources.RightHand) && rightHandRaycastHit.transform.tag == "ALLOW_MANIPULATION")
                {
                    attr_right_target = rightHandRaycastHit.transform;
                    //Move and start particle system
                    Transform tracker = Instantiate(gravity_tracker, attr_right_target.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    tracker.SetParent(attr_right_target);
                }
                //Check for flick of right hand
                if (attr_right_target != null && Vector3.Dot(rightControllerPose[in_source_buffer].velocity, (Camera.main.transform.forward - Camera.main.transform.up)) < -0.5)
                {
                    //Calculate velocity and apply it to the target
                    Vector3 calculate_velocity = ComplementarCalculations.CalculateParabola(attr_right_target.transform.position, rightHand.transform.position);
                    attr_right_target.GetComponent<Rigidbody>().velocity = calculate_velocity;
                    //Destroy particle system
                    Destroy(attr_right_target.GetChild(0).gameObject);
                    attr_right_target = null;
                }
                break;
            }
        }
    }
}
