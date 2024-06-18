using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MenuManager : MonoBehaviour
{
    [Header("SHOW BUTTON PROPERTY")]
    [SerializeField] GameObject _menu;
    [SerializeField] InputActionProperty _show_button;

    [Header("BODY COMPONENT")]
    [SerializeField] Transform _head;

    float spawnDistance = 2;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (_show_button.action.WasPressedThisFrame())
        {
            Debug.Log("DITEKAN");
            _menu.SetActive(!_menu.activeSelf);
            _menu.transform.position = _head.position + new Vector3(_head.forward.x, 0, _head.forward.z).normalized * spawnDistance;
        }

        _menu.transform.LookAt(new Vector3(_head.position.x, _menu.transform.position.y, _head.position.z));
        _menu.transform.forward *= -1;
    }
    public void DisableMenu()
    {
        _menu.SetActive(false);
    }
}
