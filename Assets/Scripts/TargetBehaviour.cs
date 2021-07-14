using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetBehaviour : MonoBehaviour {

	public UnityEngine.UI.Text screenLog;

	public void OnTriggerEnter2D(Collider2D col) {
		screenLog.text = "boa";
	}

}
