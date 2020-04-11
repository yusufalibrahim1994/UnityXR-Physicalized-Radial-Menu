using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class InputManager : MonoBehaviour
{

    Animator anim;

    [HideInInspector]
    public bool menuOpen;
    bool animating;



    [Header("BUTTON TO OPEN MENU")]
    public SteamVR_Action_Boolean action_menu;
    public bool automaticMenuClosing = true;
    public IconManager iconManager;
    public Hand righthandScript;
    public Hand lefthandScript;
    public Player player;

    [Header("MENU POSITION TO BE SPAWNED IN")]
    public float menuDistanceFromHand = 0;
    public Transform menuCenter;
    public Transform headLocation;

    [Header("MENU ROTATION WHEN SPAWNED")]
    public MenuRotationFollowWhat menuRotationFollowWhat;
    public enum MenuRotationFollowWhat
    {
        Quaternion,
        HandY,
        HandXY,
        HandXYZ
    }
    public float menuFixedXAngle = 0;

    [Header("ITEMS INSIDE MENU")]
    public SciFiPistolSpawner sciFiPistolSpawner;
    public SteamVR_Action_Boolean releaseSciFiPistol;
    public ItemPackageSpawner flashlightSpawner;
    public SteamVR_Action_Boolean releaseFlashlight;
    public ItemPackageSpawner bowSpawner;



    //public float menuAngle = 60f;

    /*public AudioClip au_menuOpen;
    public AudioClip au_menuClose;
    AudioSource audioSource;*/

    private void Start()
    {
        anim = iconManager.GetComponent<Animator>();
        iconManager.gameObject.SetActive(false);
        iconManager.interactable = false;

        /*audioSource = new GameObject("menuAudio").AddComponent<AudioSource>();
        audioSource.transform.parent = transform;
        audioSource.spatialBlend = 1;
        audioSource.spatialize = true;
        audioSource.minDistance = 100;
        audioSource.dopplerLevel = 0;*/
    }


    private void Update()
    {
        //OPENING AND CLOSING THE RADIAL MENU//
        if (automaticMenuClosing)
        {
            if (!animating & !iconManager.loading)
            {
                if (!menuOpen)
                {
                    if (player.leftHand.currentAttachedObject == null && action_menu.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        OpenMenu();
                    }
                    if (player.rightHand.currentAttachedObject == null && action_menu.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        OpenMenu();
                    }
                }
                else
                {
                    // if menu button is pressed or player looks away from menu
                    if (action_menu.GetStateUp(SteamVR_Input_Sources.Any) /*|| Vector3.Dot(player.hmdTransform.forward, (menuCenter.position - player.hmdTransform.position).normalized) < 0.2*/)
                    {
                        CloseMenu();
                    }
                }
            }
        }
        else
        {
            if (!animating & !iconManager.loading)
            {
                if (!menuOpen)
                {
                    if (player.leftHand.currentAttachedObject == null && action_menu.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        OpenMenu();
                    }
                    if (player.rightHand.currentAttachedObject == null && action_menu.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        OpenMenu();
                    }
                }
                else
                {
                    // if menu button is pressed or player looks away from menu
                    if (action_menu.GetStateDown(SteamVR_Input_Sources.Any) || Vector3.Dot(player.hmdTransform.forward, (menuCenter.position - player.hmdTransform.position).normalized) < 0.2)
                    {
                        CloseMenu();
                    }
                }
            }
        }

        //MANAGING THE ITEMS INSIDE THE RADIAL MENU//

        //PISTOL GUN
        if (player.rightHand.currentAttachedObject != null)
        {

            ItemPackage currentAttachedItemPackage = sciFiPistolSpawner.GetAttachedItemPackage(righthandScript);

            if (currentAttachedItemPackage == sciFiPistolSpawner.itemPackage) // the item at the top of the hand's stack has an associated ItemPackage
            {
                if (sciFiPistolSpawner.takeBackItem && sciFiPistolSpawner.requireReleaseActionToReturn) // if we want to take back matching items and aren't waiting for a trigger press
                {
                    if (releaseSciFiPistol.GetStateDown(SteamVR_Input_Sources.RightHand) || releaseSciFiPistol.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        sciFiPistolSpawner.TakeBackItem(righthandScript);
                    }
                }
            }
        }

        if (player.leftHand.currentAttachedObject != null)
        {

            ItemPackage currentAttachedItemPackage = sciFiPistolSpawner.GetAttachedItemPackage(lefthandScript);

            if (currentAttachedItemPackage == sciFiPistolSpawner.itemPackage) // the item at the top of the hand's stack has an associated ItemPackage
            {
                if (sciFiPistolSpawner.takeBackItem && sciFiPistolSpawner.requireReleaseActionToReturn) // if we want to take back matching items and aren't waiting for a trigger press
                {
                    if (releaseSciFiPistol.GetStateDown(SteamVR_Input_Sources.RightHand) || releaseSciFiPistol.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        sciFiPistolSpawner.TakeBackItem(lefthandScript);
                    }
                }
            }
        }

        //FLASHLIGHT
        if (player.rightHand.currentAttachedObject != null)
        {

            ItemPackage currentAttachedItemPackage = flashlightSpawner.GetAttachedItemPackage(righthandScript);

            if (currentAttachedItemPackage == flashlightSpawner.itemPackage) // the item at the top of the hand's stack has an associated ItemPackage
            {
                if (flashlightSpawner.takeBackItem && flashlightSpawner.requireReleaseActionToReturn) // if we want to take back matching items and aren't waiting for a trigger press
                {
                    if (releaseFlashlight.GetStateDown(SteamVR_Input_Sources.RightHand) || releaseFlashlight.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        flashlightSpawner.TakeBackItem(righthandScript);
                    }
                }
            }
        }

        if (player.leftHand.currentAttachedObject != null)
        {

            ItemPackage currentAttachedItemPackage = flashlightSpawner.GetAttachedItemPackage(lefthandScript);

            if (currentAttachedItemPackage == flashlightSpawner.itemPackage) // the item at the top of the hand's stack has an associated ItemPackage
            {
                if (flashlightSpawner.takeBackItem && flashlightSpawner.requireReleaseActionToReturn) // if we want to take back matching items and aren't waiting for a trigger press
                {
                    if (releaseFlashlight.GetStateDown(SteamVR_Input_Sources.RightHand) || releaseFlashlight.GetStateDown(SteamVR_Input_Sources.LeftHand))
                    {
                        flashlightSpawner.TakeBackItem(lefthandScript);
                    }
                }
            }
        }
    }

    public void OpenMenu()
    {
        Vector3 lookDir = player.hmdTransform.forward;
        lookDir.y = 0;
        lookDir = lookDir.normalized;

        //POSITION OF MENU SPAWNING IN FRONT OF HAND
        Vector3 spawnPos = new Vector3(menuCenter.position.x - menuDistanceFromHand, menuCenter.position.y, menuCenter.position.z); //player.feetPositionGuess + lookDir * player.scale * spawnDistance;
        iconManager.transform.position = spawnPos;


        //ROTATION OF MENU FOLLOWS HAND VS FIXED
        switch (menuRotationFollowWhat)
        {
            case MenuRotationFollowWhat.Quaternion:
            {
                iconManager.transform.rotation = Quaternion.Slerp(iconManager.transform.rotation, menuCenter.rotation, 1);
                break;
            }
            case MenuRotationFollowWhat.HandY:
            {
                iconManager.transform.rotation = Quaternion.Euler(menuFixedXAngle +19, menuCenter.transform.rotation.eulerAngles.y, 0);
                break;
            }

            case MenuRotationFollowWhat.HandXY:
            {
                iconManager.transform.rotation = Quaternion.Euler(menuCenter.transform.rotation.eulerAngles.x +19, menuCenter.transform.rotation.eulerAngles.y, 0);
                break;
            }

            case MenuRotationFollowWhat.HandXYZ:
            {
                iconManager.transform.rotation = Quaternion.Euler(menuCenter.transform.rotation.eulerAngles.x +19, menuCenter.transform.rotation.eulerAngles.y, menuCenter.transform.rotation.eulerAngles.z);
                break;
            }
        }

        iconManager.gameObject.SetActive(true);
        menuOpen = true;
        iconManager.interactable = true;


        /*audioSource.transform.position = menuCenter.position;
        audioSource.PlayOneShot(au_menuOpen);*/
        //StartCoroutine(AnimMenuOpen());
    }

    /*IEnumerator AnimMenuOpen()
    {
        animating = true;
        yield return new WaitForSeconds(0.5f);
        animating = false;
    }*/

    public void CloseMenu()
    {
        iconManager.gameObject.SetActive(false);
        iconManager.interactable = false;

        menuOpen = false;
        //audioSource.PlayOneShot(au_menuClose);
        //StartCoroutine(AnimMenuClose());
    }

    /*IEnumerator AnimMenuClose()
    {
        animating = true;
        yield return new WaitForSeconds(0.5f);
        animating = false;
    }*/
}
