using UnityEngine;

public class VolumePengendali : MonoBehaviour
{
    void Start()
    {
        // Ambil komponen Audio Source yang menempel di objek ini
        AudioSource audioSaya = GetComponent<AudioSource>();

        if (audioSaya != null)
        {
            // Cek apakah ada memori bernama "VolumeGame". Jika ada, ambil angkanya. 
            // Jika belum ada (misal langsung play dari SampleScene), set volumenya ke standar penuh (1f).
            float volumeTersimpan = PlayerPrefs.GetFloat("VolumeGame", 1f);

            // Terapkan suara sesuai memori slider tadi!
            audioSaya.volume = volumeTersimpan;
        }
    }
}