using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour {

	public float gravMultiplier;
	public UnityEngine.UI.Text screenLog;
	
	private Gyroscope gyro;
	private bool gyroActive;
	private bool plain;
	
	void Awake() {
		EnableGyro();
		plain = false;
	}

	public void EnableGyro() {
		if (gyroActive)
			return;

		if (SystemInfo.supportsGyroscope) {
			gyro = Input.gyro;
			gyroActive = gyro.enabled = true;
		}
		
		if (!gyroActive)
			screenLog.text = "Giroscópio não suportado";

	}

	private void Update() {
		if (!gyroActive)
			return;
		
		Quaternion rot = gyro.attitude;
		Vector2 down = new Vector2(gyro.gravity.x, gyro.gravity.y).normalized * gravMultiplier;

		if (Mathf.Abs(rot.x) < 0.15f && Mathf.Abs(rot.y) < 0.15f) {
			if (!plain) {
				if (screenLog != null)
					screenLog.text = "Dispositivo em uma superfície plana bobão.\nLevanta o celular, o jogo precisa de gravidade";
				plain = true;
			}
			return;
		} else if (plain) {
			plain = false;
			if (screenLog != null)
				screenLog.text = "";
		}

		Physics2D.gravity = down;
	}

	private float VectorToAngle(Vector2 v) {
		return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
	}

	public Quaternion GetRotation() => gyro.attitude;

}
