using TMPro;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    #region   ///======================================= Editor Gui
    #if UNITY_EDITOR
     [UnityEditor.CustomEditor(typeof(MainMenu))]
    public class MyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GameManager.Screw_Gui();
            base.OnInspectorGUI();
        }
    }
    #endif
    #endregion


    [Header("Start")]
    [SerializeField] Screw_Button start_b;
    [SerializeField] Screw_Button Retrun_b;
    [SerializeField] Animator Start_Panel;

    [Header("Looby")]
    [SerializeField] Animator Looby_Panel;
    [SerializeField] Screw_Button Join_Room_b;
    [SerializeField] TMP_InputField Join_Room_input;
    [SerializeField] Screw_Button Creat_Room_b;
    [SerializeField] TMP_InputField creat_room_name_input;
    [SerializeField] TMP_Dropdown Max_Players;
    [SerializeField] TMP_Dropdown Rounds_Count;

    [Header("Auto Room")]
    [SerializeField] Screw_Button Join_Auto_Room_2players_b;
    [SerializeField] Screw_Button Join_Auto_Room_4players_b;

    public static MainMenu instance { get { return (FindFirstObjectByType<MainMenu>());} }

    
    

    private void Start() 
    {
        start_b.On_Click.AddListener(delegate{ConnectToServer.instance.JoinToLobby();});
        Retrun_b.On_Click.AddListener(Retrun_Looby);
        Active_Current_Pane(Start_Panel);

        Join_Room_b.On_Click.AddListener(delegate{ConnectToServer.instance.JoinRoom(Join_Room_input.text);});

        Creat_Room_b.On_Click.AddListener(delegate
        {
            int rounds = Rounds_Count.value + 1;
            int max_players = Max_Players.value == 0 ? 2:4;
            ConnectToServer.instance.CreatRoom(creat_room_name_input.text,rounds,max_players);
        }
        );

        Join_Auto_Room_2players_b.On_Click.AddListener(delegate{ConnectToServer.instance.JoinAutoRoom();});
        Join_Auto_Room_2players_b.On_Click.AddListener(delegate{ConnectToServer.instance.JoinAutoRoom();});
    }
 

    #region  Helper Actions
    private void Open_Panel(Animator an,bool Active = true)
    {
        an.SetBool("Active", Active);
    }

    public void Active_Current_Pane(Animator an)
    {
        Open_Panel(Start_Panel,an == Start_Panel);
        Open_Panel(Looby_Panel,an == Looby_Panel);
        Options.instance.Active_Option_Panel(false);
    }
    #endregion


    #region Enter Looby
    public void EnterLobby()
    {
        Active_Current_Pane(Looby_Panel);
    }

    private void Retrun_Looby()
    {
        Active_Current_Pane(Start_Panel);
    }
    #endregion
}
