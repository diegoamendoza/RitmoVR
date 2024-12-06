using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider; // El Slider UI.
    [SerializeField] private AudioMixer audioMixer; // El AudioMixer a controlar.
    private const string VolumeParameter = "Volume"; // Nombre del parámetro expuesto.

    private void Start()
    {
        // Configurar el slider para reflejar el valor inicial del volumen.
        if (volumeSlider != null)
        {
            float currentVolume;
            audioMixer.GetFloat(VolumeParameter, out currentVolume);
            volumeSlider.value = Mathf.InverseLerp(-80, 0, currentVolume); // Convertir de dB a rango [0,1].
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float value)
    {
        // Convertir el valor del slider (0-1) a dB (-80 a 0) y aplicarlo al AudioMixer.
        float dB = Mathf.Lerp(-80, 0, value);
        audioMixer.SetFloat(VolumeParameter, dB);
    }
}
