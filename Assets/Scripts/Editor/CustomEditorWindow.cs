using System.Collections;
using Photon.Pun;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CustomEditorWindow : EditorWindow
{
    [MenuItem("Tools/Open Custom Editor Window")]
    public static void OpenWindow()
    {
        CustomEditorWindow window = GetWindow<CustomEditorWindow>("Custom Editor");
        window.minSize = new Vector2(400, 150); // تم تبديل الأبعاد لتناسب التصميم المطلوب
    }

    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

    private void OnGUI() 
    {
        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 14;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.fixedHeight = 25; // تم تقليل ارتفاع الأزرار
        buttonStyle.margin = new RectOffset(20, 20, 5, 5); // تم تقليل المسافة بين الأزرار

        GUILayout.Space(10);

        GUILayout.FlexibleSpace(); // يوضع الأزرار في الجهة المركزية
        DrawButton("Load Main Menu", new Color(1f, 0.56f, 0.31f), () =>
        {
            LoadScene(Car_Saler_Simulator_Scenes.Main_Menu);
        });

        DrawButton("Load Game Room", new Color(0.71f, 0.49f, 0.86f), () =>
        {
            LoadScene(Car_Saler_Simulator_Scenes.Game_Room);
        });
        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace();
        DrawButton("Pun Test", new Color(0.0f, 0.75f, 1.0f), () =>
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
        });

        DrawButton("Disconnect", new Color(1.0f, 0.5f, 0.5f), () =>
        {
            PhotonNetwork.Disconnect();
        });
        
        GUILayout.FlexibleSpace();

        GUILayout.FlexibleSpace();
        DrawButton("Save Data", new Color(1.0f, 0.25f, 0.25f), () =>
        {
           FindObjectOfType<GameManager>().Save();
        });
        
        DrawButton("Load Data", new Color(1.0f, 0.92f, 0.016f), () =>
        {
            FindObjectOfType<GameManager>().Load();
        });
        GUILayout.FlexibleSpace();
        
    }

    private void DrawButton(string buttonText, Color buttonColor, System.Action onClick)
    {
        GUI.backgroundColor = buttonColor;

        if (GUILayout.Button(buttonText, buttonStyle)) // استخدام buttonStyle بدلاً من GUILayout.Height(25)
        {
            onClick?.Invoke();
        }
    }

    private void LoadScene(Car_Saler_Simulator_Scenes scene)
    {
        string scenePath = scene == Car_Saler_Simulator_Scenes.Main_Menu ? "Assets/Scenes/Game Room.unity" : "Assets/Scenes/MainMenu.unity";
        if (Application.isPlaying)
        {
            GameManager.Load_Scene(scene);
        }
        else
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
