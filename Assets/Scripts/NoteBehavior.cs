using UnityEngine;

public class NoteBehavior : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField]
    public NoteData NoteData { get; set; }

    // Velocidad de movimiento de las notas
    public float moveSpeed = 5f;

    // Dirección en la que se moverán las notas
    private Vector3 moveDirection = Vector3.forward;
    public GameManager gameManager;
    public ParticleSystem particleSystem;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }
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



    private void MoveNote()
    {
        // Mueve la nota continuamente hacia adelante
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (NoteData.color)
        {
            case 0:
                if (other.CompareTag("LRed") && NoteData.hand == 0)
                {
                    ParticleSystem particulas = Instantiate(particleSystem, transform.position, Quaternion.identity);
                    particulas.Play();
                    Destroy(particulas, 0.5f);
                    particleSystem.Play();
                    Debug.Log("Nice Lred");
                    gameManager.gameObject.GetComponent<ControllerReader>().VibrateController(true, 0.8f, .3f);
                    gameManager.PlaySound();
                    gameManager.AddPoints(100);
                  

                    Destroy(gameObject);
                }
                else if (other.CompareTag("RRed") && NoteData.hand == 1)
                {
                    ParticleSystem particulas = Instantiate(particleSystem, transform.position, Quaternion.identity);
                    particulas.Play();
                    Destroy(particulas, 0.5f);
                    particleSystem.Play();
                    Debug.Log("Nice RRed");
                    gameManager.gameObject.GetComponent<ControllerReader>().VibrateController(false, 0.8f, .3f);
                    gameManager.PlaySound();
                    gameManager.AddPoints(100);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Fail");
                    Debug.Log(other.tag);

                }
                break;
            case 1:
                if (other.CompareTag("LBlue") && NoteData.hand == 0)
                {
                    ParticleSystem particulas = Instantiate(particleSystem, transform.position, Quaternion.identity);
                    particulas.Play();
                    Destroy(particulas, 0.5f);
                    particleSystem.Play();
                    Debug.Log("Nice");
                    gameManager.gameObject.GetComponent<ControllerReader>().VibrateController(true, 0.8f, .3f);
                    gameManager.PlaySound();
                    gameManager.AddPoints(100);
                    Destroy(gameObject);
                }
                else if (other.CompareTag("RBlue") && NoteData.hand == 1)
                {
                    ParticleSystem particulas = Instantiate(particleSystem, transform.position, Quaternion.identity);
                    particulas.Play();
                    Destroy(particulas, 0.5f);
                    particleSystem.Play();
                    Debug.Log("Nice RBlue");
                    gameManager.gameObject.GetComponent<ControllerReader>().VibrateController(false, 0.8f, .3f);
                    gameManager.PlaySound();
                    gameManager.AddPoints(100);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Fail");
                    Debug.Log(other.tag);

                }
                break;
            case 2:
                if (other.CompareTag("LPurple"))
                {
                    ParticleSystem particulas = Instantiate(particleSystem, transform.position, Quaternion.identity);
                    particulas.Play();
                    Destroy(particulas, 0.5f);
                    particleSystem.Play();
                    Debug.Log("Nice purple");
                    gameManager.gameObject.GetComponent<ControllerReader>().VibrateController(true, 0.8f, .3f);
                    gameManager.PlaySound();
                    gameManager.AddPoints(200);
                    Destroy(gameObject);

                }
                else if (other.CompareTag("RPurple") && NoteData.hand == 1)
                {
                    ParticleSystem particulas = Instantiate(particleSystem, transform.position, Quaternion.identity);
                    particulas.Play();
                    Destroy(particulas, 0.5f);
                    particleSystem.Play();
                    Debug.Log("Nice RPurple");
                    gameManager.gameObject.GetComponent<ControllerReader>().VibrateController(false, 0.8f, .3f);
                    gameManager.PlaySound();
                    gameManager.AddPoints(200);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Fail");
                    Debug.Log(other.tag);
                }
                break;
        }
    }
}
