using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int maxShield = 20;
    public int currShield;
    public int recoverRate = 1;
    public bool protect = true;
    public bool recover = false;

    private float damageTime = 0;

    public ShieldBarScript shield_ui;


    // Start is called before the first frame update
    void Start()
    {
        currShield = maxShield;

        if (shield_ui != null) {
            shield_ui.SetMaxShield(maxShield);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currShield < 0)
        {
            currShield = 0;
        }

        if (currShield == 0)
        {
            protect = false;
        } else
        {
            protect = true;
        }
    }

    private void FixedUpdate()
    {
        if (shield_ui != null)
        {
            shield_ui.SetShield(currShield);
        }
        damageTime += Time.deltaTime;
        if (damageTime >= 5f && currShield <= maxShield)
        {
            StartCoroutine(heal());
            //currShield += recoverRate;
        } else if (currShield >= maxShield)
        {
            currShield = maxShield;
        }
    }

    public void depleteShield(int damage)
    {
        currShield += damage;
        damageTime = 0f;
        
        if (currShield <= 0)
        {
            currShield = 0;
        }
    }

    private IEnumerator heal()
    {
        yield return new WaitForSeconds(1);
        if (currShield < maxShield)
        {
            currShield += recoverRate;
        }
    }

    private IEnumerator wait_to_recover()
    {
        recover = false;
        yield return new WaitForSeconds(5);
        
    }
}
