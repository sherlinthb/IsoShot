using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

    public Transform weaponHold;
    public Gun startingGun;
    public Gun shotgun;
    public Gun rifle;
    public Gun machinegun;
    public Gun handGun;
    Gun equippedGun;

    void Start()
    {
        if(startingGun != null)
        {
            EquipGun(startingGun);
        }
        
    }
    public void EquipGun(Gun gunToEquip)
    {
        if(equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
            equippedGun = Instantiate(gunToEquip, weaponHold.position,weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void EnemyShooting()
    {
        if (equippedGun != null)
        {
            equippedGun.EnemyShoot();
        }
        
    }

    public void equipShotgun()
    {
        EquipGun(shotgun);
    }

    public void equipRifle()
    {
        EquipGun(rifle);
    }

    public void equipMachinegun()
    {
        EquipGun(machinegun);
    }

    public void equipHandgun()
    {
        EquipGun(handGun);
    }

    public void OnTriggerHold()
    {
        if(equippedGun != null)
        {
            equippedGun.OnTriggerHold();
        }
    }
    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }

    public float GunHeight { 
    get
        {
            return weaponHold.position.y;
        }
    }

    public void Aim(Vector3 aimPoint)
    {

        if (equippedGun != null)
        {
            equippedGun.Aim(aimPoint);
        }
    }

    public void Reload()
    {
        if (equippedGun != null)
        {
            equippedGun.Reload();
        }
    }
}
