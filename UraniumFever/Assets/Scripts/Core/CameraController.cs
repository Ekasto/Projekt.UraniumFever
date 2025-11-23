using UnityEngine;

namespace UraniumFever.Core
{
    /// <summary>
    /// Controls camera movement for viewing the game board.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Pan Settings")]
        [SerializeField] private float panSpeed = 10f;
        [SerializeField] private Vector2 panLimitX = new Vector2(-10f, 10f);
        [SerializeField] private Vector2 panLimitZ = new Vector2(-10f, 10f);

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 20f;

        [Header("Rotate Settings")]
        [SerializeField] private float rotateSpeed = 100f;

        private Camera _camera;
        private Vector3 _lastMousePosition;
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

            // WASD or Arrow Keys
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                movement += transform.forward;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                movement -= transform.forward;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                movement -= transform.right;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                movement += transform.right;

            // Middle mouse button drag
            if (Input.GetMouseButton(2))
            {
                Vector3 delta = _lastMousePosition - Input.mousePosition;
                Vector3 direction = transform.right * delta.x + transform.forward * delta.y;
                movement += direction * 0.01f;
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

            _lastMousePosition = Input.mousePosition;
        }

        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                _currentZoom -= scroll * zoomSpeed;
                _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);

                Vector3 newPosition = transform.position;
                newPosition.y = _currentZoom;
                transform.position = newPosition;
            }
        }

        private void HandleRotate()
        {
            // Q and E keys to rotate
            float rotation = 0f;

            if (Input.GetKey(KeyCode.Q))
                rotation = 1f;
            if (Input.GetKey(KeyCode.E))
                rotation = -1f;

            // Right mouse button drag to rotate
            if (Input.GetMouseButton(1))
            {
                float delta = Input.GetAxis("Mouse X");
                rotation = -delta;
            }

            if (rotation != 0f)
            {
                transform.Rotate(Vector3.up, rotation * rotateSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
