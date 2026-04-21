using System.Collections;
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
    public Transform maskMid;
    public Transform maskEnd;

    [Header("Settings")]
    public float startR = 0.1f;
    public float followR = 0.1f;
    public int resolution = 20; // makin besar makin halus

    [Header("Mask System")]
    public GameObject maskObject;          // object SpriteMask
    public SpriteRenderer strokeRenderer;  // sprite garis

    [Header("Arrow")]
    float rotationOffset = 107f;

    // ==============================
    // 🔥 ARROW SETTINGS
    // ==============================
    [Header("Arrow Hint")]
    [Header("Arrow Demo")]
    float idleDelay = 5f;
    float demoSpeed = 1f;
    int demoLoop = 1;
    float demoCooldown = 3f;

    float idleTimer = 0f;
    bool isDemoPlaying = false;
    bool isCooldown = false;

    Vector3 arrowBaseScale;

    public int currentIndex = 0;
    private bool isDragging = false;

    LevelManager levelManager;

    void Start()
    {
        GenerateCurve();

        levelManager = FindObjectOfType<LevelManager>();

        if (strokeVisual != null)
            strokeVisual.SetActive(false);

        if (arrow != null)
            arrow.SetActive(false);
            arrowBaseScale = arrow.transform.localScale;

        if (maskTransform != null && maskStart != null)
            maskTransform.position = maskStart.position;

        if (maskObject != null)
            maskObject.SetActive(true);

        if (strokeRenderer != null)
            strokeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        UpdateArrowPosition();
        ResetProgressInstant();
    }

    void Update()
    {
        if (curvePoints.Count == 0) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // START DRAG
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector2.Distance(mousePos, curvePoints[0]) < startR)
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
            TManager.instance.PlayerDidAction();
        }

        // RELEASE → FAIL jika belum selesai
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            if (currentIndex < curvePoints.Count - 1)
            {
                ResetStroke();
            }
            if (arrow != null && controlPoints != null && controlPoints.Count > 0)
            {
                arrow.transform.position = controlPoints[0].position;
                
            }
            isDragging = false;
        }

        // 🔥 kalau huruf ini bukan yang aktif → sembunyikan arrow
        if (transform.parent.GetSiblingIndex() != levelManager.currentLetterIndex)
        {
            if (arrow != null) arrow.SetActive(false);
            return;
        }

        // huruf aktif → arrow boleh tampil
        if (arrow != null && !arrow.activeSelf)
            arrow.SetActive(true); 

        
        HandleIdleDemo();
        GetComponentInParent<LetterController>().ClearArrow();
    }

    // ==============================
    // 🔥 FOLLOW CURVE REAL-TIME
    // ==============================
    void FollowCurve(Vector2 mousePos)
    {
        for (int i = currentIndex; i < curvePoints.Count; i++)
        {
            float dist = Vector2.Distance(mousePos, curvePoints[i]);

            if (dist < followR)
            {
                currentIndex = i;
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
        }
    }

    void HandleIdleDemo()
    {
        if (arrow == null || isDemoPlaying || isCooldown) return;

        if (Input.GetMouseButton(0))
        {
            idleTimer = 0f;
            return;
        }

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDelay)
        {
            StartCoroutine(PlayDemo());
        }
    }

    IEnumerator PlayDemo()
    {
        isDemoPlaying = true;
        idleTimer = 0f;

        if (controlPoints == null || controlPoints.Count == 0)
        {
            isDemoPlaying = false;
            yield break;
        }

        for (int loop = 0; loop < demoLoop; loop++)
        {
            for (int i = 0; i < curvePoints.Count; i++)
            {
                if (Input.GetMouseButton(0))
                {
                    isDemoPlaying = false;
                    yield break;
                }


                arrow.transform.position = curvePoints[i];

                if (i < curvePoints.Count - 1)
                {
                    Vector3 dir = curvePoints[i + 1] - curvePoints[i];
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    float finalAngle = rotationOffset + angle;

                    arrow.transform.rotation = Quaternion.Euler(0, 0, finalAngle);
                }

                yield return new WaitForSeconds(1f / (demoSpeed * 60f));
            }
        }

        // 🔥 BALIK KE START
        arrow.transform.position = controlPoints[0].position;

        isDemoPlaying = false;

        UpdateArrowPosition();

        // 🔥 COOLDOWN sebelum demo lagi
        StartCoroutine(DemoCooldown());
    }

    IEnumerator DemoCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(demoCooldown);
        idleTimer = 0f;
        isCooldown = false;
    }

    // ==============================
    // 🎯 UPDATE MASK PROGRESS
    // ==============================
    protected virtual void UpdateMaskProgress()
    {
        if (maskTransform == null || maskStart == null || maskEnd == null || maskMid == null) return;

        float t = (float)currentIndex / (curvePoints.Count - 1);

        // 🔥 Quadratic Bezier
        Vector3 pos =
            Mathf.Pow(1 - t, 2) * maskStart.position +
            2 * (1 - t) * t * maskMid.position +
            Mathf.Pow(t, 2) * maskEnd.position;

        maskTransform.position = pos;
    }

    // ==============================
    // ✅ COMPLETE
    // ==============================
    protected virtual void CompleteStroke()
    {
        isDragging = false;

        // 🔥 matikan mask
        if (maskObject != null)
            maskObject.SetActive(false);

        // 🔥 tampilkan full stroke
        if (strokeRenderer != null)
            strokeRenderer.maskInteraction = SpriteMaskInteraction.None;

        if (arrow != null)
            arrow.SetActive(false);

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


        if (maskTransform != null && maskStart != null)
            maskTransform.position = maskStart.position;

        if (maskObject != null)
            maskObject.SetActive(true);

        if (strokeRenderer != null)
            strokeRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        UpdateArrowPosition();
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

    protected virtual void ResetProgressInstant()
    {
        currentIndex = 0;

        if (maskTransform != null && maskStart != null)
            maskTransform.position = maskStart.position;

        if (strokeVisual != null)
            strokeVisual.SetActive(false);

        if (arrow != null && controlPoints != null)
        {
            arrow.transform.position = controlPoints[0].position;
            
        }
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
        if (maskStart == null || maskMid == null || maskEnd == null) return;

        // =========================
        // 🔮 TITIK (VIOLET)
        // =========================
        Color violet = new Color(0.6f, 0.2f, 0.8f);

        Gizmos.color = violet;
        Gizmos.DrawSphere(maskStart.position, 0.12f);
        Gizmos.DrawSphere(maskMid.position, 0.12f);
        Gizmos.DrawSphere(maskEnd.position, 0.12f);

        // garis bantu (optional)
        Gizmos.DrawLine(maskStart.position, maskMid.position);
        Gizmos.DrawLine(maskMid.position, maskEnd.position);

        // =========================
        // 🌸 CURVE (PINK)
        // =========================
        Color pink = new Color(1f, 0.4f, 0.7f);
        Gizmos.color = pink;

        Vector3 prevPoint = maskStart.position;

        int resolution = 30;

        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;

            Vector3 point =
                Mathf.Pow(1 - t, 2) * maskStart.position +
                2 * (1 - t) * t * maskMid.position +
                Mathf.Pow(t, 2) * maskEnd.position;

            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}

