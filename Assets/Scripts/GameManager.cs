using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public enum Translation_Type
{
   Loading
}
public enum Car_Saler_Simulator_Scenes
{
   Main_Menu,Game_Room
}

public class GameManager : MonoBehaviourPunCallbacks
{


    #region   ///======================================= Editor Gui
    #if UNITY_EDITOR
     [UnityEditor.CustomEditor(typeof(GameManager))]
    public class MyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GameManager gameManager = (GameManager)target;
            GameManager.Screw_Gui();
            GUI.backgroundColor = Color.white;
            base.OnInspectorGUI(); 
        }
    }
    #endif

    #if UNITY_EDITOR
    public static void Screw_Gui()
    {
            GameManager carManager = FindObjectOfType<GameManager>();
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 14;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.fixedHeight = 40;
            buttonStyle.margin = new RectOffset(20, 20, 5, 5);


#region Load Scene
GUILayout.BeginHorizontal();
GUI.backgroundColor = new Color(1f, 0.56f, 0.31f); // اللون باهت وزاكي
if (GUILayout.Button("Load Main Menu", buttonStyle))
{
    // اتخاذ الإجراءات المناسبة عند النقر على الزر
    if(Application.isPlaying)
    {
        GameManager.Load_Scene(Car_Saler_Simulator_Scenes.Main_Menu);
    }
    else 
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }
}

GUI.backgroundColor = new Color(0.71f, 0.49f, 0.86f); // اللون باهت وزاكي
if (GUILayout.Button("Load Game_Room", buttonStyle))
{
    // اتخاذ الإجراءات المناسبة عند النقر على الزر
    if(Application.isPlaying)
    {
        GameManager.Load_Scene(Car_Saler_Simulator_Scenes.Game_Room);
    }
    else 
    {
         UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Game Room.unity");
    }
}
GUILayout.EndHorizontal();
#endregion


#region Pun2 Tester
GUILayout.BeginHorizontal();
           GUI.backgroundColor =  new Color(0.0f, 0.75f, 1.0f); // Bright Blue
           
if(GUILayout.Button("Pun Test",buttonStyle))
{
    UnityEditor.EditorBuildSettings.scenes = new UnityEditor.EditorBuildSettingsScene[]
    {
        new UnityEditor.EditorBuildSettingsScene("Assets/Scenes/Game Room.unity", true),
        new UnityEditor.EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true)
    };

    if(UnityEditor.EditorApplication.isPlaying == false)
    {
        UnityEditor.EditorApplication.isPlaying = true;
    }

    // Join lobby and auto room
    FindAnyObjectByType<GameManager>().StartCoroutine(JoinLobbyAndRoom());
}

IEnumerator JoinLobbyAndRoom()
{
    // Wait for a short delay to ensure scene loading
    yield return new WaitForSeconds(1.0f);
    
    while (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            yield return null;
        }

        while (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            yield return null;
        }

    // Find the ConnectToServer script
    ConnectToServer co = ConnectToServer.instance;
    if (co != null)
    {
        while (!PhotonNetwork.InRoom)
        {
            co.JoinAutoRoom();
            yield return null;
        }
    }
}
       
       GUI.backgroundColor = new Color(1.0f, 0.5f, 0.5f);
       if(GUILayout.Button("Disconnect",buttonStyle))
        {
            
        }
GUILayout.EndHorizontal();
#endregion


GUILayout.BeginHorizontal();
            GUI.backgroundColor =  new Color(1.0f, 0.25f, 0.25f); // Vivid Red

            if (GUILayout.Button("Save Data"))
           {
              FindObjectOfType<GameManager>().Save();
           }
             
            GUI.backgroundColor =  new Color(1.0f, 0.92f, 0.016f); // Vivid Yellow

            if (GUILayout.Button("Load Data"))
           {
              FindObjectOfType<GameManager>().Load();
           }
           GUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
    }
    #endif
    #endregion


    #region ///======================================= Serialize
    public static GameManager instance;
    [System.Serializable]
    public class A_Translation
    {
        public Animator Panel;
        public float Translation_Deally = 0.2f;
    }

     [SerializeField] public A_Translation Loading;

    [System.Serializable]
    public class Screw_game_Data
    {
        public string Player_Name = " ﺐﻋﻼﻟا ";
        public bool Sound = true;
    }

    public static Screw_game_Data informations
    {get
    {
        if(!GameManager.instance) return FindAnyObjectByType<GameManager>().m_informations; return GameManager.instance.m_informations;
    }
    }

    public Screw_game_Data m_informations;
    #endregion ///=======================================


    #region ///======================================= Perpare
    private void Awake() 
    {
        if(!GameManager.instance)
        {
            GameManager.instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }

        Load();
    }
    #endregion


    #region  ///======================================= Translation
    public static void Translation(Translation_Type _Type,float Deally,UnityAction action = null,float wait_to_start_translation = 0f)
    {
        instance.StartCoroutine(instance.start_Translation(_Type,Deally,action,wait_to_start_translation));
    }

    public IEnumerator start_Translation(Translation_Type _Type,float Deally,UnityAction action ,float wait_to_start_translation)
    {
        A_Translation tr = Get_Translation(_Type);
        yield return new WaitForSeconds(wait_to_start_translation);
        tr.Panel.SetBool("Active",true);
        yield return new WaitForSeconds(tr.Translation_Deally);
        if(action != null) action();
        yield return new WaitForSeconds(Deally);
        tr.Panel.SetBool("Active",false);
    }

    private A_Translation Get_Translation(Translation_Type _Type)
    {
         return Loading;
    }
    #endregion
    


    #region  ///======================================= Load Scene
    public static void Load_Scene(Car_Saler_Simulator_Scenes _scene,Translation_Type translation_Type = Translation_Type.Loading,UnityAction on_load_scene = null)
    {
        instance.StartCoroutine(instance.start_Load_Scene(_scene,translation_Type,on_load_scene));
    }

    public static void Load_Pun_Scene(Car_Saler_Simulator_Scenes _scene,Translation_Type translation_Type = Translation_Type.Loading,UnityAction on_load_scene = null)
    {
        instance.StartCoroutine(instance.start_Pun_Load_Scene(_scene,translation_Type,on_load_scene));
    }

    public IEnumerator start_Load_Scene(Car_Saler_Simulator_Scenes _scene,Translation_Type translation_Type = Translation_Type.Loading,UnityAction on_load_scene = null)
    {
        string sceneName = Get_Scene(_scene);
        A_Translation tr = Get_Translation(translation_Type);
        tr.Panel.SetBool("Active", true);
        yield return new WaitForSeconds(tr.Translation_Deally);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        tr.Panel.SetBool("Active", false);

        if(on_load_scene != null) on_load_scene();
    }

    public IEnumerator start_Pun_Load_Scene(Car_Saler_Simulator_Scenes _scene, Translation_Type translation_Type = Translation_Type.Loading, UnityAction on_load_scene = null)
    {
        string sceneName = Get_Scene(_scene);
        A_Translation tr = Get_Translation(translation_Type);
        tr.Panel.SetBool("Active", true);
        yield return new WaitForSeconds(tr.Translation_Deally);

        // Load the scene synchronously using Photon
        PhotonNetwork.LoadLevel(sceneName);

        // Wait for a brief moment before continuing (simulating asynchronous loading)
        yield return new WaitForSeconds(0.1f);

        tr.Panel.SetBool("Active", false);

        GameObject playerGO = PhotonNetwork.Instantiate(Resources_Manager.Instance._Game_Play_Manager.gameObject.name, Vector3.zero, Quaternion.identity);

        // Invoke the callback if provided
        if (on_load_scene != null)
        {
            on_load_scene.Invoke();
        }
    }

    public string Get_Scene(Car_Saler_Simulator_Scenes _Scenes)
    {
        switch (_Scenes)
        {
            case Car_Saler_Simulator_Scenes.Game_Room : return("Game Room");
        }
        return "MainMenu";
    }

    public Car_Saler_Simulator_Scenes Get_Active_Scene_Typ()
    { 
          if(SceneManager.GetActiveScene().name == "Game Room")
          return Car_Saler_Simulator_Scenes.Game_Room;
          
          return Car_Saler_Simulator_Scenes.Main_Menu;
    }
    #endregion

    
    #region  ///======================================= Save System
    public void Save()
   {
    string __data_json = JsonUtility.ToJson(m_informations);
    PlayerPrefs.SetString("Screw_Game_Data", __data_json);
   }

   public void Load()
   {
    string __data_json = PlayerPrefs.GetString("Screw_Game_Data", "");

    if (!string.IsNullOrEmpty(__data_json))
        m_informations = JsonUtility.FromJson<Screw_game_Data>(__data_json);
    else
        m_informations = new Screw_game_Data();
        m_informations.Player_Name = (int) Random.RandomRange(100,2000) + " ﺐﻋﻼﻟا ";
   }
    #endregion

}
