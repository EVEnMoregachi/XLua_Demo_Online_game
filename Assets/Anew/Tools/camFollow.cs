using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour {

    // Use this for initialization

    GameObject target;
    Vector3 offset;

    bool ok= false;
	void Start () {
      
       

    }
	void Update()
    {
        if (ok) return;
        if (GameObject.FindGameObjectWithTag("Player")!=null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
              ok = true;
            offset = target.transform.position - this.transform.position;
        }

    }
	// Update is called once per frame
	void LateUpdate () {
        if (ok)
        {
            this.transform.position = target.transform.position - offset;
        }


    }
}
