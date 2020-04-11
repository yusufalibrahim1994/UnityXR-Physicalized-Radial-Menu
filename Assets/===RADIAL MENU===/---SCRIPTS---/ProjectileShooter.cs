using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ProjectileShooter : MonoBehaviour
{
    public Transform spawnLocator;
    public Transform spawnLocatorMuzzleFlare;
    public Transform shellLocator;
    //public Animator recoilAnimator;

    public Transform[] shotgunLocator;

    private Interactable interactable;

    public SteamVR_Action_Boolean shootAction;
    public SteamVR_Action_Boolean nextBullet;
    public SteamVR_Action_Boolean previousBullet;


    public new PlaySound audio;

    public bool firing;
    float firingTimer;
    public int projectileType = 0;
    [System.Serializable]
    public class projectile
    {
        public string name;
        public Rigidbody bombPrefab;
        public GameObject muzzleflare;
        public float min, max;
        public bool rapidFire;
        public float rapidFireCooldown;

        public bool shotgunBehavior;
        public int shotgunPellets;
        public GameObject shellPrefab;
        public bool hasShells;
    }
    public projectile[] projectileList;

    public bool Torque = false;
    public float Tor_min, Tor_max;

    public bool MinorRotate;
    public bool MajorRotate = false;
    int seq = 0;

    private void Start()
    {
        if (interactable == null)
            interactable = this.GetComponentInParent<Interactable>();
    }

    private void Update()
    {
        if (interactable.attachedToHand)
        {
            if (shootAction.GetStateDown(interactable.attachedToHand.handType))
            {
                firing = true;
                Shoot();
            }
            if (shootAction.GetStateUp(interactable.attachedToHand.handType))
            {
                firing = false;
                firingTimer = 0;
            }
            if(nextBullet.GetStateDown(interactable.attachedToHand.handType))
            {
                Switch(-1);
            }
            if (previousBullet.GetStateDown(interactable.attachedToHand.handType))
            {
                Switch(1);
            }
        }
    }

    private void Shoot()
    {
        audio.Play();
        Instantiate(projectileList[projectileType].muzzleflare, spawnLocatorMuzzleFlare.position, spawnLocatorMuzzleFlare.rotation);
        //   bombList[bombType].muzzleflare.Play();

        if (projectileList[projectileType].hasShells)
        {
            Instantiate(projectileList[projectileType].shellPrefab, shellLocator.position, shellLocator.rotation);
        }
        //recoilAnimator.SetTrigger("recoil_trigger");

        Rigidbody rocketInstance;
        rocketInstance = Instantiate(projectileList[projectileType].bombPrefab, spawnLocator.position, spawnLocator.rotation) as Rigidbody;
        // Quaternion.Euler(0,90,0)
        rocketInstance.AddForce(spawnLocator.forward * Random.Range(projectileList[projectileType].min, projectileList[projectileType].max));

        if (projectileList[projectileType].shotgunBehavior)
        {
            for (int i = 0; i < projectileList[projectileType].shotgunPellets; i++)
            {
                Rigidbody rocketInstanceShotgun;
                rocketInstanceShotgun = Instantiate(projectileList[projectileType].bombPrefab, shotgunLocator[i].position, shotgunLocator[i].rotation) as Rigidbody;
                // Quaternion.Euler(0,90,0)
                rocketInstanceShotgun.AddForce(shotgunLocator[i].forward * Random.Range(projectileList[projectileType].min, projectileList[projectileType].max));
            }
        }

        if (Torque)
        {
            rocketInstance.AddTorque(spawnLocator.up * Random.Range(Tor_min, Tor_max));
        }
        if (MinorRotate)
        {
            RandomizeRotation();
        }
        if (MajorRotate)
        {
            Major_RandomizeRotation();
        }
    }

    public void Switch(int value)
    {
        projectileType += value;
        if (projectileType < 0)
        {
            projectileType = projectileList.Length;
            projectileType--;
        }
        else if (projectileType >= projectileList.Length)
        {
            projectileType = 0;
        }
    }

    void RandomizeRotation()
    {
        if (seq == 0)
        {
            seq++;
            transform.Rotate(0, 1, 0);
        }
        else if (seq == 1)
        {
            seq++;
            transform.Rotate(1, 1, 0);
        }
        else if (seq == 2)
        {
            seq++;
            transform.Rotate(1, -3, 0);
        }
        else if (seq == 3)
        {
            seq++;
            transform.Rotate(-2, 1, 0);
        }
        else if (seq == 4)
        {
            seq++;
            transform.Rotate(1, 1, 1);
        }
        else if (seq == 5)
        {
            seq = 0;
            transform.Rotate(-1, -1, -1);
        }
    }

    void Major_RandomizeRotation()
    {
        if (seq == 0)
        {
            seq++;
            transform.Rotate(0, 25, 0);
        }
        else if (seq == 1)
        {
            seq++;
            transform.Rotate(0, -50, 0);
        }
        else if (seq == 2)
        {
            seq++;
            transform.Rotate(0, 25, 0);
        }
        else if (seq == 3)
        {
            seq++;
            transform.Rotate(25, 0, 0);
        }
        else if (seq == 4)
        {
            seq++;
            transform.Rotate(-50, 0, 0);
        }
        else if (seq == 5)
        {
            seq = 0;
            transform.Rotate(25, 0, 0);
        }
    }
}