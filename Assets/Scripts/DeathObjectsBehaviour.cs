using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathObjectsBehaviour : MonoBehaviour {
	public GameObject player;
	public UnityEngine.UI.Text screenLog;
	public Vector2 playerSpawn;

	public void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject == player) {
			player.transform.position = playerSpawn;
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			screenLog.text = "ja morreu ze kkkkkk";
		}
	}

}
