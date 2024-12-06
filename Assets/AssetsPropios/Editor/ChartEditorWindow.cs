//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.UI;

//public class ChartEditorWindow : EditorWindow
//{
//    private AudioClip audioClip;
//    private float currentTime = 0f;
//    private List<Note> notes = new List<Note>();
//    private Vector2 gridScrollPos;
//    private Vector2 notesScrollPos;


//    private float maxTime = 0f;
//    private bool isPlaying = false;

//    private AudioSource audioSource;

//    private List<Note> selectedNotes = new List<Note>();
//    private float sequenceDuration = 1f;

//    [MenuItem("Tools/Chart Editor")]
//    public static void ShowWindow()
//    {
//        GetWindow<ChartEditorWindow>("Chart Editor");
//    }

//    private void OnGUI()
//    {
//        DrawAudioClipField();
//        if (audioClip != null)
//        {
//            DrawPlaybackControls();
//            DrawTimeline();
//            DrawGrid();
//            DrawSelectedNotesControls();
//        }
//    }

//    private void DrawAudioClipField()
//    {
//        EditorGUILayout.LabelField("Audio Clip", EditorStyles.boldLabel);
//        audioClip = (AudioClip)EditorGUILayout.ObjectField(audioClip, typeof(AudioClip), false);

//        if (audioClip != null && audioSource == null)
//        {
//            CreateAudioSource();
//            maxTime = audioClip.length;
//        }
//    }

//    private void DrawTimeline()
//    {
//        EditorGUILayout.LabelField("Timeline", EditorStyles.boldLabel);
//        Rect timelineRect = GUILayoutUtility.GetRect(0, 40, GUILayout.ExpandWidth(true));

//        // Fondo de la timeline
//        EditorGUI.DrawRect(timelineRect, Color.gray);

//        if (audioClip != null)
//        {
//            // Líneas de notas en la timeline
//            foreach (var note in notes)
//            {
//                float noteX = timelineRect.x + (note.time / maxTime) * timelineRect.width;
//                EditorGUI.DrawRect(new Rect(noteX, timelineRect.y, 2, timelineRect.height), Color.green);

//                // Hacer clic en la línea para saltar al tiempo
//                if (GUI.Button(new Rect(noteX - 5, timelineRect.y, 10, timelineRect.height), GUIContent.none, GUIStyle.none))
//                {
//                    currentTime = note.time;
//                    SeekAudio(currentTime);
//                }
//            }

//            // Línea roja indicando el tiempo actual
//            float markerX = timelineRect.x + (currentTime / maxTime) * timelineRect.width;
//            EditorGUI.DrawRect(new Rect(markerX, timelineRect.y, 2, timelineRect.height), Color.red);

//            // Mostrar tiempo actual y duración total
//            string currentTimeText = FormatTime(currentTime);
//            string maxTimeText = FormatTime(maxTime);
//            EditorGUI.LabelField(new Rect(timelineRect.x, timelineRect.y - 20, timelineRect.width, 20), $"{currentTimeText} / {maxTimeText}", EditorStyles.centeredGreyMiniLabel);

//            // Slider para moverse en la timeline
//            float newTime = GUI.HorizontalSlider(
//                new Rect(timelineRect.x, timelineRect.y + 20, timelineRect.width, 10),
//                currentTime, 0, maxTime
//            );

//            if (!Mathf.Approximately(newTime, currentTime))
//            {
//                currentTime = newTime;
//                if (isPlaying) SeekAudio(currentTime);
//            }
//        }
//    }

//    private void DrawGrid()
//    {
//        EditorGUILayout.LabelField("Grid (16x9)", EditorStyles.boldLabel);
//        gridScrollPos = EditorGUILayout.BeginScrollView(gridScrollPos, GUILayout.Height(300));

//        for (int y = 0; y < 9; y++)
//        {
//            EditorGUILayout.BeginHorizontal();
//            for (int x = 0; x < 16; x++)
//            {
//                bool isActive = notes.Exists(n => n.x == x && n.y == y && Mathf.Approximately(n.time, currentTime));
//                Note activeNote = notes.Find(n => n.x == x && n.y == y && Mathf.Approximately(n.time, currentTime));
//                Color noteColor = activeNote != null ? GetNoteColor(activeNote.color) : Color.white;

//                Note newNote = new Note { x = x, y = y, time = Mathf.Floor(currentTime * 1000) / 1000 };

//                GUI.backgroundColor = isActive ? noteColor : Color.white;

//                if (GUILayout.Button("", GUILayout.Width(30), GUILayout.Height(30)))
//                {
//                    if (isActive)
//                    {
//                        GUI.backgroundColor = Color.white;
//                        notes.Remove(activeNote);
//                        selectedNotes.Remove(activeNote);
//                        isActive = false;
//                        activeNote = null;
//                        noteColor = Color.white;

//                    }
//                    else
//                    {
//                        notes.Add(newNote);
//                        selectedNotes.Add(newNote);
//                    }
//                }
//            }
//            EditorGUILayout.EndHorizontal();
//        }

//        GUI.backgroundColor = Color.white;
//        EditorGUILayout.EndScrollView();
//    }

//    private void DrawSelectedNotesControls()
//    {
//        EditorGUILayout.LabelField("Selected Notes", EditorStyles.boldLabel);
//        notesScrollPos = GUILayout.BeginScrollView(notesScrollPos, GUILayout.Width(400), GUILayout.Height(400));
//        if (selectedNotes.Count > 0)
//        {

//            sequenceDuration = EditorGUILayout.FloatField("Sequence Duration (s):", sequenceDuration);



//            if (GUILayout.Button("Distribute Notes"))
//            {
//                DistributeSelectedNotes();
//            }

//            if (GUILayout.Button("Delete Selected Notes"))
//            {
//                DeleteSelectedNotes();
//            }
//            foreach (var note in selectedNotes)
//            {
//                EditorGUILayout.LabelField("Note", EditorStyles.boldLabel);
//                note.order = EditorGUILayout.IntField("Order:", note.order);
//                note.hand = (NoteHand)EditorGUILayout.EnumPopup("Hand:", note.hand);
//                note.color = (NoteColor)EditorGUILayout.EnumPopup("Color:", note.color);
//            }

//        }
//        GUI.backgroundColor = Color.white;
//        EditorGUILayout.EndScrollView();
//    }

//    private void DistributeSelectedNotes()
//    {
//        if (selectedNotes.Count > 0 && sequenceDuration > 0)
//        {
//            // Ordenar las notas seleccionadas por el número de orden
//            selectedNotes.Sort((a, b) => a.order.CompareTo(b.order));

//            // Calcular la cantidad de marcas de tiempo únicas
//            int maxOrder = selectedNotes[^1].order; // El último elemento tiene el mayor número de orden
//            float interval = sequenceDuration / Mathf.Max(1, maxOrder); // Aseguramos al menos una división válida

//            // Asignar tiempos basados en el orden
//            foreach (var note in selectedNotes)
//            {
//                note.time = selectedNotes[0].time + (note.order * interval);
//            }

//            // Recalcular tiempos para órdenes duplicados
//            for (int i = 1; i < selectedNotes.Count; i++)
//            {
//                if (selectedNotes[i].order == selectedNotes[i - 1].order)
//                {
//                    selectedNotes[i].time = selectedNotes[i - 1].time;
//                }
//            }

//            Repaint();
//        }

//        selectedNotes.Clear();
//    }


//    private void DeleteSelectedNotes()
//    {
//        foreach (var note in selectedNotes)
//        {
//            notes.Remove(note);
//        }
//        selectedNotes.Clear();
//        Repaint();
//    }

//    private void DrawPlaybackControls()
//    {
//        EditorGUILayout.LabelField("Playback Controls", EditorStyles.boldLabel);
//        EditorGUILayout.BeginHorizontal();

//        if (GUILayout.Button("Play"))
//        {
//            PlayAudio();
//        }

//        if (GUILayout.Button("Pause"))
//        {
//            PauseAudio();
//        }

//        if (GUILayout.Button("Stop"))
//        {
//            StopAudio();
//        }

//        EditorGUILayout.EndHorizontal();

//        EditorGUILayout.BeginHorizontal();
//        if (GUILayout.Button("Save Chart"))
//        {
//            SaveChart();
//        }
//        if (GUILayout.Button("Load Chart"))
//        {
//            LoadChart();
//        }
//        EditorGUILayout.EndHorizontal();
//    }

//    private void CreateAudioSource()
//    {
//        GameObject audioSourceObject = new GameObject("ChartEditorAudioSource");
//        audioSourceObject.hideFlags = HideFlags.HideAndDontSave;
//        audioSource = audioSourceObject.AddComponent<AudioSource>();
//        audioSource.playOnAwake = false;
//    }

//    private void PlayAudio()
//    {
//        if (audioClip == null) return;

//        if (!audioSource.isPlaying)
//        {
//            audioSource.clip = audioClip;
//            audioSource.time = currentTime;
//            audioSource.Play();
//        }
//        isPlaying = true;
//        EditorApplication.update += UpdateAudioPlayback;
//    }

//    private void PauseAudio()
//    {
//        if (audioSource.isPlaying)
//        {
//            audioSource.Pause();
//        }
//        isPlaying = false;
//        EditorApplication.update -= UpdateAudioPlayback;
//    }

//    private void StopAudio()
//    {
//        if (audioSource.isPlaying)
//        {
//            audioSource.Stop();
//        }
//        isPlaying = false;
//        currentTime = 0;
//        audioSource.time = 0;
//        EditorApplication.update -= UpdateAudioPlayback;
//    }

//    private void SeekAudio(float time)
//    {
//        if (audioSource != null)
//        {
//            audioSource.time = time;
//        }
//    }

//    private void UpdateAudioPlayback()
//    {
//        if (isPlaying && audioClip != null)
//        {
//            currentTime = audioSource.time;
//            if (currentTime >= maxTime)
//            {
//                StopAudio();
//            }
//            Repaint();
//        }
//    }

//    private string FormatTime(float time)
//    {
//        int minutes = Mathf.FloorToInt(time / 60);
//        int seconds = Mathf.FloorToInt(time % 60);
//        int milliseconds = Mathf.FloorToInt((time % 1) * 1000);
//        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
//    }

//    private void SaveChart()
//    {
//        string path = EditorUtility.SaveFilePanel("Save Chart", "", "chart.json", "json");
//        if (string.IsNullOrEmpty(path)) return;

//        ChartData chartData = new ChartData
//        {
//            notes = notes
//        };
//        string json = JsonUtility.ToJson(chartData, true);
//        System.IO.File.WriteAllText(path, json);
//    }

//    private void LoadChart()
//    {
//        string path = EditorUtility.OpenFilePanel("Load Chart", "", "json");
//        if (string.IsNullOrEmpty(path)) return;

//        string json = System.IO.File.ReadAllText(path);
//        ChartData chartData = JsonUtility.FromJson<ChartData>(json);
//        notes = chartData.notes;
//        Repaint();
//    }

//    private Color GetNoteColor(NoteColor noteColor)
//    {
//        switch (noteColor)
//        {
//            case NoteColor.Red: return Color.red;
//            case NoteColor.Blue: return Color.blue;
//            case NoteColor.Purple: return Color.magenta;
//            case NoteColor.White: return Color.white;
//            default: return Color.white;
//        }
//    }
//}

//[System.Serializable]
//public class Note
//{
//    public int x;
//    public int y;
//    public float time;
//    public int order;
//    public NoteHand hand;
//    public NoteColor color;
//}

//[System.Serializable]
//public class ChartData
//{
//    public List<Note> notes;
//}

//public enum NoteHand
//{
//    Left,
//    Right
//}

//public enum NoteColor
//{
//    Red,
//    Blue,
//    Purple,
//    White
//}
