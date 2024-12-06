using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Spawneer : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip audioClip;
    private AudioSource audioSource;
    private float currentTime = 0f;
    private bool isPlaying = false;

    [Header("Spawn Settings")]
    public float delay = 0f;
    public string notesJsonPath = "Assets/NotesData.json"; // Ruta del archivo JSON
    public Transform spawnParent;

    [Header("Note Prefabs")]
    public GameObject redLeftPrefab;
    public GameObject redRightPrefab;
    public GameObject blueLeftPrefab;
    public GameObject blueRightPrefab;
    public GameObject purpleLeftPrefab;
    public GameObject purpleRightPrefab;

    private List<NoteData> notes;
    private List<GameObject> spawnedNotes = new List<GameObject>(); // Para rastrear las notas instanciadas

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;

        LoadNotesFromJson(); // Cargar las notas al inicio


        Play();
    }

    private void Update()
    {
        if (isPlaying)
        {
            currentTime = audioSource.time;

            foreach (var note in notes)
            {
                if (!note.isSpawned && currentTime >= note.time - delay)
                {
                    SpawnNote(note);
                    note.isSpawned = true;
                }
            }

            // Conexiones de las notas
            UpdateNoteConnections();
        }
    }

    public void Play()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
            isPlaying = true;
        }
    }

    public void Pause()
    {
        audioSource.Pause();
        isPlaying = false;
    }

    public void Stop()
    {
        audioSource.Stop();
        currentTime = 0f;
        isPlaying = false;
    }

    private void SpawnNote(NoteData note)
    {
        GameObject prefab = GetPrefab((NoteColor)note.color, (NoteHand)note.hand);
        if (prefab == null) return;

        Vector3 position = new Vector3((note.x - 8) * .2f, (((9-1) -note.y) -2) *.2f, -25);
        GameObject spawnedNote = Instantiate(prefab, position + spawnParent.position, Quaternion.identity, spawnParent);
        spawnedNotes.Add(spawnedNote);

        var noteBehavior = spawnedNote.GetComponent<NoteBehavior>();
        if (noteBehavior != null)
        {
            noteBehavior.Initialize(note); // Inicializar con los datos de la nota
        }
    }

    private GameObject GetPrefab(NoteColor color, NoteHand hand)
    {
        return (color, hand) switch
        {
            (NoteColor.Red, NoteHand.Left) => redLeftPrefab,
            (NoteColor.Red, NoteHand.Right) => redRightPrefab,
            (NoteColor.Blue, NoteHand.Left) => blueLeftPrefab,
            (NoteColor.Blue, NoteHand.Right) => blueRightPrefab,
            (NoteColor.Purple, NoteHand.Left) => purpleLeftPrefab,
            (NoteColor.Purple, NoteHand.Right) => purpleRightPrefab,
            _ => null,
        };
    }

    private void LoadNotesFromJson()
    {
        if (!File.Exists(notesJsonPath))
        {
            Debug.LogError($"JSON file not found at path: {notesJsonPath}");
            notes = new List<NoteData>();
            return;
        }

        string jsonContent = File.ReadAllText(notesJsonPath);
        NotesContainer container = JsonUtility.FromJson<NotesContainer>(jsonContent);
        notes = container.notes;
    }

    private void UpdateNoteConnections()
    {
        for (int i = 0; i < spawnedNotes.Count - 1; i++)
        {
            NoteBehavior current = spawnedNotes[i].GetComponent<NoteBehavior>();
            NoteBehavior next = spawnedNotes[i + 1].GetComponent<NoteBehavior>();

            if (current != null && next != null)
            {
                // Verificar si las notas están conectadas y si tienen la misma mano
                if (next.NoteData.order == current.NoteData.order + 1 &&
                    next.NoteData.hand == current.NoteData.hand)
                {
                    // Conectar la nota actual con la siguiente
                    current.ConnectTo(next.transform.position);
                }
            }
        }
    }

}

[System.Serializable]
public class NoteData
{
    public float x;
    public float y;
    public float time;
    public int order;
    public int hand;
    public int color;
    [HideInInspector] public bool isSpawned = false;
}

[System.Serializable]
public class NotesContainer
{
    public List<NoteData> notes;
}

public enum NoteHand
{
    Left = 0,
    Right = 1
}

public enum NoteColor
{
    Red = 0,
    Blue = 1,
    Purple = 2
}
