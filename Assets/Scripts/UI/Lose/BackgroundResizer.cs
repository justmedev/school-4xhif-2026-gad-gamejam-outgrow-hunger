using UnityEngine;

namespace UI.Lose
{
    public class BackgroundResizer : MonoBehaviour
    {
        private const float ScaleMultiplier = 1.1f;

        private Camera _cam;
        private RectTransform _rectTransform;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _cam = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!_cam) return;

            if (_rectTransform)
            {
                float scale = Mathf.Max(
                    Screen.width / _rectTransform.rect.width,
                    Screen.height / _rectTransform.rect.height
                ) * ScaleMultiplier;

                _rectTransform.localScale = Vector3.one * scale;
            }
            else if (_spriteRenderer && _cam.orthographic)
            {
                float camHeight = _cam.orthographicSize * 2f;
                float camWidth = camHeight * _cam.aspect;

                Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;

                float scale = Mathf.Max(
                    camWidth / spriteSize.x,
                    camHeight / spriteSize.y
                ) * ScaleMultiplier;

                transform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }
}