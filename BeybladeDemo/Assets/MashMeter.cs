using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MashMeter : MonoBehaviour {

	public Image meterContent;

	private void Start()
	{
		meterContent.fillAmount = 0;
	}

	public void UpdateMeter(float percent)
	{
		meterContent.fillAmount = percent;
	}
}
