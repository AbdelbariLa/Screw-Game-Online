using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Photon_Spawner_Manager : MonoBehaviourPunCallbacks
{
    #region Editor GUI
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Photon_Spawner_Manager))]
    public class MyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);
            GameManager.Screw_Gui();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.white;
            base.OnInspectorGUI();
        }
    }
#endif
    #endregion
}
