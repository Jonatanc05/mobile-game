using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private static readonly float EXTRA_HEIGHT = 0.2f;

	public Joystick joystick;
	[Range(0, 1)] public float joystickDeadzone;
	public float maxSpeed;
	public float jumpForce;
	public float jumpRequestDuration;

	private Rigidbody2D rb;
	private BoxCollider2D playerCollider;
	private LayerMask groundLayerIndex;
	private float jumpReqRemainingTime;
    //private float xVel;
    private Vector2 appliedVelocity;
    private bool jumpRequest;
    private Vector2 right;
    private 

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<BoxCollider2D>();
		groundLayerIndex = LayerMask.NameToLayer("level");
    }

	void Update() {
        /*if (Mathf.Abs(joystick.Horizontal) > joystickDeadzone) {
			xVel = joystick.Horizontal * maxSpeed;
		} else {
			xVel = 0f;
		}

		if (jumpReqRemainingTime > 0f) {
			jumpReqRemainingTime -= Time.deltaTime;
		} else {
			jumpRequest = false;
		}

		if (!jumpRequest && joystick.Vertical > .7f) {
			jumpRequest = true;
			jumpReqRemainingTime = jumpRequestDuration;
		}*/
        right = new Vector2(-Physics2D.gravity.y, Physics2D.gravity.x).normalized;
        Vector2 j = joystick.Direction;
        if (j != Vector2.zero) {
            appliedVelocity = right * maxSpeed * Mathf.Cos((Mathf.Atan2(j.y, j.x)*Mathf.Rad2Deg - Mathf.Atan2(right.y, right.x)*Mathf.Rad2Deg)*Mathf.Deg2Rad);
        } else
            appliedVelocity = Vector2.zero;
        //Debug.Log(Mathf.Cos((Mathf.Atan2(j.y, j.x) * Mathf.Rad2Deg - Mathf.Atan2(right.y, right.x) * Mathf.Rad2Deg)*Mathf.Deg2Rad));
        //Debug.Log(Mathf.Atan2(j.y, j.x) * Mathf.Rad2Deg - Mathf.Atan2(right.y, right.x) * Mathf.Rad2Deg);
    }

    void FixedUpdate() {
        /*if (jumpRequest && IsGrounded()) {
			rb.velocity += new Vector2(xVel * Time.deltaTime, jumpForce);
			jumpRequest = false;
		} else {
			rb.velocity += new Vector2(xVel * Time.deltaTime, 0f);
		}*/
        rb.velocity += appliedVelocity * Time.fixedDeltaTime;
	}

	public bool IsGrounded() {
		Collider2D collider = Physics2D.OverlapArea(playerCollider.bounds.min + new Vector3(0.1f, 0f, 0f),
				new Vector2(playerCollider.bounds.max.x - 0.1f, playerCollider.bounds.min.y - EXTRA_HEIGHT), 1 << groundLayerIndex);

		return collider != null;
	}

}
