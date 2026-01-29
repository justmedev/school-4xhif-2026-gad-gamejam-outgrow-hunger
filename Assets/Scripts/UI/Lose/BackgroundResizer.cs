using UnityEngine;

namespace UI.Lose
{
    public class BackgroundResizer : MonoBehaviour
    {
        public float scaleMultiplier = 1.1f;

        private Camera _cam;
        private RectTransform _rectTransform;
        private SpriteRenderer _spriteRenderer;

        void Awake()
        {
            _cam = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void LateUpdate()
        {
            if (!_cam) return;

            if (_rectTransform)
            {
                _rectTransform.sizeDelta = new Vector2(
                    Screen.width * scaleMultiplier,
                    Screen.height * scaleMultiplier
                );
            }
            else if (_spriteRenderer && _cam.orthographic)
            {
                float camHeight = _cam.orthographicSize * 2f;
                float camWidth = camHeight * _cam.aspect;

                Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;

                transform.localScale = new Vector3(
                    camWidth / spriteSize.x * scaleMultiplier,
                    camHeight / spriteSize.y * scaleMultiplier,
                    1f
                );
            }
        }
    }
}