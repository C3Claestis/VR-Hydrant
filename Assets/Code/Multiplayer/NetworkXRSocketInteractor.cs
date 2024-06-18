using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
public class NetworkXRSocketInteractor : XRSocketInteractor
{
    public SocketInteractor socketInteractor;

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Ketika objek diambil, panggil fungsi TakeSocketContents() dari SocketInteractor
        socketInteractor.TakeSocketContents();
    }
    
}
