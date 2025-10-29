using UnityEngine;

public class MinimapSetup2D : MonoBehaviour
{
    [Header("Scene refs")]
    [SerializeField] private Camera minimapCam;
    [SerializeField] private RenderTexture minimapRT;
    [SerializeField] private LevelBound2D bounds;

    [Header("RT sizing")]
    [SerializeField] private int baseHeight = 256; // change to 512 if you want more detail

    void Start()
    {
        if (!minimapCam || !minimapRT || !bounds) return;

        // 1) Compute level aspect
        float levelW = Mathf.Max(0.01f, bounds.maxX - bounds.minX);
        float levelH = Mathf.Max(0.01f, bounds.maxY - bounds.minY);
        float aspect = levelW / levelH;

        // 2) Resize RenderTexture to match level aspect
        int height = Mathf.Max(1, baseHeight);
        int width = Mathf.Max(1, Mathf.RoundToInt(height * aspect));

        if (minimapRT.width != width || minimapRT.height != height)
        {
            minimapRT.Release();
            minimapRT.width = width;
            minimapRT.height = height;
            minimapRT.Create();
        }

        // Ensure the camera actually uses this RT
        minimapCam.targetTexture = minimapRT;

        // 3) Position the minimap camera at level center, correct orientation
        Vector3 p = minimapCam.transform.position;
        p.x = (bounds.minX + bounds.maxX) * 0.5f;
        p.y = (bounds.minY + bounds.maxY) * 0.5f;
        p.z = -10f;
        minimapCam.transform.position = p;
        minimapCam.transform.rotation = Quaternion.identity;
        minimapCam.orthographic = true;

        // 4) Fit orthographic size so the whole level is visible
        float sizeByHeight = levelH * 0.5f;
        float sizeByWidth = (levelW * 0.5f) / minimapCam.aspect; // aspect now matches RT
        minimapCam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth);
    }
}
