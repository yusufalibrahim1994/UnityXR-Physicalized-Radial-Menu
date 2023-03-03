using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RadialMenuXR : MonoBehaviour
{
    [Header("RADIAL MENU")]

    [SerializeField] private GameObject[] menuPlaceholderItems;
    [SerializeField] private GameObject[] correspondingXRinteractables;

    [SerializeField] private float menuRadius = 1.5f;
    [SerializeField] private XRDirectInteractor directInteractor;


    //public TextMeshProUGUI debugOutput;
    public Transform temporaryParent;
    public Transform targetParent;
    public GameObject optionContainer;
    [SerializeField] Vector3[] initialMenuItemsTransform;
    [SerializeField][Range (0.05f, 1)] float inverseScale = 0.1f;
    [SerializeField] [Range(0.05f, 1)] float outlineGrabThresholdDistance = 0.05f;
    public XRInteractionManager interactionManager;
    private bool isHapticDone = false;
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private GameObject particles;
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip selectClip;

    private bool allowSelection = false;
    private GameObject selectionGO;
    private Transform selectionTransform;

    private bool isSelectedDone = true;
    #region Input
    private XRIDefaultInputActions inputActions;

    private void Awake()
    {
        inputActions = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnThumbstickPressStart(InputAction.CallbackContext context)
    {
        isSelectedDone = true;
        optionContainer.SetActive(true);
        transform.parent = temporaryParent;
        actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Select").Enable();
    }

    public void OnThumbstickPressEnd(InputAction.CallbackContext context)
    {
        transform.parent = targetParent;
        transform.localPosition = targetParent.localPosition;
        transform.localRotation = targetParent.localRotation;

        optionContainer.SetActive(false);
        isSelectedDone = false;
        particles.SetActive(false);

    }
    #endregion


    #region Built-in methods

    private void Start()
    {
        inputActions.XRIRightHandInteraction.OpenMenu.performed += OnThumbstickPressStart;
        inputActions.XRIRightHandInteraction.CloseMenu.performed += OnThumbstickPressEnd;
        InstantiateCircle();
        optionContainer.SetActive(false);

        for(int i = 0; i < menuPlaceholderItems.Length; i++)
        {
            initialMenuItemsTransform[i] = new Vector3 (menuPlaceholderItems[i].transform.localScale.x, menuPlaceholderItems[i].transform.localScale.y, menuPlaceholderItems[i].transform.localScale.z);
        }
    }


    private void Update()
    {
        for(int i = 0; i < menuPlaceholderItems.Length; i++)
        {
            if(menuPlaceholderItems[i].activeInHierarchy)
            {
                float HandToObjectDistance = Vector3.Distance(targetParent.transform.position, menuPlaceholderItems[i].transform.position);

                float inverseDistanceBetweenTransforms = 1/ HandToObjectDistance;
                menuPlaceholderItems[i].transform.localScale = new Vector3(initialMenuItemsTransform[i].x * inverseDistanceBetweenTransforms * inverseScale, initialMenuItemsTransform[i].y * inverseDistanceBetweenTransforms * inverseScale, initialMenuItemsTransform[i].z * inverseDistanceBetweenTransforms * inverseScale);

                if (HandToObjectDistance < outlineGrabThresholdDistance)
                {
                    Haptic(menuPlaceholderItems[i].GetComponent<Outline>().enabled);
                    menuPlaceholderItems[i].GetComponent<Outline>().enabled = true;

                    allowSelection = menuPlaceholderItems[i].GetComponent<Outline>().enabled;
                    selectionGO = ReturnGameobject(correspondingXRinteractables[i]);
                    //SelectMenuItem(correspondingXRinteractables[i].gameObject, menuItems[i].transform);

                    actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Select").Disable();
                }
                else
                {
                    correspondingXRinteractables[i].SetActive(false);
                    menuPlaceholderItems[i].GetComponent<Outline>().enabled = false;


                }

            }
            if(allowSelection)
            {
                SelectMenuItem();
            }

        }

    }


    #endregion

    private void InstantiateCircle()
    {
        float angle = 360f / (float)menuPlaceholderItems.Length;
        for (int i = 0; i < menuPlaceholderItems.Length; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(i * angle, Vector3.up);
            Vector3 direction = rotation * Vector3.forward;

            Vector3 position = targetParent.position + (direction * menuRadius);
            menuPlaceholderItems[i].transform.localPosition = position;
            menuPlaceholderItems[i].transform.localRotation = rotation;

        }
    }

    public void SelectMenuItem()
    {
        if (isSelectedDone == false)
        {
            selectionGO.SetActive(true);
            interactionManager.ForceSelect(directInteractor, selectionGO.GetComponent<XRGrabInteractable>());
            particles.transform.position = directInteractor.transform.position;
            particles.SetActive(true);
            AudioSource.PlayClipAtPoint(selectClip, transform.position);
            allowSelection = false;
            selectionGO = null;
        }

    }
    public GameObject ReturnGameobject(GameObject g)
    {
        return g;
    }
    public void Haptic(bool b)
    {
        if(b == false)
        {
            directInteractor.xrController.SendHapticImpulse(0.1f, 0.4f);
            AudioSource.PlayClipAtPoint(hoverClip, transform.position);
        }
    }

    public void CustomDebug(string s)
    {
        Debug.Log(s);
    }

    public void PlayClip(AudioClip c)
    {
        AudioSource.PlayClipAtPoint(c, transform.position);
    }


}