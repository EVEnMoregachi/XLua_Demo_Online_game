using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour {



	public  static UImanager  instance;
	public Dictionary<string,GameObject> UIDic = new Dictionary<string,GameObject> ();

	Dictionary<string,UIBase> UIDataMap = new Dictionary<string,UIBase> ();
    
	List<GameObject>  MainUIList = new List<GameObject>();
	List<GameObject>  SecondUIList = new List<GameObject>();
	List<GameObject>  PopUIList = new List<GameObject>();

	public GameObject parent;

	class UIBase {
		public string name;
		public UIType uiType;   
		public string  path;
        public string[] nextUIList;

	}
	public enum UIType
	{ 
		ROOT = 1,
		MAIN = 2,
		SECOND = 3,
		POP = 4 
	}
	void Awake(){ 
		instance = this;
		UIBase ub = new UIBase ();
		ub.path = "main";
		ub.name = "main";
		UIDataMap.Add ("main", ub );
		ub = new UIBase ();
		ub.path = "pop";
		ub.name = "pop";
		UIDataMap.Add ("pop", ub );
		ub = new UIBase ();
		ub.path = "second";
		ub.name = "second";
		UIDataMap.Add ("second", ub );
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	// 打开UI界面
	public void OpenUI(UIType uiType , string name){
		GameObject go = getUIgameObject (name);
		switch (uiType) {
			case UIType.ROOT: //点到根，清空一切页面
				closeUI(UIType.MAIN);
				closeUI(UIType.SECOND);
				closeUI(UIType.POP);
				break;
			case UIType.MAIN:
				closeUI (UIType.MAIN);
				closeUI (UIType.SECOND);
				closeUI (UIType.POP);
				openNext (uiType,go);
				break;
			case UIType.SECOND:
				closeUI(UIType.SECOND);
				closeUI(UIType.POP);
				openNext (uiType,go);
				break;
			case UIType.POP:
				closeUI(UIType.POP);
				openNext (uiType,go);
				break;




		}

	}
	void openNext(UIType uiType, GameObject go ){
		switch (uiType) { 
		case UIType.MAIN:
			go.SetActive (true);
			break;
		case UIType.SECOND:
			go.SetActive (true);
			break;
		case UIType.POP:
			go.SetActive (true);
			break; 
		}

	}

	public GameObject getUIgameObject(string name){
		if (UIDic.ContainsKey (name)) {
			return UIDic [name];
		} else {


            //Debug.Log (  name);  
            //Debug.Log (  UIDataMap.Count);  
            //Debug.Log ( ((UIBase)UIDataMap[name]).name);  
            //GameObject temp =	Resources.Load<GameObject> ( ((UIBase)UIDataMap[name]).name);
            //UIDic.Add (name,Instantiate (temp, temp.transform.position, Quaternion.identity) as GameObject);
            //return temp;
            return null;
		} 
	}


	void closeUI(UIType uiType ){
		switch (uiType) { 
			case UIType.MAIN:
				for(int i=0;i< MainUIList.Count;i++){
					MainUIList [i].SetActive (false); 
				}
				break;
		case UIType.SECOND:
				for(int i=0;i< SecondUIList.Count;i++){
					SecondUIList [i].SetActive (false); 
				}
				break;
		case UIType.POP:
				for(int i=0;i< PopUIList.Count;i++){
					PopUIList [i].SetActive (false); 
				}
				break;




		}

	}


	public void buttonRegister(ButtonComponet bc){
		getUIgameObject(bc.openPage).SetActive (true);
	}
}
