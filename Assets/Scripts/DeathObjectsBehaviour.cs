using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathObjectsBehaviour : MonoBehaviour {
	public GameObject player;
	public UnityEngine.UI.Text screenLog;
	public Vector2 playerSpawn;

	public void OnCollisionEnter2D(Collision2D coll) {
		foreach (ContactPoint2D contact in coll.contacts) {
			if (contact.otherCollider.tag == "Player" || contact.collider.tag == "Player") {
				player.transform.position = playerSpawn;
				player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				screenLog.text = "morreu de bobo";
			}
		}
	}

}
