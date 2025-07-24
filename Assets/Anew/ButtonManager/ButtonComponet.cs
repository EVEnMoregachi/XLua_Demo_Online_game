using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonComponet : MonoBehaviour {
 
	public int name_;
	public int ID;
	public string openPage;
	void OnEnable() {
//		this.GetComponent<Button> ().onClick.AddListener (Register);
	}
	void  OnDisable(){
	//	this.GetComponent<Button> ().onClick.RemoveAllListeners ();

	}
	void Register(){
		UImanager.instance.buttonRegister (this);
	}
	 
}
