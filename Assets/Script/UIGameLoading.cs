using System.Collections;
using System.Collections.Generic;
#if !UNITY_WEBPLAYER
using System.IO;
#endif
using UnityEngine;
using UnityEngine.UI;
 using System;

public class UIGameLoading : MonoBehaviour
{
 string luaABname = "lua";
    public Text txtVersion;
    public Text txtRes;
    public Text txtSize;
    public Text txtSize2;
    public Text txtSpeed;
    public Slider progressBar;
    public GameObject objMessage;
    public Text txtContent;
    public Button btnConfirm;
    public GameObject Tip;

#if !UNITY_WEBPLAYER

    List<AssetBundleInfo> listDown;

    Dictionary<string, AssetBundleInfo> dicServer;
    Dictionary<string, AssetBundleInfo> dicLocal;

    string timeServerString;
    string timeLocalString;

    uint timeServer { get { return uint.Parse(timeServerString); } }
    uint timeLocal { get { return uint.Parse(timeLocalString); } }

    float allSize;
    float downSize;

    public void Start()
    {
//        if (int.Parse(GetTimeStamp(true))>1560139201){
//           Tip.SetActive(true);
//          return;
//        }

        txtVersion.text = "当前版本:" + GameConfig.Version;
        txtRes.text = "";
        txtSize.text = "";
        txtSize2.text = "";

        progressBar.gameObject.SetActive(false);

        dicLocal = new Dictionary<string, AssetBundleInfo>();

        if (Directory.Exists(LoadTools.assetBundlePath) == false)
            Directory.CreateDirectory(LoadTools.assetBundlePath);

#if UNITY_EDITOR
        if (LoadTools.useAssetBundle == false){
            Debug.Log("aa");
            StartLogin(); 
        }
        else
            CopyDataFromStreaming();
#else
        CopyDataFromStreaming();
#endif
    }

    public static string GetTimeStamp(bool bflag)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }
 
    private void CopyDataFromStreaming()
    { 
        
        Debug.Log("CopyDataFromStreaming LoadTools.assetBundlePath  " +LoadTools.assetBundlePath);
        // if (File.Exists(Path.Combine(LoadTools.assetBundlePath, "assetslist.txt")) == false)
        // {
           
            StartCoroutine(CopyStreaming());
        //}
        // else
        // {
        //     StartDown();
        // }
    }

    private IEnumerator CopyStreaming()
    {
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0;
        txtRes.text = "首次进入游戏初始化资源";//(不消耗流量)
        yield return new WaitForEndOfFrame();
        string wwwStreamingAssetBundlePath = Path.Combine(LoadTools.wwwStreamingAssetsPath, "AssetBundle");
        
 Debug.Log("CopyStreaming wwwStreamingAssetBundlePath  " +wwwStreamingAssetBundlePath);
        WWW www = new WWW(Path.Combine(Path.Combine(LoadTools.wwwStreamingAssetsPath, "AssetBundle"), "assetslist.txt"));

        while (www.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        if (string.IsNullOrEmpty(www.error) == false)
        {
            StartCoroutine(CopyStreaming());
            yield break;
        }
Debug.Log("CopyStreaming 1111  " );

        string txtAssetslist = www.text;
        string[] arrAssetslist = www.text.Split('\n');
        
        for (int i = 1; i < arrAssetslist.Length; i++)
        {
//Debug.Log(i+" CopyStreaming arrAssetslist[i]  " + arrAssetslist[i] );
            string[] arrData = arrAssetslist[i].Split(',');

            progressBar.value = (i + 1f) / arrAssetslist.Length;

            string fileName = arrData[1]+"_"+arrData[2];
string  abname = arrData[1];
//Debug.Log("CopyStreaming fileName  " +Path.Combine(wwwStreamingAssetBundlePath, fileName) );
            www = new WWW(Path.Combine(wwwStreamingAssetBundlePath, fileName));

            while (www.isDone == false)
            {
                yield return new WaitForEndOfFrame();
            }
            if (string.IsNullOrEmpty(www.error) == false)
            {
                i--;
                continue;
            }
             if (fileName.IndexOf("lua") != -1 )
                    {
                            Debug.Log("fileName  yy   ***  "+fileName);
                    } 
            File.WriteAllBytes(Path.Combine(LoadTools.assetBundlePath, abname), www.bytes);

            yield return new WaitForFixedUpdate();
        }

        //File.Copy(ResourcesTools.streamingAssetsPath + "/AssetBundle/assetslist.txt", Path.Combine(ResourcesTools.assetBundlePath, "assetslist.txt"));

        File.WriteAllText(Path.Combine(LoadTools.assetBundlePath, "assetslist.txt"), txtAssetslist);

        txtRes.text = "";
        StartDown();
    }

    private void StartDown()
    {
Debug.Log("StartDown StartDown  "   );
        listDown = new List<AssetBundleInfo>();

        if (string.IsNullOrEmpty(GameConfig.AssetIP) == false)
        {
            StartCoroutine(CheckResources());
        }
        else
        {
            Debug.Log("bb");
            StartLogin();
        }

    }

    private IEnumerator CheckResources()
    {
        string s = "检查更新";
        txtRes.text = s;

#if UNITY_EDITOR
        WWW www = new WWW(GameConfig.AssetIP + "/assetslist.txt");
#else
        WWW www = new WWW(GameConfig.AssetIP + "/assetslist.txt?v="+UnityEngine.Random.Range(10000,99999));
#endif
        int index = 0;
        while (www.isDone == false)
        {
            txtRes.text = s + "...".Substring(index);
            index = (index + 1) % 2;
            yield return new WaitForEndOfFrame();
        }

        if (string.IsNullOrEmpty(www.error) == false)
        {
            Debug.Log(www.url + "\n" + www.error);
            yield return new WaitForSeconds(5f);
            StartCoroutine(CheckResources());
        }
        else
        {
            string[] arr = www.text.Split('\n');
            timeServerString = arr[0];
            dicServer = CreateAssetDictionary(arr);

            if (File.Exists(LoadTools.assetBundlePath + "/assetslist.txt") == true)
            {
                #if !UNITY_WEBPLAYER
                        arr = File.ReadAllText(LoadTools.assetBundlePath + "/assetslist.txt").Split('\n');
                #endif
                timeLocalString = arr[0];
                dicLocal = CreateAssetDictionary(arr);
                GameConfig.Assetversion = (timeLocal % 100000).ToString();
                txtVersion.text = "当前版本:" + GameConfig.Version + ":" + GameConfig.Assetversion;
            }
            else
            {
                timeLocalString = "0";
                dicLocal = new Dictionary<string, AssetBundleInfo>();
            }
            yield return new WaitForEndOfFrame();
            if (timeServer > timeLocal)
            {
                foreach (var item in dicServer.Values)
                {
                    if (dicLocal.ContainsKey(item.name) == false)
                    {
                        allSize += item.size;
                        listDown.Add(item);
                    }
                    else if (dicLocal[item.name].md5 != item.md5)
                    {
                        allSize += item.size;
                        listDown.Add(item);
                    }
                }

                //objMessage.SetActive(true);
                 StartCoroutine(DownAssets());
              //  txtContent.text = "发现版本更新,本地更新大小约"+GetSize(allSize)+",是否更新?";
/*
                btnCancel.onClick.AddListener(() => { Application.Quit(); });
                btnConfirm.onClick.AddListener(() => {
                   // objMessage.SetActive(false);
                    StartCoroutine(DownAssets()); }); */
 
            }
            else
            {
            Debug.Log("cc");
                StartLogin();
            }
        }
    }

    private string GetSize(float v)
    {
        if (v < 1024)
            return v + "K";
        if (v < 1024 * 1024)
            return (v / 1024f).ToString("0.00") + "KB";
        if (v < 1024 * 1024 * 1024)
            return (v / (1024f * 1024f)).ToString("0.00") + "MB";
        return "";
    }

    private IEnumerator DownAssets()
    {
        txtRes.text = "正在下载更新文件";
        txtSize2.text = "0%";
        txtSize.text = string.Format("{0}MB/{1}", (downSize).ToString("0.00"), GetSize(allSize));
        txtSpeed.text = "0KB/S";
        progressBar.value = 0;
        progressBar.gameObject.SetActive(true);
        for (int i = 0; i < listDown.Count; i++)
        {
            AssetBundleInfo ab = listDown[i];
            WWW www = new WWW(GameConfig.AssetIP + "/" + ab.name + "_" + ab.md5);
            while (www.isDone == false)
            {
                yield return new WaitForEndOfFrame();
                progressBar.value = (1f * (downSize + www.bytesDownloaded)) / allSize;
                txtSize.text = string.Format("{0}/{1}", GetSize(downSize + www.bytesDownloaded), GetSize(allSize));
                txtSpeed.text = string.Format("{0}/S", GetSize(www.bytesDownloaded / Time.deltaTime));
                txtSize2.text = (progressBar.value * 100).ToString("0.00") + "%";
            }
            //yield return www;
            if (string.IsNullOrEmpty(www.error) == false)
            {
                Debug.Log(ab.name + " " + www.error + " " + www.url);
                yield return new WaitForSeconds(5f);
                i--;
                continue;
            }
            downSize += www.bytesDownloaded;
#if !UNITY_WEBPLAYER
            File.WriteAllBytes(LoadTools.assetBundlePath + "/" + ab.name, www.bytes);
#endif
            dicLocal[ab.name] = ab; 
            if (i == listDown.Count - 1)
            {
                timeLocalString = timeServerString;
                GameConfig.Assetversion = (timeLocal % 100000).ToString();
            }
            SaveLocal();
        }  
        StartLogin();

    }

    private void SaveLocal()
    {
        string txt = timeLocalString;


        foreach (var item in dicLocal.Values)
        {
            txt += "\n" + item.type + "," + item.name + "," + item.md5 + "," + item.size + ","+item.level;
        }
#if !UNITY_WEBPLAYER
        File.WriteAllText(LoadTools.assetBundlePath + "/assetslist.txt", txt);
#endif
    }

    private Dictionary<string, AssetBundleInfo> CreateAssetDictionary(string[] arrServerList)
    {
        Dictionary<string, AssetBundleInfo> result = new Dictionary<string, AssetBundleInfo>();
        for (int i = 1; i < arrServerList.Length; i++)
        {
            if (string.IsNullOrEmpty(arrServerList[i]) == true)
                continue;
            string[] arr = arrServerList[i].Split(',');
            AssetBundleInfo ab = new AssetBundleInfo(int.Parse(arr[0]), arr[1], arr[2], float.Parse(arr[3]), int.Parse(arr[4]));
            result.Add(ab.name, ab);
        }
        return result;
    }

    private void StartLogin()
    {
        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame()
    {
        txtRes.text = "游戏初始化";
        progressBar.gameObject.SetActive(true);
        txtSpeed.gameObject.SetActive(false);
        txtSize.gameObject.SetActive(false);
        txtSize2.gameObject.SetActive(false);
        progressBar.value = 0f;
        yield return new WaitForEndOfFrame();

        LoadTools.Init();

        // List<AssetBundleInfo> list = new List<AssetBundleInfo>(dicLocal.Values);

        // for (int i = 0; i < list.Count; i++)
        // {
        //     if (list[i].level == 1)
        //     {
        //         AssetBundleCreateRequest ar = LoadTools.LoadAssetBundleAsync(list[i].name);
        //         yield return ar;
        //         LoadTools.AddAssetBundle(ar.assetBundle);
        //     }
        //     progressBar.value = (1f+i)/list.Count * 0.9f;
        //     yield return new WaitForEndOfFrame();
        // }
      

        txtRes.text = "";
        progressBar.gameObject.SetActive(false);
        Debug.Log("over load");
        #if ( UNITY_ANDROID|| UNITY_IOS) && !UNITY_EDITOR

          luaABname = "";
           Debug.Log("UNITY_ANDROID UNITY_ANDROID   luaABname = "+luaABname);
            if  (luaABname==""){ 
                foreach (string key in dicLocal.Keys)
                {

                  //  Debug.Log("key  "+key);
                    if (key.IndexOf("lua") != -1 )
                    {
                            luaABname = key ; 
                            Debug.Log("luaABname == " + luaABname);
                            ResourceManager.luaFileName = luaABname ;
                    }else if (key.IndexOf("font") != -1 )
                    {
                           
                            Debug.Log("font == " + key);
                             ResourceManager.InitFontResource(key);
                    }
                  
                }
            }
            Debug.Log("luaABname yy "+luaABname);
           ResourceManager.loadLuaToByte(luaABname);
        #endif

        
        GameObject go = Resources.Load<GameObject>("Main");
        Instantiate(go);
        Destroy(this.gameObject);
        AppBoot.instance.Init();
    }

    private void SluaTickHandler(int obj)
    {
        progressBar.value = obj / 100f / 10f + 0.9f;
    }

    private bool appBootInit;

    private void DoComplete()
    {
        appBootInit = true;
       
    }
    
#endif
}

public class AssetBundleInfo
{
    public string name;
    public string md5;
    public float size;
    public int type;
    public int level;

    public AssetBundleInfo(int type, string name, string md5, float size,int level)
    {
        this.type = type;
        this.name = name;
        this.size = size;
        this.md5 = md5;
        this.level = level;
    }

}
