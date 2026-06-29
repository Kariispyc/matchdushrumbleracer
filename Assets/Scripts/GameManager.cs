using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Hubungkan Objek Teks UI")]
    public TextMeshProUGUI teksSoalUI;
    public TextMeshProUGUI teksSkorUI;
    public TextMeshProUGUI teksNyawaUI;

    [Header("Hubungkan Layar UI Panel")]
    public GameObject panelGameOverUI;
    public GameObject panelMenangUI;
    public GameObject panelPauseUI; // <-- SLOT BARU: Untuk Menu Pause

    [Header("Hubungkan Ketiga Kotak di Jalan")]
    public AnswerBox scriptBoxKiri;
    public AnswerBox scriptBoxTengah;
    public AnswerBox scriptBoxKanan;

    [Header("Hubungkan Komponen Suara (SFX)")]
    public AudioSource pemutarSuara;
    public AudioClip sfxJawabanBenar;
    public AudioClip sfxJawabanSalah;
    public AudioClip sfxGameOver;
    public AudioClip sfxMenang;

    [Header("Pengaturan Jarak Gerbang Baru")]
    public float jarakMajuKotakBaru = 40f;

    private int skor = 0;
    private int nyawa = 3;
    private int jawabanBenar;
    private bool gameSelesai = false;

    private int jumlahSoalDikerjakan = 0;
    private int maksimalSoal = 5;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Pastikan waktu Unity berjalan normal di awal game
        Time.timeScale = 1f;

        if (panelGameOverUI != null) panelGameOverUI.SetActive(false);
        if (panelMenangUI != null) panelMenangUI.SetActive(false);
        if (panelPauseUI != null) panelPauseUI.SetActive(false); // Sembunyikan panel pause di awal

        BuatSoalMatematika();
    }

    public void BuatSoalMatematika()
    {
        if (gameSelesai) return;

        int angkaA = Random.Range(1, 10);
        int angkaB = Random.Range(1, 10);
        jawabanBenar = angkaA + angkaB;

        if (teksSoalUI != null) teksSoalUI.text = angkaA + " + " + angkaB + " = ?";

        int jalurBenar = Random.Range(0, 3);
        SetAngkaKeKotak(scriptBoxKiri, jalurBenar == 0, jawabanBenar);
        SetAngkaKeKotak(scriptBoxTengah, jalurBenar == 1, jawabanBenar);
        SetAngkaKeKotak(scriptBoxKanan, jalurBenar == 2, jawabanBenar);
    }

    void SetAngkaKeKotak(AnswerBox kotak, bool apakahIniBenar, int jawabanBetul)
    {
        if (kotak == null) return;
        kotak.apakahBenar = apakahIniBenar;

        if (apakahIniBenar)
        {
            kotak.nilaiJawaban = jawabanBetul;
        }
        else
        {
            int jawabanSalah = jawabanBetul + Random.Range(-3, 4);
            if (jawabanSalah == jawabanBetul) jawabanSalah += 2;
            kotak.nilaiJawaban = jawabanSalah;
        }
        kotak.UpdateTeksAngka();
    }

    public void CekJawabanMobil(bool statusJawaban)
    {
        if (gameSelesai) return;

        jumlahSoalDikerjakan++;

        if (statusJawaban == true)
        {
            skor += 10;
            if (teksSkorUI != null) teksSkorUI.text = "SKOR : " + skor;
            if (pemutarSuara != null && sfxJawabanBenar != null)
            {
                pemutarSuara.PlayOneShot(sfxJawabanBenar);
            }
        }
        else
        {
            nyawa--;
            if (teksNyawaUI != null) teksNyawaUI.text = "NYAWA : " + nyawa;
            if (pemutarSuara != null && sfxJawabanSalah != null)
            {
                pemutarSuara.PlayOneShot(sfxJawabanSalah);
            }

            if (nyawa <= 0)
            {
                ProsesGameOver();
                return;
            }
        }

        if (jumlahSoalDikerjakan >= maksimalSoal && nyawa > 0)
        {
            ProsesMenang();
            return;
        }

        PindahkanKotakKeDepan();
        BuatSoalMatematika();
    }

    void PindahkanKotakKeDepan()
    {
        float posisiZBaru = scriptBoxTengah.transform.position.z + jarakMajuKotakBaru;
        scriptBoxKiri.transform.position = new Vector3(scriptBoxKiri.transform.position.x, scriptBoxKiri.transform.position.y, posisiZBaru);
        scriptBoxTengah.transform.position = new Vector3(scriptBoxTengah.transform.position.x, scriptBoxTengah.transform.position.y, posisiZBaru);
        scriptBoxKanan.transform.position = new Vector3(scriptBoxKanan.transform.position.x, scriptBoxKanan.transform.position.y, posisiZBaru);
    }

    void ProsesGameOver()
    {
        gameSelesai = true;
        if (panelGameOverUI != null) panelGameOverUI.SetActive(true);

        if (pemutarSuara != null && sfxGameOver != null)
        {
            pemutarSuara.PlayOneShot(sfxGameOver);
        }

        MatikanPergerakanMobil();
    }

    void ProsesMenang()
    {
        gameSelesai = true;
        if (panelMenangUI != null) panelMenangUI.SetActive(true);

        GameObject objekMusik = GameObject.Find("MusikPermainan");
        if (objekMusik != null)
        {
            AudioSource bgm = objekMusik.GetComponent<AudioSource>();
            if (bgm != null) bgm.Stop();
        }

        if (pemutarSuara != null && sfxMenang != null)
        {
            pemutarSuara.PlayOneShot(sfxMenang);
        }

        MatikanPergerakanMobil();
    }
    public void PindahKeLevelSulit()
    {
        Time.timeScale = 1f; // Pastikan waktu berjalan normal kembali
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleSceneSulit");
    }
    void MatikanPergerakanMobil()
    {
        PlayerController mobil = FindFirstObjectByType<PlayerController>();
        if (mobil != null)
        {
            mobil.MatikanMobil();
        }
    }

    // ==========================================
    // KHUSUS KODE BARU FITUR KONTROL NAVIGASI
    // ==========================================

    public void GameStopDitabrak()
    {
        if (panelPauseUI != null)
        {
            panelPauseUI.SetActive(true);
            Time.timeScale = 0f; // Menghentikan waktu game (Mobil membeku)
        }
    }

    public void GameLanjutkan()
    {
        if (panelPauseUI != null)
        {
            panelPauseUI.SetActive(false);
            Time.timeScale = 1f; // Mengaktifkan kembali waktu game (Mobil jalan lagi)
        }
    }

    public void UlangiLevel()
    {
        Time.timeScale = 1f; // Pastikan waktu normal
        // LOGIKA BARU: Mengulang scene berdasarkan nomor index panggung yang sedang aktif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KembaliKeMenuUtama()
    {
        Time.timeScale = 1f; // WAJIB: Normalkan waktu sebelum kembali ke menu utama
        SceneManager.LoadScene("MainMenu");
    }
}