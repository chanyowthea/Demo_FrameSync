using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Facade : MonoBehaviour
{
    public static Facade Instance;

    private Dictionary<string, Component> mManagerDic = new Dictionary<string, Component>();

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitManager();
    }

    void InitManager()
    {
        // 加载资源
        AddManager(FacadeConfig.ChildSystem_Asset, gameObject.AddComponent<AssetManager>());
        // 加载UI
        AddManager(FacadeConfig.ChildSystem_View, gameObject.AddComponent<ViewManager>());
        // 接收场景消息
        AddManager(FacadeConfig.ChildSystem_UdpRecive, gameObject.AddComponent<UdpReciveManager>());
        // 发送场景消息
        AddManager(FacadeConfig.ChildSystem_UdpSend, gameObject.AddComponent<UdpSendManager>());
        // 场景消息通讯器
        AddManager(FacadeConfig.ChildSystem_UdpSocket, gameObject.AddComponent<UnityUdpSocket>());
        // 初始界面接收消息
        AddManager(FacadeConfig.ChildSystem_TcpRecive, gameObject.AddComponent<TcpReciveManager>());
        // 初始界面发送消息
        AddManager(FacadeConfig.ChildSystem_TcpSend, gameObject.AddComponent<TcpSendManager>());
        // 初始界面消息通讯器
        AddManager(FacadeConfig.ChildSystem_TcpSocket, gameObject.AddComponent<UnityTcpSocket>());
        // 场景碰撞系统
        AddManager(FacadeConfig.ChildSystem_Physic, gameObject.AddComponent<PhysicalSystem>());
    }

    void Start()
    {
        // 开始连接tcp服务器
        UnityTcpSocket tcp = GetManager<UnityTcpSocket>(FacadeConfig.ChildSystem_TcpSocket);
        if (tcp.ConnectToServer())
            GetManager<ViewManager>(FacadeConfig.ChildSystem_View).LoadView("prefab/ui/loginview_prefab");
    }

    public void AddManager<T>(string tName, T tCom) where T : Component
    {
        mManagerDic[tName] = tCom;
    }

    public T GetManager<T>(string tName) where T : Component
    {
        Component com;
        if (mManagerDic.TryGetValue(tName, out com))
            return com as T;
        else
            return default(T);
    }
}
