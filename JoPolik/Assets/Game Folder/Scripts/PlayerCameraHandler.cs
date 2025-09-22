using UnityEngine;
using Photon.Pun;

public class PlayerCameraHandler : MonoBehaviourPun
{
    void Start()
    {
        Camera cam = GetComponentInChildren<Camera>();
        AudioListener listener = GetComponentInChildren<AudioListener>();

        if (!photonView.IsMine)
        {
            if (cam != null) cam.enabled = false;
            if (listener != null) listener.enabled = false;
        }
    }
}
