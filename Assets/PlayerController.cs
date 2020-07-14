using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Joystick joystick;
	//[Range(0, 1)] public float joystickDeadzone;
	public float maxSpeed;
	public float jumpForce;
	public float jumpRequestDuration;
    public float jumpCooldown;


    private Rigidbody2D rb;
	private BoxCollider2D playerCollider;
	//private LayerMask groundLayerIndex;
	private float jumpReqRemainingTime;
    private float jumpReqRemainingCooldown;
    private Vector2 appliedVelocity;
	private bool jumpRequest;
	private Vector2 right;
	private Vector2 up;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<BoxCollider2D>();
		//groundLayerIndex = LayerMask.NameToLayer("level");
		up = Vector2.up;
		right = Vector2.right;
	}

	void Update() {
		// Gira o vetor da gravidade normalizado 90º sentido anti-horário retornando o que é "direita" na orientação atual
		right = new Vector2(-Physics2D.gravity.y, Physics2D.gravity.x).normalized;
		up = new Vector2(-right.y, right.x);

		// Cálculo do quanto mover o player apenas na direção do vetor "right"
		if (joystick.Direction != Vector2.zero)
			appliedVelocity = ProjectionOfAOnB(joystick.Direction, right) * maxSpeed;
		else
			appliedVelocity = Vector2.zero;

		if (jumpReqRemainingTime > 0f) jumpReqRemainingTime -= Time.deltaTime;
		else jumpRequest = false;
        if (jumpReqRemainingCooldown > 0f) jumpReqRemainingCooldown -= Time.deltaTime;

		// Checa se o joystick está para "cima" baseando-se na orientação atual
		if (!jumpRequest && jumpReqRemainingCooldown <= 0f && ProjectionOfAOnBScale(joystick.Direction, up) > .7f) {
			jumpRequest = true;
			jumpReqRemainingTime = jumpRequestDuration;
		}
	}

	void FixedUpdate() {
		if (jumpRequest && IsGrounded()) {
            jumpRequest = false;
            jumpReqRemainingCooldown = jumpCooldown;
            Debug.Log("pulô");
			rb.velocity += up * jumpForce;
		}
		rb.velocity += appliedVelocity * Time.fixedDeltaTime;
	}

	public bool IsGrounded() {
		ContactPoint2D[] contacts = new ContactPoint2D[8];
		playerCollider.GetContacts(contacts);
		foreach (ContactPoint2D contact in contacts) {
			if (ProjectionOfAOnBScale(contact.normal, up) > 0.6f)
				return true;
		}
		return false;
	}

	Vector2 ProjectionOfAOnB(Vector2 a, Vector2 b) {
		if (b == Vector2.zero)
			return Vector2.zero;

		// Esta é a fórmula que encontrei, funciona mas (acho que) é computacionalmente caro
		//return b * Mathf.Cos(Mathf.Atan2(a.y, a.x) - Mathf.Atan2(b.y, b.x));

		// Esta é a fórmula matemática de projeção de vetores
		return b * ( (a.x*b.x + a.y*b.y) / b.sqrMagnitude );
	}

	float ProjectionOfAOnBScale(Vector2 a, Vector2 b) {
		if (b == Vector2.zero)
			return 0f;

		return (a.x * b.x + a.y * b.y) / b.sqrMagnitude;
	}

}
