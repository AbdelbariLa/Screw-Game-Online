using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class ReadOnlyAttribute : PropertyAttribute { }


public class ConnectToServer : MonoBehaviourPunCallbacks
{

#region ///======================================= Editor Gui
#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(ConnectToServer))]
public class MyEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
        boldStyle.fontStyle = FontStyle.Bold;
        boldStyle.normal.textColor = Color.white; // Set text color to white

        GUIStyle normalStyle = new GUIStyle(GUI.skin.label);
        normalStyle.normal.textColor = Color.green; // Set text color to green

        // أظهر نص: إذا كان متصل بالسيرفر
        UnityEditor.EditorGUILayout.LabelField("Connected to Server:", PhotonNetwork.IsConnected ? "Yes" : "No", boldStyle);

        // أظهر نص: إذا كان متصل بالردهة
        UnityEditor.EditorGUILayout.LabelField("Connected to Lobby:", PhotonNetwork.InLobby ? "Yes" : "No", boldStyle);

        // أظهر نص: إذا كان منضمًا لروم وكل معلومات الروم
        if (PhotonNetwork.InRoom)
        {
            UnityEditor.EditorGUILayout.LabelField("Joined Room:", "Yes", boldStyle);
            UnityEditor.EditorGUILayout.LabelField("Room Name:", PhotonNetwork.CurrentRoom.Name, normalStyle);
            UnityEditor.EditorGUILayout.LabelField("Players in Room:", PhotonNetwork.CurrentRoom.PlayerCount.ToString(), normalStyle);
            UnityEditor.EditorGUILayout.LabelField("Max Players in Room:", PhotonNetwork.CurrentRoom.MaxPlayers.ToString(), normalStyle);
        }
        else
        {
            UnityEditor.EditorGUILayout.LabelField("Joined Room:", "No", boldStyle);
        }
    }
}
#endif
#endregion


    public static ConnectToServer instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    public void JoinToLobby()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        if (!PhotonNetwork.InLobby)
        {
            if(!PhotonNetwork.IsConnected)
            {
                 PhotonNetwork.ConnectUsingSettings();
                 MessagesSystem.instance.Show_Message("Joining To Lobby ...",Color.green);
            }
            else 
            {
                 PhotonNetwork.JoinLobby();
            }
        }
        else
        {
            MainMenu.instance.EnterLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        if(MainMenu.instance)
        MainMenu.instance.EnterLobby();
        Debug.Log("In Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        MessagesSystem.instance.Show_Message("Disconnected from Photon: " + cause.ToString(),Color.red);
    }

    public void CreatRoom(string roomName,int Rounds,int Max_Players)
    { 
        RoomOptions room   = new RoomOptions();
        room.MaxPlayers = Max_Players;
        PhotonNetwork.CreateRoom(roomName,room);
    }

    public void JoinRoom(string roomName)
    {
        // Get the list of available rooms
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinAutoRoom()
    {
        Debug.Log("Joining Auto Room");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("AutoRoom", roomOptions, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        GameManager.Load_Pun_Scene(Car_Saler_Simulator_Scenes.Game_Room);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        GameManager.Load_Pun_Scene(Car_Saler_Simulator_Scenes.Game_Room);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        MessagesSystem.instance.Show_Message(message,Color.red);
    }

    public void Leave_Room()
    {
      PhotonNetwork.LeaveRoom();
    }
}