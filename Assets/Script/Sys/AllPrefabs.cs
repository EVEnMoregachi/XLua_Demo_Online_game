using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPrefabs : MonoBehaviour {

    public static AllPrefabs instance;
    //public GameObject a_enter_prefab;
    //public GameObject a_mainui_prefab; 
    //public GameObject A_package_prefab;
    //public GameObject A_menpai_prefab;  //门派
    public GameObject text_Drama_Button_Prefab;
    private GameObject changeNamePrefab;
    public Dictionary<string, GameObject> module = new Dictionary<string, GameObject>();
	public Dictionary<string, GameObject> pageNameModule = new Dictionary<string, GameObject>();//玩家的页面的名字与页面结合起来
    //文字预制体
    public GameObject text_Drama_Prefab;
    public GameObject GameMain_Prefab;
    private GameObject changeNameGo;
    void Awake() {
     
        instance = this;
       
    }
     public void init()
    {
      //  Debug.Log3("AllPrefabs Awake" + Time.realtimeSinceStartup);
      
        text_Drama_Button_Prefab = Resources.Load<GameObject>("Prefabs/text_drama_button");
        text_Drama_Prefab = Resources.Load<GameObject>("Prefabs/drama_text_unit");
        module["a_enter"] = Resources.Load<GameObject>("Prefabs/Modules/a_enter");
        //  //Debug.Log(" *****&&&*&*&*& " + module["a_enter"]);

        module["a_mainui"] = Resources.Load<GameObject>("Prefabs/Modules/a_mainui");
        changeNamePrefab = Resources.Load<GameObject>("Prefabs/Public/changeName");
       // Debug.Log3("AllPrefabs Awake end " + Time.realtimeSinceStartup);

    }
    public void makeMenuElement(string name)
    {
        module[name] = Resources.Load<GameObject>("Prefabs/Modules/" + name);
    }
    // 获取改名页面
    public GameObject getChangeNameGo()
    {
        if (changeNameGo == null)
        {
            ////Debug.Log(changeNamePrefab);
            changeNameGo = Instantiate(changeNamePrefab, changeNamePrefab.transform.position,Quaternion.identity) as GameObject;
			changeNameGo.transform.SetParent(Public.instance.canvasLoad.transform, false);
			changeNameGo.transform.SetAsLastSibling ();

        }else
        {
            changeNameGo.SetActive(true);
        }
        return changeNameGo;
    }

 	//将名字与预制体放进字典 
	public void AddPageNameModule(string ObjName,GameObject Obj){
		AllPrefabs.instance.pageNameModule[ObjName] = Obj;
	
	}




}
