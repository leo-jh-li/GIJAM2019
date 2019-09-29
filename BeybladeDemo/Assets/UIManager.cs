using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public Slider healthSlider;
	public Slider ultimateSlider;
	public Text comboText;
	public Beyblade player;
   
    public float comboDisplayTime = 0;
	public float comboFadeSpeed = 1f;
	private IEnumerator comboCoroutine = null;
	public CanvasGroup comboCanvas;

	public ClashEvent clashEvent;

	void Awake() {
		player.setHealthUICallback(UpdateHealth);
		clashEvent.setComboCallback(UpdateComboText);
	}

	void Start() {
		comboCanvas.alpha = 0;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.N)) {
			// for testing only
			this.UpdateComboText(2);
		}
	}

	void UpdateHealth(float health) {
		this.healthSlider.value = health / player.m_maxStamina;
	}

	void UpdateComboText(int combo) {
		if (combo <= 0) return;

		if (comboCoroutine != null) {
			StopCoroutine(comboCoroutine);
			comboCoroutine = null;
			comboCanvas.alpha = 0;
		}
		this.comboText.text = "COMBO X" + combo.ToString(); 
		
		comboCoroutine = updateComboCor();
        StartCoroutine(comboCoroutine);
	}

	IEnumerator updateComboCor() {
		float counter = 0;
		while (counter <= 1) {
			comboCanvas.alpha = counter;
			counter += comboFadeSpeed * Time.deltaTime;
			yield return null;
		}

		yield return new WaitForSeconds(comboDisplayTime);
		counter = 0;
		while (counter <= 1) {
			comboCanvas.alpha = 1 - counter;
			counter += comboFadeSpeed * Time.deltaTime;
			yield return null;
		}

		comboCanvas.alpha = 0;

	}
}
