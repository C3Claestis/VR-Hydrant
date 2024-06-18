using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActionPlayerHandle : MonoBehaviour
{
    [SerializeField] InputAction runButton;
    [SerializeField] ActionBasedContinuousMoveProvider move;

    void OnEnable()
    {
        // Mengaktifkan aksi untuk mendengarkan input
        runButton.Enable();        
    }

    void OnDisable()
    {
        // Menonaktifkan aksi saat objek dimatikan
        runButton.Disable();        
    }
    private void Start()
    {
        runButton.performed += ctx => isRun = true;
        runButton.canceled += ctx => isRun = false;
    }

    private void Update()
    {
        move.moveSpeed = isRun ? 2f : 1f;
    }

    private bool isRun;
}
