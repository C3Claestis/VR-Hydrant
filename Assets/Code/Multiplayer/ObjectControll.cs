using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ObjectControll : MonoBehaviourPunCallbacks
{
    // Objek pertama dan kedua yang akan diatur
    [SerializeField] private GameObject object1;
    [SerializeField] private GameObject object2;

    // Panggil fungsi RPC untuk mengatur kedua objek menjadi Untagged
    public void SetObjectsUntagged()
    {
        photonView.RPC("RPC_SetObjectsUntagged", RpcTarget.All);
    }

    // Panggil fungsi RPC untuk mengatur objek menjadi SelangPanjang
    public void SetObjectsSelangPanjang()
    {
        photonView.RPC("RPC_SetObjectsSelangPanjang", RpcTarget.All);
    }

    // RPC method untuk mengatur kedua objek menjadi Untagged
    [PunRPC]
    private void RPC_SetObjectsUntagged()
    {
        // Mengubah tag objek pertama dan kedua menjadi Untagged
        object1.tag = "Untagged";
        object2.tag = "Untagged";
    }

    // RPC method untuk mengatur objek menjadi SelangPanjang
    [PunRPC]
    private void RPC_SetObjectsSelangPanjang()
    {
        // Mengubah tag objek pertama dan kedua menjadi SelangPanjang
        object1.tag = "SelangTirisanAir";
        object2.tag = "SelangTirisanAir";
    }
}
