using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Hubungkan Prefab Jalan")]
    public GameObject prefabJalan;

    [Header("Pengaturan Jalan")]
    public Transform posisiMobil;        // Untuk memantau posisi mobil sudah sampai mana
    public float panjangJalan = 50f;     // Jarak panjang satu balok jalan
    public int jumlahJalanDiLayar = 5;   // Jumlah antrean jalan (Disarankan isi 5 di Inspector)

    private List<GameObject> daftarJalanAktif = new List<GameObject>();
    private float posisiZSpawnAwal = 0f; 

    void Start()
    {
        // Di awal game, langsung buat antrean jalan awal agar mobil tidak jatuh ke jurang
        for (int i = 0; i < jumlahJalanDiLayar; i++)
        {
            if (i == 0)
            {
                // Mencari objek jalan utama yang sudah ada di scene saat game baru klik Play
                GameObject jalanAwal = GameObject.Find("Ground_Road");
                if (jalanAwal != null)
                {
                    daftarJalanAktif.Add(jalanAwal);
                    posisiZSpawnAwal += panjangJalan;
                    continue;
                }
            }

            SpawnJalanBaru();
        }
    }

    void Update()
    {
        if (posisiMobil == null) return;

        // RUMUS BARU: Menghitung sisa aspal yang tersedia di depan mobil
        float sisaJalanDiDepan = posisiZSpawnAwal - posisiMobil.position.z;
        
        // Batas aman minimal (setara dengan panjang 2 balok jalan)
        float jarakAmanMinimal = panjangJalan * 2f;

        // Jika sisa aspal di depan mobil sudah kurang dari batas aman, langsung pasang jalan baru!
        if (sisaJalanDiDepan < jarakAmanMinimal)
        {
            SpawnJalanBaru();
            HapusJalanLama();
        }
    }

    // Fungsi untuk melahirkan balok jalan baru di ujung depan
    void SpawnJalanBaru()
    {
        if (prefabJalan == null) return;

        // Menduplikat prefab jalan dan posisinya otomatis memanjang ke depan mengikuti koordinat Z
        // Menggunakan prefabJalan.transform.rotation agar rotasi jalan baru lurus (tidak melintang)
        GameObject jalanBaru = Instantiate(prefabJalan, new Vector3(0, 0, posisiZSpawnAwal), prefabJalan.transform.rotation);
        
        // Masukkan objek jalan baru ke dalam daftar catatan aktif
        daftarJalanAktif.Add(jalanBaru);
        
        // Geser target posisi koordinat Z untuk jalan berikutnya ke depan
        posisiZSpawnAwal += panjangJalan;
    }

    // Fungsi untuk menghapus jalan di belakang mobil yang sudah tidak terlihat oleh kamera
    void HapusJalanLama()
    {
        if (daftarJalanAktif.Count > 0)
        {
            // Ambil data jalan yang posisinya paling belakang
            GameObject jalanLama = daftarJalanAktif[0];
            
            // Hapus dari daftar catatan script
            daftarJalanAktif.RemoveAt(0);
            
            // Hancurkan objeknya dari panggung game agar laptop tidak lag/berat
            Destroy(jalanLama);
        }
    }
}