using UnityEngine;
using UnityEngine.Events;

public class Resettable : MonoBehaviour {

	public Transform spawn;
	private Rigidbody2D rb;

	void Start() {
		GameMaster.instance.resetEvent.AddListener(new UnityAction(Reset));
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	public void Reset() {
		if (rb) rb.velocity = Vector2.zero;
		transform.position = spawn.position;
	}

}
