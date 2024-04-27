using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyGun : Gun
{
    // Start is called before the first frame update
    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;

        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);
        b.GetComponent<Projectile>().Initialize(5, 75, 2, 0, null); // version without special effect
        //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect

        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;

        return true;
    }

  
}
