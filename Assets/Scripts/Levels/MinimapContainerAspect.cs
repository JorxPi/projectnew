using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
public class MinimapContainerAspect : MonoBehaviour
{
    [SerializeField] private LevelBound2D bounds; // your min/max asset

    void Awake()
    {
        var f = GetComponent<AspectRatioFitter>();
        f.aspectMode = AspectRatioFitter.AspectMode.FitInParent;

        float w = Mathf.Max(0.01f, bounds.maxX - bounds.minX);
        float h = Mathf.Max(0.01f, bounds.maxY - bounds.minY);
        f.aspectRatio = w / h; // container (the border) now matches level aspect
    }
}