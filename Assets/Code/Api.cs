using UnityEngine;
using Photon.Pun;
using System;

public class Api : MonoBehaviourPun
{
    // Public Variables
    [SerializeField] private float HP_api; // Current HP of the object

    // Private Variables
    private float initialHP; // Stores the initial HP value for percentage calculations

    #region Unity Methods

    private void Start()
    {
        // Initialize initialHP with the starting HP value
        initialHP = HP_api;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if HP is zero or less
        if (HP_api <= 0)
        {
            if (photonView.IsMine)
            {
                // Call RPC to destroy the object on all clients
                photonView.RPC("DestroyObject", RpcTarget.AllBuffered);
            }
        }
    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// Checks if the HP has reached a certain percentage of the initial value.
    /// </summary>
    /// <param name="persen">The percentage of the initial HP.</param>
    void CanDestroyApi(float persen)
    {
        // Check if HP is less than or equal to a certain percentage of the initial HP
        if (HP_api <= initialHP * persen)
        {
            HP_api = 0; // Set HP to 0 to trigger destruction
        }
    }

    #endregion

    #region PunRPC Methods

    /// <summary>
    /// Destroys the object on all clients.
    /// </summary>
    [PunRPC]
    void DestroyObject()
    {
        // Destroy the object on all clients
        PhotonNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// Reduces the HP by a certain amount on all clients.
    /// </summary>
    /// <param name="hp">The amount of HP to reduce.</param>
    [PunRPC]
    public void RPC_SetPenguranganApiHP(float hp)
    {
        HP_api -= hp;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Reduces the HP by a certain amount locally.
    /// </summary>
    /// <param name="hp">The amount of HP to reduce.</param>
    public void SetPenguranganApiHP(float hp) => HP_api -= hp;

    #endregion

    #region TODO
    // TODO: Add additional methods for handling different scenarios related to HP management.
    // TODO: Implement health regeneration if necessary.
    // TODO: Add visual or audio feedback when HP is reduced or the object is destroyed.
    #endregion
}
