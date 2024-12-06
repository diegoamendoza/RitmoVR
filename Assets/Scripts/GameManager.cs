using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI puntaje;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip clip;
    int puntos = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        AddPoints(0);
    }

    public void AddPoints(int points)
    {
        puntos += points;
        puntaje.text = puntos.ToString();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(clip);
    }

}
