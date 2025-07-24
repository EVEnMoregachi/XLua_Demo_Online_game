using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerInput : MonoBehaviour {

    public static   PlayerInput instance;
    void Awake()
    {
        instance = this;
        LuaTools.Init();
        AppBoot.instance.Init();
    }
        private float speed = 5;
        private Transform m_Transform;
    public GameObject sword;
        void Start()
        {
            m_Transform = this.transform;
      
        }

        void Update()
        {
            playerMove();
            playerFlash();
        checkRay();
    }

        void playerMove()
        {                                           //检测四个斜向的按键
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            transform.GetChild(0).GetComponent<Animation>().Play("run");
            m_Transform.localRotation = Quaternion.Euler(0, -45, 0);//旋转的四元数
                m_Transform.Translate(new Vector3(-0.71f, 0, 0.71f) * speed * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            transform.GetChild(0).GetComponent<Animation>().Play("run");
            m_Transform.localRotation = Quaternion.Euler(0, 45, 0);
                m_Transform.Translate(new Vector3(0.71f, 0, 0.71f) * speed * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            transform.GetChild(0).GetComponent<Animation>().Play("run");
            m_Transform.localRotation = Quaternion.Euler(0, -135, 0);
                m_Transform.Translate(new Vector3(-0.71f, 0, -0.71f) * speed * Time.deltaTime, Space.World);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            transform.GetChild(0).GetComponent<Animation>().Play("run");
            m_Transform.localRotation = Quaternion.Euler(0, 135, 0);
                m_Transform.Translate(new Vector3(0.71f, 0, -0.71f) * speed * Time.deltaTime, Space.World);
            }
            else
            {                                   //单独对四个正方向最后进行检测
                if (Input.GetKey(KeyCode.W))
            {
                transform.GetChild(0).GetComponent<Animation>().Play("run");
                m_Transform.localRotation = Quaternion.Euler(0, 0, 0);
                    m_Transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
                }
                else if (Input.GetKey(KeyCode.S))
            {
                transform.GetChild(0).GetComponent<Animation>().Play("run");
                m_Transform.localRotation = Quaternion.Euler(0, 180, 0);
                    m_Transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
                }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.GetChild(0).GetComponent<Animation>().Play("run");
                m_Transform.localRotation = Quaternion.Euler(0, -90, 0);
                    m_Transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
                }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.GetChild(0).GetComponent<Animation>().Play("run");
                m_Transform.localRotation = Quaternion.Euler(0, 90, 0);
                    m_Transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
                }
            else
            {
                transform.GetChild(0).GetComponent<Animation>().CrossFade("idle");
            }
        }

        }
        void playerFlash()
        { 
            if (Input.GetKeyDown(KeyCode.Space) )
            { 
               m_Transform.DOMove(m_Transform.position+ m_Transform.forward*6, 0.2f); 
            }
        }

    void checkRay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //从摄像机发出到点击坐标的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                //划出射线，只有在scene视图中才能看到
                Debug.DrawLine(ray.origin, hitInfo.point);
                GameObject gameObj = hitInfo.collider.gameObject;

               
             //   Debug.Log("click object name is " + gameObj.name);
                sword.GetComponent<Sword>().飞向目标(hitInfo.point + Vector3.up * 2.5f);
 
                //当射线碰撞目标为boot类型的物品，执行拾取操作
                //if (gameObj.tag == "地面")
                //{
                //    Debug.Log("pickup!");
                //}
            }
        }

    }


    }  
