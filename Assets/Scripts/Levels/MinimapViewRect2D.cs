using UnityEngine;

public class MinimapViewRect2D : MonoBehaviour
{
    [SerializeField] private Camera mainCam;                 // Main Camera
    [SerializeField] private Camera minimapCam;              // MinimapCamera
    [SerializeField] private LevelBound2D bounds;     // same asset
    [SerializeField] private RectTransform minimapRawImage;  // RectTransform of the RawImage
    [SerializeField] private RectTransform viewRect;         // RectTransform of the red Image

    void LateUpdate()
    {
        if (!mainCam || !minimapCam || !bounds || !minimapRawImage || !viewRect) return;

        float bxMin = bounds.minX, bxMax = bounds.maxX;
        float byMin = bounds.minY, byMax = bounds.maxY;

        float levelW = Mathf.Max(0.0001f, bxMax - bxMin);
        float levelH = Mathf.Max(0.0001f, byMax - byMin);

        // Main camera extents
        float halfH = mainCam.orthographicSize;
        float halfW = halfH * mainCam.aspect;

        float camX = mainCam.transform.position.x;
        float camY = mainCam.transform.position.y;

        float viewLeft = camX - halfW;
        float viewRight = camX + halfW;
        float viewBottom = camY - halfH;
        float viewTop = camY + halfH;

        // Normalized in level space
        float nCenterX = (camX - bxMin) / levelW;
        float nCenterY = (camY - byMin) / levelH;
        float nWidth = (viewRight - viewLeft) / levelW;
        float nHeight = (viewTop - viewBottom) / levelH;

        // Map to RawImage pixels
        Rect r = minimapRawImage.rect;
        float mapW = r.width;
        float mapH = r.height;

        viewRect.sizeDelta = new Vector2(nWidth * mapW, nHeight * mapH);
        viewRect.anchoredPosition = new Vector2((nCenterX - 0.5f) * mapW, (nCenterY - 0.5f) * mapH);
    }
}
