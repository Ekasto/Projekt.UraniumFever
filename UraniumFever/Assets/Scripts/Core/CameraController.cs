using UnityEngine;
using UnityEngine.InputSystem;

namespace UraniumFever.Core
{
    /// <summary>
    /// Controls camera movement for viewing the game board using new Input System.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Pan Settings")]
        [SerializeField] private float panSpeed = 10f;
        [SerializeField] private Vector2 panLimitX = new Vector2(-10f, 10f);
        [SerializeField] private Vector2 panLimitZ = new Vector2(-10f, 10f);

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 20f;

        [Header("Rotate Settings")]
        [SerializeField] private float rotateSpeed = 100f;

        private Camera _camera;
        private float _currentZoom = 10f;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
            {
                _camera = Camera.main;
            }

            _currentZoom = transform.position.y;
        }

        private void Update()
        {
            HandlePan();
            HandleZoom();
            HandleRotate();
        }

        private void HandlePan()
        {
            Vector3 movement = Vector3.zero;

            // WASD keyboard input
            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed)
                    movement += transform.forward;
                if (Keyboard.current.sKey.isPressed)
                    movement -= transform.forward;
                if (Keyboard.current.aKey.isPressed)
                    movement -= transform.right;
                if (Keyboard.current.dKey.isPressed)
                    movement += transform.right;
            }

            if (movement != Vector3.zero)
            {
                movement.y = 0;
                Vector3 newPosition = transform.position + movement * panSpeed * Time.deltaTime;

                // Apply limits
                newPosition.x = Mathf.Clamp(newPosition.x, panLimitX.x, panLimitX.y);
                newPosition.z = Mathf.Clamp(newPosition.z, panLimitZ.x, panLimitZ.y);

                transform.position = newPosition;
            }
        }

        private void HandleZoom()
        {
            if (Mouse.current != null)
            {
                float scroll = Mouse.current.scroll.ReadValue().y;

                if (scroll != 0)
                {
                    _currentZoom -= scroll * zoomSpeed * Time.deltaTime;
                    _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);

                    Vector3 newPosition = transform.position;
                    newPosition.y = _currentZoom;
                    transform.position = newPosition;
                }
            }
        }

        private void HandleRotate()
        {
            float rotation = 0f;

            // Q and E keys to rotate
            if (Keyboard.current != null)
            {
                if (Keyboard.current.qKey.isPressed)
                    rotation = 1f;
                if (Keyboard.current.eKey.isPressed)
                    rotation = -1f;
            }

            if (rotation != 0f)
            {
                transform.Rotate(Vector3.up, rotation * rotateSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
