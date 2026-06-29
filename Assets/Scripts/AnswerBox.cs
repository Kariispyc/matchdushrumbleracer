using UnityEngine;
using TMPro;

public class AnswerBox : MonoBehaviour
{
    public int nilaiJawaban;     
    public bool apakahBenar;     
    public TextMeshPro komponenTeks; 

    void Start()
    {
        UpdateTeksAngka();
    }

    public void UpdateTeksAngka()
    {
        if (komponenTeks == null)
        {
            komponenTeks = GetComponentInChildren<TextMeshPro>();
        }

        if (komponenTeks != null)
        {
            komponenTeks.text = nilaiJawaban.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name.ToLower().Contains("mobil"))
        {
            // Deteksi jika di panggung ada GameManagerSulit (Level Sulit)
            if (GameManagerSulit.instance != null)
            {
                GameManagerSulit.instance.CekJawabanMobil(apakahBenar);
                return;
            }

            // Jika tidak ada, deteksi GameManager biasa (Level Mudah)
            if (GameManager.instance != null)
            {
                GameManager.instance.CekJawabanMobil(apakahBenar);
            }
        }
    }
}