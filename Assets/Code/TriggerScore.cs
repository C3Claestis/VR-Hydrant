using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class TriggerScore : MonoBehaviourPun
{
    #region Serialized Fields

    [Header("Core Components")]
    [SerializeField] private GameObject correct; // Object indicating a correct action
    [SerializeField] private ScoreManager scoreManager; // Reference to ScoreManager

    [Header("Decoy Objects")]
    [SerializeField] private GameObject nozzelDecoy;
    [SerializeField] private GameObject selangDecoy;
    [SerializeField] private GameObject handleDecoy;

    [Header("UI Elements")]
    [SerializeField] private GameObject Finish_UI; // UI shown on completion

    #region Members Api    
    [SerializeField] private GameObject colision_selang; // Collision object for hose
    [SerializeField] private TriggerScore anotherTrigger; // Reference to another TriggerScore
    #endregion

    [Header("Trigger Objects")]
    [SerializeField] private GameObject TriggerObject;
    [SerializeField] private GameObject AreaSelangGulung; // Area for rolling the hose
    [SerializeField] private GameObject xrKnob; // Knob for XR interaction

    #endregion

    #region Public Fields

    public bool isFinishing; // Flag indicating finishing state
    public bool isComplete; // Flag indicating completion state
    public bool isApi; // Flag indicating API state
    public bool isCollision; // Flag indicating collision state
    public bool isWater; // Flag indicating water state

    #endregion

    #region Public Methods

    public void SetIsComplete(bool value)
    {
        isComplete = value;
        // Call RPC to synchronize completion status
        photonView.RPC("SyncCompleteStatus", RpcTarget.All, value);
    }

    #endregion

    #region Unity Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Nozzel")
        {
            if (isFinishing)
            {
                // Call RPC to activate TriggerObject and AreaSelangGulung
                photonView.RPC("ActivateTriggerObjectAndArea", RpcTarget.All);
            }

            // Call RPC to set correct status and trigger score upgrade
            photonView.RPC("ActivateCorrectAndUpgradeScore", RpcTarget.All);

            // Destroy this game object on all clients
            photonView.RPC("DestroyGameObject", RpcTarget.All);
        }
    }

    private void Update()
    {
        if (isApi && transform.childCount == 0 && photonView.IsMine)
        {
            // Call RPC to set correct status, colision_selang and trigger score upgrade
            photonView.RPC("ActivateApiAndUpgradeScore", RpcTarget.All);

            // Call RPC to set anotherTrigger as complete
            photonView.RPC("SetAnotherTriggerComplete", RpcTarget.All, true);

            // Destroy this game object on all clients
            photonView.RPC("DestroyGameObject", RpcTarget.All);
        }

        if (isComplete && photonView.IsMine)
        {
            if (!nozzelDecoy.activeSelf && !selangDecoy.activeSelf && !handleDecoy.activeSelf)
            {
                // Call RPC to activate Finish_UI, correct and trigger score upgrade
                photonView.RPC("CompleteAndUpgradeScore", RpcTarget.All);

                // Destroy this game object on all clients
                photonView.RPC("DestroyGameObject", RpcTarget.All);
            }
        }

        if (isWater && xrKnob.activeSelf && photonView.IsMine)
        {
            // Call RPC to set correct status and trigger score upgrade
            photonView.RPC("ActivateCorrectAndUpgradeScore", RpcTarget.All);
        }
    }

    #endregion

    #region PunRPC Methods

    [PunRPC]
    void SyncCompleteStatus(bool syncedStatus)
    {
        // Synchronize completion status on all clients
        isComplete = syncedStatus;
        if (isComplete)
        {
            Finish_UI.SetActive(true);
            correct.SetActive(true);
            scoreManager.SetIsUpgrade(true);
            Destroy(gameObject);
        }
    }

    [PunRPC]
    void ActivateTriggerObjectAndArea()
    {
        TriggerObject.SetActive(true);
        AreaSelangGulung.SetActive(true);
    }

    [PunRPC]
    void ActivateCorrectAndUpgradeScore()
    {
        correct.SetActive(true);
        scoreManager.SetIsUpgrade(true);
    }

    [PunRPC]
    void ActivateApiAndUpgradeScore()
    {
        correct.SetActive(true);
        colision_selang.SetActive(true);
        scoreManager.SetIsUpgrade(true);
    }

    [PunRPC]
    void SetAnotherTriggerComplete(bool value)
    {
        anotherTrigger.SetIsComplete(value);
    }

    [PunRPC]
    void CompleteAndUpgradeScore()
    {
        Finish_UI.SetActive(true);
        correct.SetActive(true);
        scoreManager.SetIsUpgrade(true);
    }

    [PunRPC]
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    #endregion

    #region TODO

    // TODO: Add error handling for null references to avoid potential crashes.
    // TODO: Optimize the Update method to reduce unnecessary checks.
    // TODO: Add detailed logging for debugging purposes.
    // TODO: Consider refactoring the code to separate responsibilities more clearly.

    #endregion
}
