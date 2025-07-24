using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : MonoBehaviour {
    public Transform 剑的位置;
    Vector3 目标位置;
	// Use this for initialization
	void Start () {
        this.transform.position = 剑的位置.position;
	}
    int 状态; // 0 待机  1 飞出去  2 飞回去
	// Update is called once per frame


    public void 飞向目标(Vector3 目标)
    {
        LuaTools.PlaySoundWav("QuickSwing");
        状态 = 1;
        目标位置 = 目标;
        this.transform.LookAt(目标位置);
        this.transform.DOMove(目标位置, 0.11f).OnComplete(delegate()
        {
            飞回来(); 
        });
    }
    void 飞回来()
    {
        状态 = 3;
        this.transform.DOMove(剑的位置.position, 0.15f).OnComplete(delegate ()
        {
            状态 = 0; 
        });
    }
	void Update () {
        if (状态==0)
        {
            this.transform.position = 剑的位置.position;
            this.transform.localEulerAngles = new Vector3(90,0,5) ;
        }else if (状态 == 1)
        {

        }
        else if (状态 == 2 )
        {
            this.transform.Translate(剑的位置.position * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider  other)
    {

        if (other.tag == "怪物")
        {
            LuaTools.PlaySound("剑击中");
            Debug.Log(other.name);
            other.GetComponent<Animation>().Stop();
           other.GetComponent<Animation>().CrossFade("hit1");
        }
     
    }

}
