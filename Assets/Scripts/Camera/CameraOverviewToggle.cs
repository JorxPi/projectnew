using System.Collections;
using UnityEngine;

public class CameraOverviewToggle : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private LevelBound2D bounds;                  // min/max asset
    [SerializeField] private CameraController2D pannerToDisable;   // your WASD script
    [SerializeField] private GameObject minimapRoot;               // assign the MinimapContainer or MinimapSlot GO

    [Header("Behaviour")]
    [SerializeField] private KeyCode toggleKey = KeyCode.T;
    [SerializeField] private float transitionTime = 0.35f; // 0 = instant

    private bool overviewOn = false;
    private bool isTransitioning = false;

    // Cached gameplay camera state
    private Vector3 origPos;
    private float origSize;

    private Coroutine anim;

    void Awake()
    {
        if (!mainCam) mainCam = Camera.main;
        origPos = mainCam.transform.position;
        origSize = mainCam.orthographicSize;
    }

    void Update()
    {
        // Do not accept toggle while an animation is running
        if (isTransitioning) return;

        if (Input.GetKeyDown(toggleKey))
        {
            overviewOn = !overviewOn;
            if (overviewOn) EnterOverview();
            else ExitOverview();
        }
    }

    private void EnterOverview()
    {
        if (!mainCam || bounds == null) return;

        // Cache current normal state
        origPos = mainCam.transform.position;
        origSize = mainCam.orthographicSize;

        // Disable panner immediately so it doesn't fight the transition
        if (pannerToDisable) pannerToDisable.enabled = false;

        // Hide minimap in overview
        if (minimapRoot) minimapRoot.SetActive(false);

        // Compute overview framing
        float levelW = Mathf.Max(0.01f, bounds.maxX - bounds.minX);
        float levelH = Mathf.Max(0.01f, bounds.maxY - bounds.minY);

        Vector3 targetPos = new Vector3(
            (bounds.minX + bounds.maxX) * 0.5f,
            (bounds.minY + bounds.maxY) * 0.5f,
            mainCam.transform.position.z
        );

        float fitByHeight = levelH * 0.5f;
        float fitByWidth = (levelW * 0.5f) / mainCam.aspect;
        float targetSize = Mathf.Max(fitByHeight, fitByWidth);

        StartAnim(targetPos, targetSize, reenablePanner: false);
    }

    private void ExitOverview()
    {
        if (!mainCam) return;

        // Animate back to cached state; re-enable panner & minimap after landing
        StartAnim(origPos, origSize, reenablePanner: true);
    }

    private void StartAnim(Vector3 toPos, float toSize, bool reenablePanner)
    {
        if (anim != null) StopCoroutine(anim);
        anim = StartCoroutine(Animate(toPos, toSize, transitionTime, reenablePanner));
    }

    private IEnumerator Animate(Vector3 toPos, float toSize, float time, bool reenablePanner)
    {
        isTransitioning = true;

        if (time <= 0f)
        {
            mainCam.transform.position = toPos;
            mainCam.orthographicSize = toSize;
        }
        else
        {
            Vector3 fromPos = mainCam.transform.position;
            float fromSz = mainCam.orthographicSize;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / time; // ignore timescale
                float s = Mathf.SmoothStep(0f, 1f, t);
                mainCam.transform.position = Vector3.Lerp(fromPos, toPos, s);
                mainCam.orthographicSize = Mathf.Lerp(fromSz, toSize, s);
                yield return null;
            }

            mainCam.transform.position = toPos;
            mainCam.orthographicSize = toSize;
        }

        if (reenablePanner && pannerToDisable)
        {
            pannerToDisable.enabled = true;
            pannerToDisable.SnapToCurrent(mainCam.transform.position); // hard sync
        }

        // Re-show minimap only when we’re back to normal gameplay view
        if (!overviewOn && minimapRoot)
            minimapRoot.SetActive(true);

        isTransitioning = false;
        anim = null;
    }
}
