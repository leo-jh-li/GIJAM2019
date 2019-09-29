using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShield : MonoBehaviour {

    public string assignedKey = "Fire3";
    public float maxShieldTime = 2f;
    public float shieldInitSize = 5f;
    public float shieldMinSize = 3f;
    public float baseDefense = 2f;
    public float highDefense = 6f;
    public float overshieldPenalty = 1f;
    public float overshieldPenaltyTime = 3f;
	
    public float defenseMultiplier = 2f;

    SphereCollider shield;
	Beyblade b;
    float shieldTimer;
    float shieldRadiusDelta;

    bool ready = true;

    // Use this for initialization
    void Start () {
        shieldTimer = maxShieldTime;
        shieldRadiusDelta = (shieldInitSize - shieldMinSize) / maxShieldTime;
        shield = GetComponent<SphereCollider>();
		b = GetComponent<Beyblade>();
		
		b.m_defenseMultiplier = defenseMultiplier;
        shield.radius = shieldInitSize;
	}
	
	// Update is called once per frame
	void Update () {
        if (ready)
        {
            if (Input.GetAxis(assignedKey) != 0)
            {
				if (!shield.enabled){
					b.DisablePlayerInfluence();
				    shield.enabled = true;
					b.m_defenseMultiplier = highDefense;
				}
                shieldTimer -= Time.deltaTime;
                shield.radius -= shieldRadiusDelta * Time.deltaTime;
                if (shieldTimer <= 0)
                {
                    b.m_defenseMultiplier = overshieldPenalty;
					shield.enabled = false;
                    StartCoroutine("BreakShield");
                }
            }
            else
            {
				if (shield.enabled){
					b.EnablePlayerInfluence();
					shield.enabled = false;
					b.m_defenseMultiplier = baseDefense;
				}
                if (shieldTimer < maxShieldTime)
                    shieldTimer += Mathf.Min(Time.deltaTime, maxShieldTime - shieldTimer);
                if (shield.radius < shieldInitSize)
                {
                    shield.radius += Mathf.Min(shieldRadiusDelta * Time.deltaTime, shieldInitSize - shield.radius);
                }
                
            }
        }
		
	}

    IEnumerator BreakShield()
    {
        ready = false;
        yield return new WaitForSeconds(overshieldPenaltyTime);
        ready = true;
		b.EnablePlayerInfluence();
		b.m_defenseMultiplier = baseDefense;
    }
}
