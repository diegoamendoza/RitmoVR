using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class Spawneer : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip audioClip;
    private AudioSource audioSource;
    private float currentTime = 0f;
    private bool isPlaying = false;

    [Header("Spawn Settings")]
    public float delay = 0f;
    public string jsonName; 
    private string notesJsonPath = Path.Combine(Application.streamingAssetsPath, "archivo.json"); // Ruta del archivo JSON
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
        notesJsonPath = Path.Combine(Application.streamingAssetsPath, jsonName);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;

        LoadNotesFromJson(); // Cargar las notas al inicio


            
        StartCoroutine(WaitForDelay());
    }

    private void Update()
    {
        if (isPlaying)
        {
            
            currentTime += Time.deltaTime;

            foreach (var note in notes)
            {
                if (!note.isSpawned && currentTime >= note.time)
                {
                    SpawnNote(note);
                    note.isSpawned = true;
                }
            }

            // Conexiones de las notas
           
        }
    }

    public void Play()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    IEnumerator WaitForDelay()
    {
        isPlaying = true;
        yield return new WaitForSeconds(delay);
        Play();
        
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
