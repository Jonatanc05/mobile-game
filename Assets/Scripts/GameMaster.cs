using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour {

	public static GameMaster instance;
	[HideInInspector] public UnityEvent resetEvent;

	void Awake() {
		if (instance == null) {
			instance = this;
			resetEvent = new UnityEvent();
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	public void ResetAll() => resetEvent.Invoke();

}
