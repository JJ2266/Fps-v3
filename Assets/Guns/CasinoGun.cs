using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CasinoGun : Gun
{
    
    [SerializeField] TMP_Text message;
    bool Special = false;
    // Start is called before the first frame update
    public override bool AttemptFire()
    {
        if (!base.AttemptFire())
            return false;


        var b = Instantiate(bulletPrefab, gunBarrelEnd.transform.position, gunBarrelEnd.rotation);

        if (!Special)
        {
            b.GetComponent<Projectile>().Initialize(Random.Range(1, 7), 100, 2, 5, null); // version without special effect
                                                                                          //b.GetComponent<Projectile>().Initialize(1, 100, 2, 5, DoThing); // version with special effect
            b.transform.localScale = new Vector3(3, 3, 3);
        }
       
        

        anim.SetTrigger("shoot");
        elapsed = 0;
        ammo -= 1;

        if (Special)
        {
            b.GetComponent<Projectile>().Initialize(Random.Range(6, 13), 100, 2, 8, null);
            b.transform.localScale = new Vector3(7,7,7);
            StartCoroutine(ResetSpecial());
        }

        return true;
    }
    public override bool AttemptAltFire()
    {
        if (!base.AttemptFire())
            return false;

        
        if (ammo >= 5)
        {
            anim.SetTrigger("special");
            
            anim.SetBool("canShoot", false);
        }
            


        return true;
    }
    public void ChooseEffect()
    {
        anim.SetBool("canShoot", true);
        int r = Random.Range(1,6);
        message.GameObject().SetActive(true);
        switch (r)
        {
            case 1:
                ammo -= 7;
                message.text = "- 7 Ammo";
                message.color = Color.red;
                StartCoroutine(ResetText());
                break;
            case 2:
                ammo += 7;
                message.text = "+ 7 Ammo";
                StartCoroutine(ResetText());
                break;
            case 3:
                //more damage and bigger bullets
                Special = true;
                message.text = "Jackpot!";
                StartCoroutine(ResetText());
                break;
            case 4:
                //faster fire rate
                timeBetweenShots = 0.2f;
                message.text = "Fire rate up !";
                StartCoroutine(ResetFireRate());
                StartCoroutine(ResetText());
                break;
            case 5:
                ammo = 0;
                message.text = "Minus All ammo! ";
                message.color= Color.red;
                StartCoroutine(ResetText());
                break;
        }
    }
    IEnumerator ResetSpecial()

    {
        yield return new WaitForSeconds(7);
        Special = false;
        
        
    }
    IEnumerator ResetFireRate()

    {
        yield return new WaitForSeconds(7);
        
        timeBetweenShots = 0.35f;

    }
    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(3);
        message.GameObject().SetActive(false);
        message.color = Color.yellow;
    }

    
}
