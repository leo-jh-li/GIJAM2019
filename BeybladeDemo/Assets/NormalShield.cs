using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShield : MonoBehaviour {

    public float defenseMultiplier = 1f;
    public string assignedKey = "Fire3";
    public float maxShieldTime = 2f;
    public float shieldInitSize = 5f;
    public float shieldMinSize = 3f;
    public float baseDefense = 1f;
    public float highDefense = 4f;
    public float overshieldPenalty = 0.3f;
    public float overshieldPenaltyTime = 3f;

    SphereCollider shield;
    float shieldTimer;
    float shieldRadiusDelta;

    bool ready = true;

    // Use this for initialization
    void Start () {
        shieldTimer = maxShieldTime;
        shieldRadiusDelta = (shieldInitSize - shieldMinSize) / maxShieldTime;
        shield = GetComponent<SphereCollider>();
        shield.radius = shieldInitSize;
	}
	
	// Update is called once per frame
	void Update () {
        if (ready)
        {
            if (Input.GetAxis(assignedKey) != 0)
            {
                shield.enabled = true;
                shieldTimer -= Time.deltaTime;
                shield.radius -= shieldRadiusDelta * Time.deltaTime;
                defenseMultiplier = highDefense;
                if (shieldTimer <= 0)
                {
                    defenseMultiplier = overshieldPenalty;
                    StartCoroutine("BreakShield");
                }
            }
            else
            {
                shield.enabled = false;
                if (shieldTimer < maxShieldTime)
                    shieldTimer += Mathf.Min(Time.deltaTime, maxShieldTime - shieldTimer);
                if (shield.radius < shieldInitSize)
                {
                    shield.radius += Mathf.Min(shieldRadiusDelta * Time.deltaTime, shieldInitSize - shield.radius);
                }
                defenseMultiplier = baseDefense;
            }
        }
		
	}

    IEnumerator BreakShield()
    {
        ready = false;
        yield return new WaitForSeconds(overshieldPenaltyTime);
        ready = true;
    }
}
