using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public Joystick joystick;
	//[Range(0, 1)] public float joystickDeadzone;
	public float acceleration;
	public float jumpForce;
	public float jumpRequestDuration;
	public float jumpCooldown;
	public float maxCollision;
    public UnityEngine.UI.Text screenLog;
    
    private Rigidbody2D rb;
	private BoxCollider2D playerCollider;
	//private LayerMask groundLayerIndex;
	private float jumpReqRemainingTime;
	private float jumpReqRemainingCooldown;
    private Vector2 spawnPoint;
	private PVector2 appliedVelocity;
	private bool jumpRequest;
	private PVector2 right;
	private PVector2 up;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<BoxCollider2D>();
        spawnPoint = transform.position;
		appliedVelocity = PVector2.zero;
		up = PVector2.up;
		right = PVector2.right;
	}

	void Update() {
		// Gira o vetor da gravidade normalizado 90º sentido anti-horário retornando o que é "direita" na orientação atual
		right = new PVector2(-Physics2D.gravity.normalized.y, Physics2D.gravity.normalized.x);
		up = new PVector2(-right.v.y, right.v.x);
		PVector2 inputDirection = new PVector2(joystick.Direction);

		// Cálculo do quanto mover o player apenas na direção do vetor "right"
		if (inputDirection != PVector2.zero)
			appliedVelocity = inputDirection.ProjectOn(right) * acceleration;
		else
			appliedVelocity = PVector2.zero;

		if (jumpReqRemainingTime > 0f) jumpReqRemainingTime -= Time.deltaTime;
		else jumpRequest = false;
		if (jumpReqRemainingCooldown > 0f) jumpReqRemainingCooldown -= Time.deltaTime;

		// Checa se o joystick está para "cima" o suficiente para pular, baseando-se na orientação atual
		if (!jumpRequest && jumpReqRemainingCooldown <= 0f && inputDirection.ScaleOfProjectionOn(up) > .7f) {
			jumpRequest = true;
			jumpReqRemainingTime = jumpRequestDuration;
		}
	}

	void FixedUpdate() {
		if (jumpRequest && IsGrounded()) {
			jumpRequest = false;
			jumpReqRemainingCooldown = jumpCooldown;
			rb.velocity += up.v * jumpForce;
		}
		if (new PVector2(rb.velocity).ProjectOn(right).v.magnitude < 5f)
			rb.velocity += appliedVelocity.v * Time.fixedDeltaTime;

	}

	public bool IsGrounded() {
		ContactPoint2D[] contacts = new ContactPoint2D[8];
		playerCollider.GetContacts(contacts);
		foreach (ContactPoint2D contact in contacts) {
			if (new PVector2(contact.normal).ScaleOfProjectionOn(up) > 0.6f)
				return true;
		}
		return false;
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
        if (new PVector2(collision.relativeVelocity).ScaleOfProjectionOn(up) > maxCollision) {
            transform.position = spawnPoint;
            StartCoroutine("FallDamageMessage");
        }
	}

    private IEnumerator FallDamageMessage() {
        screenLog.text = "ih ala morreu de queda kkkkkkkkkkkkkkkk";
        yield return new WaitForSeconds(8);
        screenLog.text = "";
    }

}
/*
[X] Encapsular Vector2 para projeção
[X] Velocidade máxima sem contar gravidade
[ ] Morrer e renascer quando sofrer impacto forte
[ ] Botão para não girar cenário junto com o celular enquanto pressionado
[ ] Level número 0: Tutorial/level fácil pro jogador aprender sozinho
[ ] Level número Alface: Portal
*/
