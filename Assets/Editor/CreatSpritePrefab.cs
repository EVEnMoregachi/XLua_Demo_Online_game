//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEditor;
//using System.IO;
//
//public class CreatSpritePrefab : MonoBehaviour
//{
//
//    //  /// &lt;summary&gt;
//    //  /// Assets 目录下的小图片目录, 包括子目录的所有图片文件都会进行处理.
//    //  /// &lt;/summary&gt;
//    //  private const string ORIGIN_DIR = "\\RawData\\Sprites";
//    //
//    //  /// &lt;summary&gt;
//    //  /// Assets 目录下的小图预制件生成的目标目录, 注意该目录下不要存放其他资源, 每次生成时都会清空该目录下的所有文件.
//    //  /// &lt;/summary&gt;
//    //  private const string TARGET_DIR = "\\Resources\\Sprites";
//    //
//    //  /// &lt;summary&gt;
//    //  /// 将制定目录下的原始图片一对一打包成 Prefab 方便在游戏运行中读取指定的图片.
//    //  /// &lt;/summary&gt;
//
//    static string TARGET_DIR = "/Resources/Sprites/";
//    static string ORIGIN_DIR = "/RawData/Sprites/";
//
//    [MenuItem("Hammerc/2D/MakeSpritePrefabs")]
//    private static void MakeSpritePrefabs()
//    {
//
//        EditorUtility.DisplayProgressBar("Make Sprite Prefabs", "Please wait...", 1);
//
//        string targetDir = Application.dataPath + TARGET_DIR;
//        //删除目标目录
//        if (Directory.Exists(targetDir))
//            Directory.Delete(targetDir, true);
//        if (File.Exists(targetDir + ".meta"))
//            File.Delete(targetDir + ".meta");
//        //创建空的目标目录
//        if (!Directory.Exists(targetDir))
//            Directory.CreateDirectory(targetDir);
//
//        //获取源目录的所有图片资源并处理
//        string originDir = Application.dataPath + ORIGIN_DIR;
//        DirectoryInfo originDirInfo = new DirectoryInfo(originDir);
//
//        MakeSpritePrefabsProcess(originDirInfo.GetFiles("*.jpg", SearchOption.AllDirectories), targetDir);
//        MakeSpritePrefabsProcess(originDirInfo.GetFiles("*.png", SearchOption.AllDirectories), targetDir);
//        EditorUtility.ClearProgressBar();
//
//    }
//
//    static private void MakeSpritePrefabsProcess(FileInfo[] files, string targetDir)
//    {
//        foreach (FileInfo file in files)
//        {
//            string allPath = file.FullName;
//            string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
//            //加载贴图
//            Sprite sprite = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite)) as Sprite;
//           
//            //创建绑定了贴图的 GameObject 对象
//            GameObject go = new GameObject(sprite.name);
//            go.AddComponent<Image>().sprite = sprite;
//            //获取目标路径
//            string targetPath = assetPath.Replace("Assets" + ORIGIN_DIR + "\\", "");
//            //去掉后缀
//            targetPath = targetPath.Substring(0, targetPath.IndexOf("."));
//            //得到最终路径
//            //          targetPath = targetDir + "\\" + targetPath + ".prefab";
//            targetPath = targetDir + "\\" + targetPath + ".prefab";
//            //得到应用当前目录的路径
//            string prefabPath = targetPath.Substring(targetPath.IndexOf("Assets"));
//            //创建目录
//            Directory.CreateDirectory(prefabPath.Substring(0, prefabPath.LastIndexOf("\\")));
//            //生成预制件
//            PrefabUtility.CreatePrefab(prefabPath.Replace("\\", "/"), go);
//
//            //销毁对象
//            GameObject.DestroyImmediate(go);
//        }
//    }
//}
