using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class CardController : MonoBehaviour
{

    #region Editor GUI
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(CardController))]
    public class MyEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);
            GameManager.Screw_Gui();
            GUI.backgroundColor = Color.white;

            CardController cardController = (CardController)target;

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(1.0f, 0.25f, 0.25f); // Vivid Red

            if (GUILayout.Button("Show Card"))
            {
                cardController.Show_Card();
            }

            GUI.backgroundColor = new Color(1.0f, 0.92f, 0.016f); // Vivid Yellow

            if (GUILayout.Button("Hide Card"))
            {
                cardController.Hide_Card();
            }
            GUILayout.EndHorizontal();

            GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
            boldStyle.fontStyle = FontStyle.Bold;
            boldStyle.normal.textColor = Color.white; // Set text color to white

            GUIStyle normalStyle = new GUIStyle(GUI.skin.label);
            normalStyle.normal.textColor = Color.green; // Set text color to green


            UnityEditor.EditorGUILayout.LabelField("Showing_card:", cardController.Showing_card ? "Yes" : "No", boldStyle);
            UnityEditor.EditorGUILayout.LabelField("Is Arrival:", cardController.Is_Arrival() ? "Yes" : "No", boldStyle);

            GUI.backgroundColor = Color.white;
            base.OnInspectorGUI();
        }
    }
    #endif
    #endregion


    #region Serialize
    public int Amount = 4;
    public int Points = 1;
    [HideInInspector] public bool Showing_card = false;
    [SerializeField] float Rotation_Speed = 800;
    [SerializeField] float Move_Speed = 10;
    Vector3 target_position;
    Vector3 target_rotation;
    PhotonView photonView;
    #endregion


    #region Prepare
    private void Start()
    {
        target_position = transform.position;
        photonView = GetComponent<PhotonView>();
    }
    #endregion


    #region Actions
    public void Show_Card()
    {
        Showing_card = true;
    }

    public void Hide_Card()
    {
        Showing_card = false;
    }

    public void Move_Card(Vector3 new_target)
    {
        target_position = new_target;
    }

    private void Swap_card()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z); // Adjust z-coordinate based on camera and object position

        // Convert mouse position from screen space to world space
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Update the target_position with the new x and y coordinates
        target_position = new Vector3(newPosition.x, newPosition.y, target_position.z);
    }
    #endregion


    #region Update
    private void FixedUpdate()
    {
        target_rotation.y = Showing_card ? 180 : 0;

        Quaternion targetRotation = Quaternion.Euler(target_rotation);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, Rotation_Speed * Time.fixedDeltaTime);

        transform.position = Vector3.MoveTowards(transform.position, target_position, Move_Speed * Time.fixedDeltaTime);

        if (Input.GetMouseButton(0))
            Swap_card();

        if (photonView.IsMine && !Is_Arrival())
        {
            // If this is the local player and the card hasn't arrived yet
            // Send the position and rotation of the card to other players
            photonView.RPC("SyncCardPositionAndRotation", RpcTarget.OthersBuffered, target_position, target_rotation);
        }
    }

    private bool Is_Arrival()
    {
        return Vector3.Distance(transform.position, target_position) <= 0.02f &&
               Quaternion.Angle(transform.rotation, Quaternion.Euler(target_rotation)) <= 0.1f;
    }
    #endregion

    #region  [PunRPC] ==================================>
    // RPC method to synchronize position and rotation of the card across network
    [PunRPC]
    private void SyncCardPositionAndRotation(Vector3 newPosition, Vector3 newRotation)
    {
        target_position = newPosition;
        target_rotation = newRotation;
    }
    #endregion
}