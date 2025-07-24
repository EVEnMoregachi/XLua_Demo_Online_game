using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData2 : MonoBehaviour {

	 
    public class PlayerDataInfo
    {
        public string name;
        public int atk;
        public string ID;
        public int hp;
        public List<EquipInfo> equipList = new List<EquipInfo>();

    }
    public class  EquipInfo
    {
        public string ID;
        public int atk;

    }

}
