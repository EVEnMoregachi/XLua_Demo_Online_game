using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTest : MonoBehaviour {



    public class PlayerDemo{

        public string name;
        public string ID;
        public int Atk;
        public int Hp;
        public List<PlayerEquip> equipList = new List<PlayerEquip>();
    }
    public class PlayerEquip
    {
        public string name;
        public string ID;
        public int Atk;
        public int Type; 
    }

}
