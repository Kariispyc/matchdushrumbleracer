using UnityEngine;
using UnityEngine.UI; // Penting untuk mengontrol Slider dan Button
using UnityEngine.SceneManagement; // Penting untuk berpindah ke scene game utama

public class MainMenuManager : MonoBehaviour
{
    [Header("Hubungkan Semua Panel Wadah")]
    public GameObject panelLevel;
    public GameObject panelSettings;
    public GameObject panelAbout;

    [Header("Hubungkan Komponen Audio Musik")]
    public AudioSource audioMusikLatar;
    public Slider sliderVolumeMusik;

    void Start()
    {
        // Pastikan semua panel tersembunyi dengan rapi di awal menu utama
        if (panelLevel != null) panelLevel.SetActive(false);
        if (panelSettings != null) panelSettings.SetActive(false);
        if (panelAbout != null) panelAbout.SetActive(false);

        // Set slider volume agar posisinya sesuai dengan volume musik saat ini (bawaan 1 atau full)
        if (audioMusikLatar != null && sliderVolumeMusik != null)
        {
            sliderVolumeMusik.value = audioMusikLatar.volume;
            // Daftarkan fungsi pengubah volume otomatis saat slider digeser
            sliderVolumeMusik.onValueChanged.AddListener(SetVolume);
        }
    }

    // --- FUNGSI UNTUK TOMBOL UTAMA ---

    public void TombolPlayDitekan()
    {
        panelLevel.SetActive(true); // Memunculkan pilihan level Mudah / Sulit
    }

    public void TombolSettingsDitekan()
    {
        panelSettings.SetActive(true); // Memunculkan panel pengaturan volume
    }

    public void TombolAboutDitekan()
    {
        panelAbout.SetActive(true); // Memunculkan panel deskripsi & kelompok
    }

    // --- FUNGSI UNTUK TOMBOL CLOSE (X) ---

    public void ClosePanelLevel() => panelLevel.SetActive(false);
    public void ClosePanelSettings() => panelSettings.SetActive(false);
    public void ClosePanelAbout() => panelAbout.SetActive(false);

    // --- FUNGSI PILIHAN LEVEL ---

    public void PilihLevelMudah()
    {
        // Ganti "SampleScene" sesuai dengan nama asli scene game utama kamu yang di atas tadi
        SceneManager.LoadScene("SampleScene");
    }

    public void PilihLevelSulit()
    {
        SceneManager.LoadScene("SampleSceneSulit");
        // Catatan: Untuk level sulit nanti bisa kita arahkan ke scene baru atau menggunakan sistem lempar data.
    }

    // --- FUNGSI SLIDER VOLUME ---
    public void SetVolume(float nilaiVolume)
    {
        if (audioMusikLatar != null)
        {
            audioMusikLatar.volume = nilaiVolume; // Mengubah volume musik secara real-time
            PlayerPrefs.SetFloat("VolumeGame", nilaiVolume);
        }
    }

    // ========================================================
    // FUNGSI BARU: UNTUK TOMBOL EXIT (KELUAR GAME)
    // ========================================================
    public void KeluarDariGame()
    {
        Debug.Log("TOMBOL EXIT BERHASIL DIKLIK!");

#if UNITY_EDITOR
        // Jika sedang tes di Unity Editor, perintah ini akan mematikan mode Play otomatis
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Jika game sudah di-build jadi aplikasi (.exe / .apk), ini akan menutup game asli
        Application.Quit();
#endif
    }
}