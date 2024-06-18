using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using Photon.Pun;

public class TrajectoryPredictor : MonoBehaviourPunCallbacks
{
    [SerializeField] private ParticleSystem _airParticle; // Sistem partikel untuk menampilkan efek udara 
    [SerializeField] private XRKnob knob; // Tombol putar untuk mengontrol kecepatan air
    private bool IsReadyWoosh = false; // Status kesiapan untuk mengeluarkan suara "woosh"

    [SerializeField] private float nilaiPemadamanApi;
    public float GetNilaiPemadamanApi() => nilaiPemadamanApi;

    PhotonView photon;
    private void Start()
    {
        photon = GetComponent<PhotonView>();
        /*knob.onValueChange.AddListener(OnKnobValueChanged); // Listen for knob value changes*/
    }

    // Dipanggil setiap frame
    private void Update()
    {
        knob.onValueChange.AddListener(OnKnobValueChanged); // Listen for knob value changes

        // Jika siap untuk "woosh"
        if (IsReadyWoosh)
        {
            // Mengaktifkan sistem partikel
            _airParticle.gameObject.SetActive(true);
            photon.RPC("AirUpdate", RpcTarget.All, true);

            // Mengatur kecepatan partikel berdasarkan nilai knob
            UpdateAirParticle(knob.value);
        }
        else // Jika tidak siap untuk "woosh"
        {
            // Menonaktifkan sistem partikel
            _airParticle.gameObject.SetActive(false);
            photon.RPC("AirUpdate", RpcTarget.All, false);
        }
    }

    private void OnKnobValueChanged(float value)
    {
        photon.RPC("UpdateKnobValue", RpcTarget.All, value); // Send knob value change to all clients
    }

    [PunRPC]
    private void UpdateKnobValue(float value)
    {
        knob.SetValueWithoutNotify(value); // Update knob value without triggering listeners
        UpdateAirParticle(value); // Update air particle properties based on the new value
    }

    private void UpdateAirParticle(float knobValue)
    {
        // Mengatur kecepatan partikel berdasarkan nilai knob
        var emisionModule = _airParticle.emission;
        var velocityOver = _airParticle.velocityOverLifetime;

        if (knobValue >= 0.1f && knobValue < 0.25f)
        {
            emisionModule.rateOverTime = 10;
            velocityOver.z = new ParticleSystem.MinMaxCurve(10);
            nilaiPemadamanApi = 0.1f;
        }
        else if (knobValue >= 0.5f && knobValue < 0.55f)
        {
            emisionModule.rateOverTime = 50;
            velocityOver.z = new ParticleSystem.MinMaxCurve(50);
            nilaiPemadamanApi = 1f;
        }
        else if (knobValue >= 0.65f && knobValue < 0.75f)
        {
            emisionModule.rateOverTime = 100;
            velocityOver.z = new ParticleSystem.MinMaxCurve(100);
            nilaiPemadamanApi = 1.5f;
        }
        else if (knobValue >= 0.75f && knobValue < 0.85f)
        {
            emisionModule.rateOverTime = 150;
            velocityOver.z = new ParticleSystem.MinMaxCurve(150);
            nilaiPemadamanApi = 1.75f;
        }
        else if (knobValue >= 0.85f && knobValue <= 1f)
        {
            emisionModule.rateOverTime = 250;
            velocityOver.z = new ParticleSystem.MinMaxCurve(250);
            nilaiPemadamanApi = 2.5f;
        }
    }

    [PunRPC]
    private void AirUpdate(bool value)
    {
        _airParticle.gameObject.SetActive(value);
    }

    public void SetIsReadyWoosh(bool isready) => IsReadyWoosh = isready; // Setter untuk status kesiapan "woosh"
    public bool GetReadyWoosh() => IsReadyWoosh; // Getter untuk status kesiapan "woosh"
}
