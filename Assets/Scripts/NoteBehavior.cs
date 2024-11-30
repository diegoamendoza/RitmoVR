using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public NoteData NoteData { get; private set; }

    // Velocidad de movimiento de las notas
    public float moveSpeed = 5f;

    // Dirección en la que se moverán las notas
    private Vector3 moveDirection = Vector3.forward;

    private void Update()
    {
        MoveNote();
    }

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

    private void MoveNote()
    {
        // Mueve la nota continuamente hacia adelante
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
