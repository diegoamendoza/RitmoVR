using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public NoteData NoteData { get; private set; }

    public void Initialize(NoteData noteData)
    {
        NoteData = noteData;
        Debug.Log(noteData.color);
        
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.enabled = false; // Desactivado por defecto
        }
    }

    public void ConnectTo(Vector3 targetPosition)
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetPosition);
        }
    }
}
