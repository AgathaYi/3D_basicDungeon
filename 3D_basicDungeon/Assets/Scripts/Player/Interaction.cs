using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    public float maxDistance;
    public LayerMask layerMask;
    public TextMeshProUGUI promptText;

    private float lastCheckTime;
    private GameObject curInteractGo;
    private IInteractable curInteractable;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        ScanCenterPoint();
    }

    private void ScanCenterPoint()
    {
        if (Time.time - lastCheckTime <= checkRate) return;
        lastCheckTime = Time.time;

        // 화면 중앙점부터 Ray쏘기
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        // 충돌 된 물체 확인
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
        {
            var interact = hit.collider.GetComponent<IInteractable>();
            if (interact != null && hit.collider.gameObject != curInteractGo)
            {
                curInteractGo = hit.collider.gameObject;
                curInteractable = interact;
                promptText.text = interact.GetInteractPrompt();
                promptText.gameObject.SetActive(true);
            }
        }
        else
        {
            curInteractGo = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    // InputSystem: 상호작용 버튼 이벤트
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGo = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
