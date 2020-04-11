using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class FlashLightVR : MonoBehaviour
{
    public Player player;
    public Interactable interactable;

    public enum ButtonDirection
    {
        BlueAxis,
        RedAxis,
        GreenAxis,
        ReverseBlueAxis,
        ReverseRedAxis,
        ReverseGreenAxis
    }

    public bool turnOffOnAwake;
    public SteamVR_Action_Boolean toggleButton = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Flashlight", "TurnOnOff");
    public Light[] lightSources;

    [Header("Blob Emission")]
    public Renderer blob;
    public float lightOffEmission = 0.0f;
    public float lightOnEmission = 8.0f;

    [Header("Button Displacement")]
    public Transform button;
    [Range(0, 1)]
    public float displacement = 0.02f;
    public ButtonDirection upDirection;

    [Header("Toggle Sound")]
    public AudioSource sfx;

    private bool active = true;
    private float[] startIntensity;
    private Vector3 origin;
    private Vector3 pressed;
    private Hand _hand;
    public Hand Hand
    {
        get
        {
            if (_hand == null)
            {
                _hand = GetComponentInParent<Hand>();
                return _hand;
            }
            else
            {
                return _hand;
            }
        }
    }


    //-------------------------------------------------
    private void Awake()
    {
        SetOriginLightsIntensity();
        SetOriginButtonPosition();

        if (turnOffOnAwake)
            TurnOff();
    }

    private void Update()
    {
        if (interactable.attachedToHand)
        {
            if (toggleButton.GetStateDown(interactable.attachedToHand.handType))
            {
                if (sfx)
                    PlaySoundPressed();

                if (button)
                    button.localPosition = pressed;
            }
            if (toggleButton.GetStateUp(interactable.attachedToHand.handType))
            {
                if (button)
                    button.localPosition = origin;

                if (sfx)
                    PlaySoundReleased();

                Toggle();
            }
        }
    }

    //-------------------------------------------------
    [ContextMenu("Turn Off")]
    public void TurnOff()
    {
        active = false;

        for (int i = 0; i < lightSources.Length; i++)
            lightSources[i].intensity = 0;

        if (blob)
        {
            Material mat = blob.material;
            Color baseColor = mat.GetColor("_EmissionColor");
            Color finalColor = baseColor * Mathf.LinearToGammaSpace(lightOffEmission);
            mat.SetColor("_EmissionColor", finalColor);
        }
    }

    [ContextMenu("Turn On")]
    public void TurnOn()
    {
        active = true;

        for (int i = 0; i < lightSources.Length; i++)
            lightSources[i].intensity = startIntensity[i];

        if (blob)
        {
            Material mat = blob.material;
            Color baseColor = mat.GetColor("_EmissionColor");
            Color finalColor = baseColor * Mathf.LinearToGammaSpace(lightOnEmission);
            mat.SetColor("_EmissionColor", finalColor);
        }
    }

    [ContextMenu("Toggle")]
    public void Toggle()
    {
        active = !active;

        if (active)
            TurnOn();
        else
            TurnOff();
    }

    //-------------------------------------------------
    private void SetOriginButtonPosition()
    {
        origin = button.transform.localPosition;

        Vector3 pressedDir = new Vector3();

        switch (upDirection)
        {
            case ButtonDirection.BlueAxis:
                pressedDir = button.transform.forward * -1;
                break;
            case ButtonDirection.RedAxis:
                pressedDir = button.transform.right * -1;
                break;
            case ButtonDirection.GreenAxis:
                pressedDir = button.transform.up * -1;
                break;
            case ButtonDirection.ReverseBlueAxis:
                pressedDir = button.transform.forward;
                break;
            case ButtonDirection.ReverseRedAxis:
                pressedDir = button.transform.right;
                break;
            case ButtonDirection.ReverseGreenAxis:
                pressedDir = button.transform.up;
                break;
        }

        pressed = origin + pressedDir.normalized * displacement;
    }

    private void SetOriginLightsIntensity()
    {
        startIntensity = new float[lightSources.Length];

        for (int i = 0; i < lightSources.Length; i++)
            startIntensity[i] = lightSources[i].intensity;
    }

    private void PlaySoundPressed()
    {
        sfx.pitch = 1.2f;
        sfx.volume = 1.0f;
        sfx.Play();
    }

    private void PlaySoundReleased()
    {
        sfx.pitch = 0.9f;
        sfx.volume = 0.5f;
        sfx.Play();
    }
}
