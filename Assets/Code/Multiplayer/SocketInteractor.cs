using Photon.Pun;
using UnityEngine;
using System.Collections;
public class SocketInteractor : MonoBehaviourPunCallbacks
{
    [PunRPC]
    void DisableSocketInteractor(int viewID)
    {
        // Dapatkan objek NetworkXRSocketInteractor berdasarkan ID PhotonView-nya
        GameObject obj = PhotonView.Find(viewID).gameObject;
        NetworkXRSocketInteractor xRSocket = obj.GetComponent<NetworkXRSocketInteractor>();

        // Nonaktifkan socket interactor hanya pada objek ini
        xRSocket.socketActive = false;

        // Panggil coroutine untuk menunggu sebelum mengaktifkan socket kembali
        StartCoroutine(ReactivateSocket(xRSocket));
    }

    // Coroutine untuk menunggu dan mengaktifkan kembali socket
    IEnumerator ReactivateSocket(NetworkXRSocketInteractor socket)
    {
        yield return new WaitForSeconds(1f); // Tunggu 1 detik

        // Aktifkan kembali socket
        socket.socketActive = true;
    }

    // Fungsi untuk mengambil isi dari socket
    public void TakeSocketContents()
    {
        // Panggil fungsi RPC untuk menonaktifkan socket interactor pada semua pemain
        photonView.RPC("DisableSocketInteractor", RpcTarget.All, GetComponent<PhotonView>().ViewID);
        // Panggil fungsi RPC untuk memberitahu semua pemain bahwa isi socket telah diambil
        photonView.RPC("OnSocketContentsTaken", RpcTarget.All);
    }
}
