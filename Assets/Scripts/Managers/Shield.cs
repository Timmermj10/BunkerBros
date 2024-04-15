using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int maxShield = 20;
    public int currShield;
    public int recoverRate = 1;
    public bool protect = true;
    public bool recovering = false;

    private float damageTime = 0;

    public List<ShieldBarScript> shield_ui;


    // Start is called before the first frame update
    void Start()
    {
        //Initialize to max shield
        currShield = maxShield;

        if (shield_ui.Count != 0) {
            foreach (ShieldBarScript bar in shield_ui)
            {
                if (bar != null)
                {
                    bar.SetMaxShield(maxShield);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //Increase time since last damage
        damageTime += Time.deltaTime;

        //Determine whether were protecting
        protect = currShield > 0;

        //Set the shield UI
        if (shield_ui.Count != 0)
        {
            foreach (ShieldBarScript bar in shield_ui)
            {
                if (bar != null)
                {
                    bar.SetShield(currShield);
                }
            }
        }
        
        //If its been more than 5s since the last damage, and were not recovering already, start recovering
        if (damageTime >= 5f && currShield < maxShield && !recovering)
        {
            //Debug.Log("Recovering Shield");
            recovering = true;
            StartCoroutine(heal());
        } 

    }

    public void depleteShield(int damage)
    {
        //Debug.Log("Shield taking damage");
        //Reset the time since last damage
        damageTime = 0f;

        //Change shield value
        currShield = Mathf.Max(currShield + damage, 0);
    }

    private IEnumerator heal()
    {
        //Debug.Log("Starting Heal");
        //while the shield isnt taking damage and its less than max
        while (damageTime >= 5 && currShield < maxShield)
        {
            //wait recovery time and heal the shield
            yield return new WaitForSeconds(0.5f);
            currShield += recoverRate;
        }
        //Debug.Log("Ended Heal");

        recovering = false;
    }
}
