using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Unity.XR.CoreUtils;
using TMPro;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] Transform head;
    [SerializeField] Transform right_hand;
    [SerializeField] Transform left_hand;

    PhotonView photonView;

    [SerializeField] Animator leftHandAnimator;
    [SerializeField] Animator rightHandAnimator;

    private Transform headRig;
    private Transform LeftHandRig;
    private Transform RightHandRig;

    [SerializeField] TextMeshProUGUI name_text;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        XROrigin rig = FindObjectOfType<XROrigin>();

        headRig = rig.transform.Find("Camera Offset/Main Camera");
        LeftHandRig = rig.transform.Find("Camera Offset/Left Hand");
        RightHandRig = rig.transform.Find("Camera Offset/Right Hand");

        if (rig == null)
        {
            Debug.Log("XRRIG TIDAK TERAMBIL");
        }

        if (photonView.IsMine)
        {
            string playerName = PlayerPrefs.GetString("NamePlayer");
            photonView.RPC("UpdateNameRPC", RpcTarget.AllBuffered, playerName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(left_hand, LeftHandRig);
            MapPosition(right_hand, RightHandRig);

            UpdateHandAnimator(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimator(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }

    void UpdateHandAnimator(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    [PunRPC]
    void UpdateNameRPC(string playerName)
    {
        name_text.text = playerName;
    }
}
