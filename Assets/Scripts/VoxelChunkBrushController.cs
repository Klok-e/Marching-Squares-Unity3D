using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(VoxelChunkController))]
    public class VoxelChunkBrushController : MonoBehaviour
    {
        private VoxelChunkController _chunk;

        [SerializeField]
        private Camera _cam;
        private const int _radius = 2;

        private int _mode;

        private float _count = 0f;

        private void Start()
        {
            _chunk = GetComponent<VoxelChunkController>();

        }

        private void Update()
        {
            _count += Time.deltaTime;
            if(_count > 0.01f)
            {
                _count = 0f;

                if(Input.GetMouseButtonDown(2))
                {
                    _mode++;
                    if(_mode > 1)
                        _mode = 0;
                }

                switch(_mode)
                {
                    case 0:
                        if(Input.GetMouseButton(0))
                        {
                            var worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);

                            int indX = Mathf.RoundToInt(worldPos.x);
                            int indY = Mathf.RoundToInt(worldPos.y);

                            Debug.Log($"Click! At {worldPos}, ind ({indX}, {indY})");

                            _chunk.AddSphere(new Vector2Int(indX, indY), _radius, -0.2f);
                        }
                        if(Input.GetMouseButton(1))
                        {
                            var worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);

                            int indX = Mathf.RoundToInt(worldPos.x);
                            int indY = Mathf.RoundToInt(worldPos.y);

                            Debug.Log($"Click! At {worldPos}, ind ({indX}, {indY})");

                            _chunk.AddSphere(new Vector2Int(indX, indY), _radius, 0.2f);
                        }
                        break;
                    case 1:
                        if(Input.GetMouseButton(0))
                        {
                            var worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);

                            int indX = Mathf.RoundToInt(worldPos.x);
                            int indY = Mathf.RoundToInt(worldPos.y);

                            Debug.Log($"Click! At {worldPos}, ind ({indX}, {indY})");

                            _chunk.SetSphere(new Vector2Int(indX, indY), _radius, -1f);
                        }
                        if(Input.GetMouseButton(1))
                        {
                            var worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);

                            int indX = Mathf.RoundToInt(worldPos.x);
                            int indY = Mathf.RoundToInt(worldPos.y);

                            Debug.Log($"Click! At {worldPos}, ind ({indX}, {indY})");

                            _chunk.SetSphere(new Vector2Int(indX, indY), _radius, 1f);
                        }
                        break;
                }
            }
        }
    }
}
