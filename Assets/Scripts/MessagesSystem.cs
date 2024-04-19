using TMPro;
using UnityEngine;

public class MessagesSystem : MonoBehaviour
{
    #region   ///======================================= Editor Gui
    #if UNITY_EDITOR
     [UnityEditor.CustomEditor(typeof(MessagesSystem))]
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


    public static MessagesSystem instance;



    [Header("Error Message")]
    [SerializeField] TextMeshProUGUI _Message;
    [SerializeField] Animator _Message_Panel;


    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }


    #region Messages
    public void Show_Message(string message,Color color)
    {
       _Message.text = message;
       _Message.color = color;
       _Message_Panel.SetTrigger("Show");
    }
    #endregion
}
