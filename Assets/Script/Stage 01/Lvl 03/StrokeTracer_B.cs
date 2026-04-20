using System.Collections.Generic;
using UnityEngine;

public class StrokeTracer_B : StrokeTracer
{
    [Header("Mask A (Curve)")]
    public Transform maskA;
    public Transform startA;
    public Transform midA;
    public Transform endA;

    [Header("Mask B (Curve)")]
    public Transform maskB;
    public Transform startB;
    public Transform midB;
    public Transform endB;

    [Range(0.1f, 0.9f)]
    public float switchPoint = 0.5f;

    bool hasStarted = false;


    // ==============================
    // 🔥 UPDATE MASK (CURVE)
    // ==============================
    protected override void UpdateMaskProgress()
    {
        if (curvePoints == null || curvePoints.Count == 0) return;

        float progress = (float)currentIndex / (curvePoints.Count - 1);

        // =========================
        // 🔥 FORCE START DI MASK A
        // =========================
        if (!hasStarted)
        {
            hasStarted = true;

            if (maskA != null) maskA.gameObject.SetActive(true);
            if (maskB != null) maskB.gameObject.SetActive(false);
        }

        // =========================
        // SWITCH LOGIC
        // =========================
        if (progress < switchPoint)
        {
            if (maskA != null) maskA.gameObject.SetActive(true);
            if (maskB != null) maskB.gameObject.SetActive(false);

            float t = progress / switchPoint;

            maskA.position = GetQuadraticBezierPoint(t, startA.position, midA.position, endA.position);
        }
        else
        {
            // 🔥 JANGAN MATIKAN MASK A
            if (maskA != null) maskA.gameObject.SetActive(true);
            if (maskB != null) maskB.gameObject.SetActive(true);

            float t = (progress - switchPoint) / (1f - switchPoint);

            maskB.position = GetQuadraticBezierPoint(t, startB.position, midB.position, endB.position);
        }
    }

    // ==============================
    // 🧠 BEZIER FUNCTION
    // ==============================
    Vector3 GetQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return Mathf.Pow(1 - t, 2) * p0 +
               2 * (1 - t) * t * p1 +
               Mathf.Pow(t, 2) * p2;
    }

    // ==============================
    // 🔄 RESET
    // ==============================
    protected override void ResetProgressInstant()
    {
        base.ResetProgressInstant();

        hasStarted = false;

        if (maskA != null)
        {
            maskA.gameObject.SetActive(true);
            if (startA != null)
                maskA.position = startA.position;
        }

        if (maskB != null)
        {
            maskB.gameObject.SetActive(false);
            if (startB != null)
                maskB.position = startB.position;
        }
    }

    protected override void CompleteStroke()
    {
        base.CompleteStroke();

        // matikan mask
        if (maskA != null) maskA.gameObject.SetActive(false);
        if (maskB != null) maskB.gameObject.SetActive(false);
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
        if (maskA != null)
            maskA.gameObject.SetActive(false);


        if (maskB != null)
            maskB.gameObject.SetActive(false);

        if (arrow != null)
            arrow.SetActive(false);
    }

    // ==============================
    // 🎯 GIZMO CURVE
    // ==============================
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

        DrawCurve(startA, midA, endA, new Color(1f, 0.4f, 0.7f)); // pink A
        DrawCurve(startB, midB, endB, new Color(1f, 0.7f, 0.85f)); // pink B

        // titik (violet)
        Gizmos.color = new Color(0.6f, 0.2f, 0.8f);

        DrawPoint(startA);
        DrawPoint(midA);
        DrawPoint(endA);

        DrawPoint(startB);
        DrawPoint(midB);
        DrawPoint(endB);
    }

    void DrawCurve(Transform p0, Transform p1, Transform p2, Color color)
    {
        if (p0 == null || p1 == null || p2 == null) return;

        Gizmos.color = color;

        Vector3 prev = p0.position;
        int resolution = 30;

        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;

            Vector3 point = GetQuadraticBezierPoint(t, p0.position, p1.position, p2.position);

            Gizmos.DrawLine(prev, point);
            prev = point;
        }
    }

    void DrawPoint(Transform t)
    {
        if (t == null) return;
        Gizmos.DrawSphere(t.position, 0.1f);
    }
}