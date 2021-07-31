using UnityEngine;
using UnityEngine.UI;

public class GyroManager : MonoBehaviour {

	public Rigidbody2D[] gravitatedBodies;
	public float gravMultiplier;
	public UnityEngine.UI.Text screenLog;
	public GameObject level;
	public GameObject toggleRotButton;
	public Sprite freeRotationSprite;
	public Sprite frozenRotationSprite;
	public bool debug;

	private Gyroscope gyro;
	private float levelAngle;
	private bool gyroActive;
	private bool plain;
	private bool freezed;
	private Vector2 lastGravity;
	private Image togRotButImage;

	void Awake() {
		EnableGyro();
		levelAngle = 90f;
		plain = false;
		freezed = false;
		togRotButImage = toggleRotButton.GetComponent<Image>();
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
		if (debug) {
			Debug.DrawLine(Vector3.zero, Vector3.zero + (Vector3) (Vector2) gyro.gravity.normalized, new Color(0.8f, 0.2f, 0.57f, 1f), 0f, false);
			Debug.DrawLine(Vector3.zero, Vector3.zero + (Vector3) lastGravity, Color.red, 0f, false);
		}
		if (!gyroActive || freezed)
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
		level.transform.localEulerAngles = new Vector3(level.transform.localEulerAngles.x, level.transform.localEulerAngles.y, levelAngle + VectorToAngle(down));
	}

	public Quaternion GetRotation() => gyro.attitude;

	public void ToggleRotation() {
		freezed = !freezed;
		if (freezed) {
			togRotButImage.sprite = frozenRotationSprite;
			lastGravity = Physics2D.gravity;
			Physics2D.gravity = Vector2.zero;
			foreach (Rigidbody2D rb in gravitatedBodies)
				rb.velocity = Vector2.zero;
		} else {
			float lastAngle = VectorToAngle(lastGravity);
			float gyroAngle = VectorToAngle((Vector2) gyro.gravity);
			levelAngle += lastAngle - gyroAngle;
			togRotButImage.sprite = freeRotationSprite;
		}
	}

	private float VectorToAngle(Vector2 v) {
		return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
	}

}
