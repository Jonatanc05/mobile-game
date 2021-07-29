using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathObjectsBehaviour : MonoBehaviour {
	public GameObject player;

	public void OnCollisionEnter2D(Collision2D coll) {
		foreach (ContactPoint2D contact in coll.contacts) {
			if (contact.otherCollider.tag == "Player" || contact.collider.tag == "Player") {
				player.GetComponent<PlayerController>().Die("vermelho = morte\nnunca jogou video game nao po");
			}
		}
	}

}
