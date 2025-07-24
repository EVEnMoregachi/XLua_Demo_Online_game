using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class RpcMakeCode
{
    static private string path = "Assets/Lua/Net";

    [MenuItem("GameTools/Create Net Code")]
    static void CreateRpcCode()
    {
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        if (Directory.Exists(path + "/Base") == false)
            Directory.Delete(path + "/Base", true);

        Directory.CreateDirectory(path + "/Base");

        string s = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/request.json").text;

        List<RpcEditorInfo> list = JsonConvert.DeserializeObject<List<RpcEditorInfo>>(s);

        for (int i = 0; i < list.Count; i++)
        {
            CreateRpcCode(list[i]);
        }


        AssetDatabase.Refresh();
    }

    private static void CreateRpcCode(RpcEditorInfo rpcEditorInfo)
    {
        string msgName = rpcEditorInfo.name;
        string rpcName = rpcEditorInfo.name.Substring(0,1).ToUpper() + rpcEditorInfo.name.Substring(1);
        string parameter1 = "";
        string parameter2 = "";
        string desc = "--"+rpcEditorInfo.desc;

        for (int i = 0; i < rpcEditorInfo.parameter.Count; i++)
        {
            parameter1 += (string.IsNullOrEmpty(parameter1) ? "" : ",")+ rpcEditorInfo.parameter[i].name;
            parameter2 += "\n\tself.data." + rpcEditorInfo.parameter[i].name + " = " + rpcEditorInfo.parameter[i].name;
            desc += "\n-- " + rpcEditorInfo.parameter[i].name + "(" + rpcEditorInfo.parameter[i].type + "): " + rpcEditorInfo.parameter[i].desc;
        }

        parameter1 += (string.IsNullOrEmpty(parameter1) ? "" : ",") + "callback";
        parameter2 += "\n\tself.callback = callback"; 
        File.WriteAllText(path + "/Base/Net" + rpcName + "Base.lua.txt"
            , codeBase.Replace("{rpcName}",rpcName).Replace("{parameter1}", parameter1)
            .Replace("{parameter2}", parameter2).Replace("{desc}", desc).Replace("{msgName}", msgName),new System.Text.UTF8Encoding(false));// System.Text.Encoding.UTF8
        
        if (File.Exists(path + "/Net" + rpcName + ".lua.txt") == false)
        {
           
            File.WriteAllText(path + "/Net" + rpcName + ".lua.txt",
                code.Replace("{rpcName}", rpcName).Replace("{parameter1}", parameter1).Replace("{parameter2}", parameter2), new System.Text.UTF8Encoding(false));
        }


    }

    //self.data.session = GameData.session
    //self.data.account = GameData.playerData.id

    static public string codeBase = @"local Net{rpcName}Base = {}

{desc}
function Net{rpcName}Base:Init({parameter1})
    self.data = {}
	self.data.msgName = ""{msgName}""
{parameter2}

    return self
end

function Net{rpcName}Base:Send()
    Net.Send(self)
end

function Net{rpcName}Base:OnReceive(v)
	-- body
    self.responseData = v.value.data[self.data.msgName]
    self:OnResult(v)
    if self.callback ~= nil then
        self.callback(self)
    end
end

return Net{rpcName}Base

";
    static public string code = @"Net{rpcName} = {}
local net = require(""Net/Base/Net{rpcName}Base"")
net.__index = net

function Net{rpcName}.Create({parameter1})
    local t = {}
	setmetatable(t, net)
    t:Init({parameter1})
    return t
end 

function net:OnResult(v)
        
end
";
}

class RpcEditorInfo
{
    public string name;
    public string desc;
    public List<RpcEditorParameterInfo> parameter;

    public RpcEditorInfo()
    {
        name = "";
        desc = "";
        parameter = new List<RpcEditorParameterInfo>();
    }
}

class RpcEditorParameterInfo
{
    public string name;
    public string type;
    public string desc;

    public RpcEditorParameterInfo()
    {
        name = "";
        type = "";
        desc = "";
    }
}