using UnityEngine;
using Photon.Pun;

public class SelangManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform pointSpawn;
    private bool isSelangInstantiated = false; // Flag untuk menandai apakah objek sudah diinstantiate

    [SerializeField] bool isSelangTirisan;
    #region Members Of GameObject Find
    GameObject areaTrigger;
    GameObject selang_panjang;
    GameObject selang_tirisan;
    #endregion
    private void Start()
    {
        // Cari areaTrigger hanya pada pemain lokal
        if (photonView.IsMine)
        {
            areaTrigger = FindInactiveObjectByName("Area Menggulung Selang");            
        }

        // Mengeksekusi RPC hanya pada pemain lokal
        if (photonView.IsMine)
        {
            photonView.RPC("ExecuteAreaTriggerOnAllPlayers", RpcTarget.All);
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (isSelangTirisan)
            {
                if (selang_panjang == null)
                {
                    selang_panjang = FindInactiveObjectByName("Selang Fire Hose New(Clone)");
                    photonView.RPC("ExecuteSelangPanjangOnAllPlayers", RpcTarget.All);
                }
                
            }
            else
            {
                if (selang_tirisan == null)
                {
                    selang_tirisan = FindInactiveObjectByName("Selang Tirisan Air(Clone)");
                    photonView.RPC("ExecuteSelangTirisanOnAllPlayers", RpcTarget.All);
                }
            }
        }
    }
    [PunRPC]
    private void ExecuteAreaTriggerOnAllPlayers()
    {
        // Pencarian objek areaTrigger
        areaTrigger = FindInactiveObjectByName("Area Menggulung Selang");        
    }
    [PunRPC]
    private void ExecuteSelangTirisanOnAllPlayers()
    {
        selang_tirisan = FindInactiveObjectByName("Selang Tirisan Air(Clone)");
    }
    [PunRPC]
    private void ExecuteSelangPanjangOnAllPlayers()
    {
        selang_panjang = FindInactiveObjectByName("Selang Fire Hose New(Clone)");
    }
    private GameObject FindInactiveObjectByName(string name)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.hideFlags == HideFlags.None && t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah objek sudah diinstantiate dan apakah pemain ini adalah master client
        if (!isSelangInstantiated && PhotonNetwork.IsMasterClient)
        {
            if (other.CompareTag("SelangGulung"))
            {
                TwoHandSelang selang = other.gameObject.GetComponent<TwoHandSelang>();

                if (selang.GetIsDrop())
                {
                    Destroy(other.gameObject);
                    GameObject newSelang = PhotonNetwork.Instantiate("Selang Fire Hose", pointSpawn.position, Quaternion.identity);
                    newSelang.transform.Rotate(0, 180, 0);
                    // Set flag menjadi true setelah objek dihancurkan
                    isSelangInstantiated = true;                                                  
                    // Panggil RPC untuk menjalankan kode di seluruh pemain dan hancurkan objek
                    photonView.RPC("DestroyObject", RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
                }
            }

            if (other.CompareTag("SelangPanjang"))
            {
                PhotonNetwork.Instantiate("Selang Gulung", pointSpawn.position, Quaternion.identity);
                photonView.RPC("DestroyObjectSelang", RpcTarget.All, selang_tirisan.GetPhotonView().ViewID);
                isSelangInstantiated = true; // Set flag menjadi true setelah objek dihancurkan

                photonView.RPC("DestroyObject", RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
            }

            if(other.CompareTag("SelangTirisanAir"))
            {
                PhotonNetwork.Instantiate("Selang Tirisan Air", pointSpawn.position, Quaternion.identity);
                photonView.RPC("DestroyObjectSelang", RpcTarget.All, selang_panjang.GetPhotonView().ViewID);
                isSelangInstantiated = true; // Set flag menjadi true setelah objek dihancurkan

                if (areaTrigger != null)
                {
                    areaTrigger.SetActive(true);
                    // Panggil RPC untuk mengaktifkan area trigger di seluruh klien
                    photonView.RPC("SetAreaTriggerActive", RpcTarget.All, true);
                }

                photonView.RPC("DestroyObject", RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
            }
        }
    }
    // RPC method untuk menghancurkan objek pada seluruh pemain
    [PunRPC]
    private void DestroyObjectSelang(int viewID)
    {
        // Dapatkan objek berdasarkan PhotonViewID
        GameObject obj = PhotonView.Find(viewID).gameObject;

        // Hancurkan objek lokal
        Destroy(obj);
        Destroy(gameObject);
        // Set flag menjadi true setelah objek dihancurkan
        isSelangInstantiated = true;

        Debug.Log("Object destroyed on all players.");
    }
    // RPC method untuk menghancurkan objek pada seluruh pemain
    [PunRPC]
    private void DestroyObject(int viewID)
    {
        // Dapatkan objek berdasarkan PhotonViewID
        GameObject obj = PhotonView.Find(viewID).gameObject;

        // Hancurkan objek lokal
        Destroy(obj);
        Destroy(gameObject);
        // Set flag menjadi true setelah objek dihancurkan
        isSelangInstantiated = true;

        Debug.Log("Object destroyed on all players.");
    }
    [PunRPC]
    private void SetAreaTriggerActive(bool isActive)
    {
        if (areaTrigger != null)
        {
            areaTrigger.SetActive(isActive);
        }
    }
}
