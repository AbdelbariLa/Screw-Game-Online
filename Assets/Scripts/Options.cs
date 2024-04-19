using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{

    #region   ///======================================= Editor Gui
    #if UNITY_EDITOR
     [UnityEditor.CustomEditor(typeof(Options))]
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

    public static Options instance;
    Animator an;
    [SerializeField] TMP_InputField input_player_name;
    [SerializeField] Screw_Button Close_b;
    [SerializeField] Screw_Button Sound;
    [SerializeField] Sprite Sound_Of,Sound_On;

    [SerializeField] Screw_Button Return_b;

    #region ///======================================= Perpare
    private void Awake() 
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        an = GetComponent<Animator>();
        Sound.On_Click.AddListener(Shange_Sound_Value);
        input_player_name.onEndEdit.AddListener(delegate{On_Shange_Player_Name();});
        Close_b.On_Click.AddListener(Close_Option_Panel);

        Return_b.On_Click.AddListener(
            delegate{
              ConnectToServer.instance.Leave_Room();
              GameManager.Load_Scene(Car_Saler_Simulator_Scenes.Main_Menu);
            });
    }
    #endregion

    #region Actions
    public void Active_Option_Panel(bool active = true)
    {
        an.SetBool("Active", active);

        if(active)
        {
            Return_b.gameObject.SetActive(GameManager.instance.Get_Active_Scene_Typ() == Car_Saler_Simulator_Scenes.Game_Room);
           input_player_name.text = GameManager.informations.Player_Name;
           Sound.GetComponent<Image>().sprite = GameManager.informations.Sound? Sound_On : Sound_Of;
        }
    }

    private void Shange_Sound_Value()
    {
        GameManager.informations.Sound = !GameManager.informations.Sound;

        Sound.GetComponent<Image>().sprite = GameManager.informations.Sound? Sound_On : Sound_Of;
    }

    private void On_Shange_Player_Name()
    {
         // Ensure the player name length is between 0 and 10 characters
         string playerName = input_player_name.text;
         playerName = playerName.Substring(0, Mathf.Clamp(playerName.Length, 0, 10));
    
         // Update the player's name in the GameManager
         GameManager.informations.Player_Name = playerName;

         // Update the input field with the clamped player name
         input_player_name.text = playerName;
    }

    private void Close_Option_Panel()
    {
        Active_Option_Panel(false);
        GameManager.instance.Save();
    }
    #endregion
}
