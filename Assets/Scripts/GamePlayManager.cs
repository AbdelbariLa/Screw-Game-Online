using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;

public class GamePlayManager : MonoBehaviourPunCallbacks
{

    #region Editor GUI
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(GamePlayManager))]
    public class MyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);
            GameManager.Screw_Gui();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.white;

            GamePlayManager gamePlayManager = (GamePlayManager)target;

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(1.0f, 0.25f, 0.25f); // Vivid Red

            /*if (GUILayout.Button("Show Card"))
            {
                cardController.Show_Card();
            }*/

            GUI.backgroundColor = new Color(1.0f, 0.92f, 0.016f); // Vivid Yellow

            /*if (GUILayout.Button("Hide Card"))
            {
                cardController.Hide_Card();
            }*/
            GUILayout.EndHorizontal();

            GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
            boldStyle.fontStyle = FontStyle.Bold;
            boldStyle.normal.textColor = Color.white; // Set text color to white

            boldStyle.normal.textColor = PhotonNetwork.IsMasterClient?Color.green:Color.red;
            UnityEditor.EditorGUILayout.LabelField("Game Is Master Client:", PhotonNetwork.IsMasterClient ? "Yes" : "No", boldStyle);

            boldStyle.normal.textColor = gamePlayManager.Exist_All_Players?Color.green:Color.red;
            UnityEditor.EditorGUILayout.LabelField("Exist All Players In Room:", gamePlayManager.Exist_All_Players ? "Yes" : "No", boldStyle);
            
            boldStyle.normal.textColor = gamePlayManager.Game_Is_Started?Color.green:Color.red;
            UnityEditor.EditorGUILayout.LabelField("Game Is Started:", gamePlayManager.Game_Is_Started ? "Yes" : "No", boldStyle);

            GUI.backgroundColor = Color.white;
            base.OnInspectorGUI();
        }
    }
    #endif
    #endregion


    #region Serialize
    [Header("Waiting Players")]
    [SerializeField] Animator Waiting_Players_Panel;
    [SerializeField] TextMeshProUGUI Waiting_Players_Text;
    [HideInInspector] public bool Exist_All_Players = false;
    [HideInInspector] public bool Game_Is_Started = false;

    [Header("A Player Left Rool")]
    [SerializeField] Animator A_Player_Left_Room_Panel;

    PhotonView photonView;
    #endregion


    #region Prepare ==================================>
    private IEnumerator Start()
    {
        Game_Is_Started = false;
        Exist_All_Players = false;
        
        if(Is_Mine())
      {
        Active_Panel(GetComponent<Animator>(),true);

        On_Shange_Players_Numbre();

        Active_Panel(Waiting_Players_Panel,true);

        while (!Game_Is_Started)
        {
            Waiting_Players_Text.text = PhotonNetwork.CurrentRoom.PlayerCount+"/"+PhotonNetwork.CurrentRoom.MaxPlayers+" : ﻦﻴﺒﻋﻼﻟا رﺎﻈﺘﻧإ";
            yield return new WaitForEndOfFrame();

            if(PhotonNetwork.IsMasterClient && Exist_All_Players)
            {
              Debug.Log("IsMasterClient : Game_Is_Started");
              photonView.RPC("Start_Game", RpcTarget.AllBuffered);
            }
        }
         
        yield return new WaitForSeconds(1f);
        Active_Panel(Waiting_Players_Panel,false); 
      }
    }
    #endregion


    #region Actions ==================================>
    private void Active_Panel(Animator an , bool active)
    {
        an.SetBool("Active",active);
    }
    private void On_Shange_Players_Numbre()
    {
         Exist_All_Players = PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    public bool Is_Mine()
    {
        if(!photonView)
        photonView = GetComponent<PhotonView>();
        return photonView.IsMine;
    }
    #endregion



    #region Update ==================================>

    #endregion


    #region  [PunRPC] ==================================>
    
    [PunRPC]
    public void Start_Game()
    {
        if(!Is_Mine()) return;

        Debug.Log("[PunRPC] : Game_Is_Started");
        Game_Is_Started = true;
    }
    #endregion


    #region Join // Leave Players ==================================>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    { 
        if(!Is_Mine()) return;

        base.OnPlayerEnteredRoom(newPlayer);
        On_Shange_Players_Numbre();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(!Is_Mine()) return;
        
       base.OnPlayerLeftRoom(otherPlayer);
       On_Shange_Players_Numbre();

       if(Game_Is_Started) /// اذا غادر احد اللاعبين اللعبة أثناء اللعب يتم الخروج من اللعبة
       {
          PhotonNetwork.LeaveRoom();
          Active_Panel(A_Player_Left_Room_Panel,true);
       }
    }
    #endregion
}
