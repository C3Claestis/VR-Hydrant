using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WaterDestroy : MonoBehaviourPun
{
    [SerializeField] TrajectoryPredictor nozzel;
 
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Api"))
        {
            Api api = other.GetComponent<Api>();
            photonView.RPC("RPC_ReduceApiHP", RpcTarget.AllBuffered, api.photonView.ViewID, nozzel.GetNilaiPemadamanApi());
            Debug.Log("API PADAM");
        }
    }
    [PunRPC]
    public void RPC_ReduceApiHP(int viewID, float nilaiPemadamanApi)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            Api api = targetView.GetComponent<Api>();
            if (api != null)
            {
                api.RPC_SetPenguranganApiHP(nilaiPemadamanApi);
            }
        }
    }
}
