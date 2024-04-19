using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Screw_Button : MonoBehaviour
{
    
    #region Editor =========================================================  
    #if UNITY_EDITOR
    private void OnValidate() 
    {
        GetComponent<Animator>().runtimeAnimatorController = Resources_Manager.Instance.Screw_Button_Animator;
        GetComponent<Animator>().updateMode  = AnimatorUpdateMode.UnscaledTime;
    }
    #endif 
    #endregion


    #region Ser =========================================================  
    public UnityEvent On_Click = new UnityEvent();
    public enum screw_b_event{other,Options,Disconect_And_Return_To_Main_Menu}
    [SerializeField] screw_b_event screw_B_Event;
    #endregion


    #region Prep =========================================================  
    private void Start() 
    {
        GetComponent<Button>().onClick.AddListener(Operation_Invoke);
        GetComponent<Animator>().runtimeAnimatorController = Resources_Manager.Instance.Screw_Button_Animator;
        GetComponent<Animator>().updateMode  = AnimatorUpdateMode.UnscaledTime;
        
        switch (screw_B_Event)
        {
            case screw_b_event.Options:
            On_Click.AddListener(delegate{Options.instance.Active_Option_Panel();});
            break;
            case screw_b_event.Disconect_And_Return_To_Main_Menu:
            On_Click.AddListener(delegate{
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            GameManager.Load_Scene(Car_Saler_Simulator_Scenes.Main_Menu);
            });
            break;
        }
    }
    #endregion
    

    #region Acti =========================================================  
    private void Operation_Invoke()
    {
        StartCoroutine(Op());
        IEnumerator Op()
        {
            GetComponent<Animator>().SetTrigger("On_Click");
            yield return new WaitForSeconds(0.3f);
            On_Click.Invoke();
        }
    }
    #endregion
}
