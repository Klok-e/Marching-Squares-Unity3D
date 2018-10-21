using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        private void LateUpdate()
        {
            var vertical = Input.GetAxis("Vertical");
            var horizontal = Input.GetAxis("Horizontal");

            transform.Translate(new Vector3(horizontal, vertical, 0) * _speed);
        }
    }
}