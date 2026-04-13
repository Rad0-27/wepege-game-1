using System.Collections.Generic;
using UnityEngine;

public class StrokeTracer : MonoBehaviour
{
    [Header("Control Points (Editor)")]
    public List<Transform> controlPoints; // titik utama (sedikit saja)

    [Header("Generated Curve")]
    public List<Vector3> curvePoints = new List<Vector3>(); // hasil curve halus

    [Header("Visual")]
    public GameObject strokeVisual;
    public GameObject arrow;

    [Header("Mask")]
    public Transform maskTransform;
    public Transform maskStart;
    public Transform maskEnd;

    [Header("Settings")]
    public float startRadius = 0.5f;
    public float followRadius = 0.7f;
    public int resolution = 20; // makin besar makin halus

    [Header("Mask System")]
    public GameObject maskObject;          // object SpriteMask
    public SpriteRenderer strokeRenderer;  // sprite garis

    [Header("Arrow")]
    float rotationOffset = 107f;

    private int currentIndex = 0;
    private bool isDragging = false;

    void Start()
    {
        GenerateCurve();

        if (strokeVisual != null)
            strokeVisual.SetActive(false);

        if (arrow != null)
            arrow.SetActive(false);

        if (maskTransform != null && maskStart != null)
            maskTransform.position = maskStart.position;

        if (maskObject != null)
            maskObject.SetActive(true);

        if (strokeRenderer != null)
            strokeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    void Update()
    {
        if (curvePoints.Count == 0) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // START DRAG
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector2.Distance(mousePos, curvePoints[0]) < startRadius)
            {
                isDragging = true;
                currentIndex = 0;

                if (strokeVisual != null)
                    strokeVisual.SetActive(true);

            }
        }

        // DRAGGING (REAL-TIME)
        if (isDragging && Input.GetMouseButton(0))
        {
            FollowCurve(mousePos);
        }

        // RELEASE → FAIL jika belum selesai
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            if (currentIndex < curvePoints.Count - 1)
            {
                ResetStroke();
            }

            isDragging = false;
        }
    }

    // ==============================
    // 🔥 FOLLOW CURVE REAL-TIME
    // ==============================
    void FollowCurve(Vector2 mousePos)
    {
        for (int i = currentIndex; i < curvePoints.Count; i++)
        {
            float dist = Vector2.Distance(mousePos, curvePoints[i]);

            if (dist < followRadius)
            {
                currentIndex = i;
                UpdateMaskProgress();
                UpdateMaskProgress();
                UpdateArrowPosition();
            }
        }

        if (currentIndex >= curvePoints.Count - 1)
        {
            CompleteStroke();
        }
    }

    void UpdateArrowPosition()
    {
        if (arrow == null || curvePoints.Count == 0) return;

        // posisi smooth
        arrow.transform.position = Vector3.Lerp(
            arrow.transform.position,
            curvePoints[currentIndex],
            Time.deltaTime * 15f
        );

        // rotasi mengikuti arah
        if (currentIndex < curvePoints.Count - 1)
        {
            Vector3 dir = curvePoints[currentIndex + 1] - curvePoints[currentIndex];
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float angle0 = rotationOffset + angle;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle0);
            Debug.Log(angle0);
        }
    }

    // ==============================
    // 🎯 UPDATE MASK PROGRESS
    // ==============================
    void UpdateMaskProgress()
    {
        if (maskTransform == null || maskStart == null || maskEnd == null) return;

        float progress = (float)currentIndex / (curvePoints.Count - 1);

        maskTransform.position = Vector3.Lerp(
            maskStart.position,
            maskEnd.position,
            progress
        );
    }

    // ==============================
    // ✅ COMPLETE
    // ==============================
    void CompleteStroke()
    {
        isDragging = false;

        // 🔥 matikan mask
        if (maskObject != null)
            maskObject.SetActive(false);

        // 🔥 tampilkan full stroke
        if (strokeRenderer != null)
            strokeRenderer.maskInteraction = SpriteMaskInteraction.None;

        // lanjut ke stroke berikutnya
        GetComponentInParent<LetterController>().NextStroke();
    }

    // ==============================
    // ❌ RESET
    // ==============================
    void ResetStroke()
    {
        currentIndex = 0;

        if (strokeVisual != null)
            strokeVisual.SetActive(false);

        if (arrow != null)
            arrow.SetActive(true);

        if (maskTransform != null && maskStart != null)
            maskTransform.position = maskStart.position;

        if (maskObject != null)
            maskObject.SetActive(true);

        if (strokeRenderer != null)
            strokeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    void OnEnable()
    {
        // saat stroke AKTIF
        if (maskObject != null)
            maskObject.SetActive(true);

        if (strokeRenderer != null)
            strokeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        if (arrow != null)
            arrow.SetActive(true);

        ResetProgressInstant();
    }

    void OnDisable()
    {
        // saat stroke NONAKTIF
        if (maskObject != null)
            maskObject.SetActive(false);

        if (arrow != null)
            arrow.SetActive(false);
    }

    void ResetProgressInstant()
    {
        currentIndex = 0;

        if (maskTransform != null && maskStart != null)
            maskTransform.position = maskStart.position;

        if (strokeVisual != null)
            strokeVisual.SetActive(false);
    }

    // ==============================
    // 🔥 GENERATE CURVE (CATMULL-ROM)
    // ==============================
    void GenerateCurve()
    {
        curvePoints.Clear();

        if (controlPoints.Count < 2) return;

        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? controlPoints[i].position : controlPoints[i - 1].position;
            Vector3 p1 = controlPoints[i].position;
            Vector3 p2 = controlPoints[i + 1].position;
            Vector3 p3 = (i + 2 < controlPoints.Count) ? controlPoints[i + 2].position : p2;

            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 point = CatmullRom(p0, p1, p2, p3, t);
                curvePoints.Add(point);
            }
        }

        // tambahkan titik terakhir
        curvePoints.Add(controlPoints[controlPoints.Count - 1].position);
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // control points
        if (controlPoints != null)
        {
            foreach (var p in controlPoints)
            {
                if (p != null)
                    Gizmos.DrawSphere(p.position, 0.1f);
            }
        }

        Gizmos.color = Color.green;

        // curve points
        if (curvePoints != null)
        {
            foreach (var p in curvePoints)
            {
                Gizmos.DrawSphere(p, 0.05f);
            }
        }
    }
}

