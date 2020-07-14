using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : MonoBehaviour {

    public UnityEngine.UI.Text values;
    public Transform arrow;
    public float gravMultiplier;

    private Gyroscope gyro;
    private bool gyroActive;
	private bool floating;
	
	void Start() {
		EnableGyro();
		floating = false;
	}

    public void EnableGyro() {
        if (gyroActive)
            return;

        if (SystemInfo.supportsGyroscope) {
            gyro = Input.gyro;
            gyroActive = gyro.enabled = true;
        }
		
        if (!gyroActive)
		values.text = "Giroscópio não suportado";

    }

    private void Update() {
		if (!gyroActive)
			return;
		
		Quaternion rot = gyro.attitude;
        Vector2 down = new Vector2(gyro.gravity.x, gyro.gravity.y).normalized * gravMultiplier;

        if (Mathf.Abs(rot.x) < 0.15f && Mathf.Abs(rot.y) < 0.15f) {
			values.text = "Dispositivo em uma superfície plana.\nLevante o dispositivo.";
            arrow.rotation = Quaternion.Euler(0f, 0f, VectorToAngle(down) + 90f);
            return;
		}
        values.text = "";

        Physics2D.gravity = down;
    }

    private float VectorToAngle(Vector2 v) {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    public Quaternion GetRotation() => gyro.attitude;

}
