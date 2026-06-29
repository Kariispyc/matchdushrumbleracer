using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Pergerakan Maju Otomatis")]
    private float speed = 15.0f; // Kecepatan maju mobil (bisa kamu sesuaikan nanti)

    [Header("Pergerakan Samping (Pindah Jalur)")]
    public float kecepatanPindah = 15f;   // Seberapa cepat mobil geser ke samping
    private int jalurSekarang = 1;        // 0 = Kiri, 1 = Tengah, 2 = Kanan
    private float jarakAntarJalur = 6.0f; // Jarak koordinat X (sesuai posisi -3, 0, 3)

    void Start()
    {
        // Pastikan waktu berjalan normal (1f) setiap kali level diulang / dimulai
        Time.timeScale = 1f;

        // Game dimulai, pastikan posisi mobil pas di tengah (X = 0)
        Vector3 posisiAwal = transform.position;
        posisiAwal.x = 0;
        transform.position = posisiAwal;
    }
    void Update()
    {
        // 1. MOBIL OTOMATIS JALAN MAJU (Tidak perlu pencet tombol atas/W lagi)
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        // 2. INPUT TOMBOL KE KIRI (A atau Panah Kiri)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (jalurSekarang > 0) // Jika tidak di jalur paling kiri (0)
            {
                jalurSekarang--; // Pindah indeks ke kiri
            }
        }

        // 3. INPUT TOMBOL KE KANAN (D atau Panah Kanan)
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (jalurSekarang < 2) // Jika tidak di jalur paling kanan (2)
            {
                jalurSekarang++; // Pindah indeks ke kanan
            }
        }

        // 4. MENGHITUNG TARGET KOORDINAT X
        // Jika jalur 0 -> (0-1) * 3 = -3 (Kiri)
        // Jika jalur 1 -> (1-1) * 3 = 0  (Tengah)
        // Jika jalur 2 -> (2-1) * 3 = 3  (Kanan)
        float posisiXTarget = (jalurSekarang - 1) * jarakAntarJalur;

        // 5. GERAKKAN MOBIL KE KANAN/KIRI SECARA MULUS (TIDAK TELEPORT)
        Vector3 posisiBaru = transform.position;
        posisiBaru.x = Mathf.MoveTowards(posisiBaru.x, posisiXTarget, kecepatanPindah * Time.deltaTime);
        transform.position = posisiBaru;
    }

    // Fungsi khusus Unity untuk mendeteksi tabrakan benda Is Trigger


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AnswerBox"))
        {
            AnswerBox kotak = other.GetComponent<AnswerBox>();

            if (kotak != null)
            {
                // Laporkan ke GameManager
                GameManager.instance.CekJawabanMobil(kotak.apakahBenar);
            }

            // HAPUS ATAU HILANGKAN BARIS DESTROY DI BAWAH INI:
            // Destroy(other.gameObject); 
        }
    }

    public void MatikanMobil()
    {
        speed = 0f; // Mengubah kecepatan maju mobil jadi nol agar berhenti berjalan
        kecepatanPindah = 0f; // Menyetop kontrol belok kanan-kiri
    }
}