using UnityEngine;
using System.Collections;
using Battle;

public class TestFOW : MonoBehaviour
{
    GameObject debugUI = null;

	// Use this for initialization
	void Start () {
        // fow系统启动
        FOWLogic.instance.Startup();
        // 通知角色出生
        Messenger.Broadcast(MessageName.MN_CHARACTOR_BORN, 1);
        // debug
         
        
    }
	
	// Update is called once per frame
	void Update () {
        int deltaMS = (int)(Time.deltaTime * 100f);
        FOWLogic.instance.Update(deltaMS);
	}

    void OnGUI()
    {
        if (debugUI != null && !debugUI.activeSelf)
        {
            if (GUI.Button(new Rect(100f, 100f, 100f, 50f), "Debug"))
            {
                debugUI.SetActive(true);
            }
        }
    }

    void OnDistory()
    {
        FOWLogic.instance.Dispose();
    }
}
