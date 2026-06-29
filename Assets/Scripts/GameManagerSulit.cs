using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerSulit : MonoBehaviour
{
    public static GameManagerSulit instance;

    [Header("Hubungkan Objek Teks UI (Khusus Sulit)")]
    public TextMeshProUGUI teksSoalUI_Sulit;
    public TextMeshProUGUI teksSkorUI_Sulit;
    public TextMeshProUGUI teksNyawaUI_Sulit;

    [Header("Hubungkan Layar UI Panel (Khusus Sulit)")]
    public GameObject panelGameOverUI_Sulit;
    public GameObject panelMenangUI_Sulit;
    public GameObject panelPauseUI_Sulit; // <-- SLOT BARU

    [Header("Hubungkan Ketiga Kotak di Jalan (Khusus Sulit)")]
    public AnswerBox scriptBoxKiri_Sulit;
    public AnswerBox scriptBoxTengah_Sulit;
    public AnswerBox scriptBoxKanan_Sulit;

    [Header("Hubungkan Komponen Suara SFX (Khusus Sulit)")]
    public AudioSource pemutarSuara_Sulit;
    public AudioClip sfxJawabanBenar_Sulit;
    public AudioClip sfxJawabanSalah_Sulit;
    public AudioClip sfxGameOver_Sulit;
    public AudioClip sfxMenang_Sulit;

    [Header("Pengaturan Jarak Gerbang Baru")]
    public float jarakMajuKotakBaru_Sulit = 40f;

    private int skorSulit = 0;
    private int nyawaSulit = 3;
    private int jawabanBenarPerkalian;
    private bool gameSulitSelesai = false;

    private int jumlahSoalDikerjakan = 0;
    private int maksimalSoal = 5;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (panelGameOverUI_Sulit != null) panelGameOverUI_Sulit.SetActive(false);
        if (panelMenangUI_Sulit != null) panelMenangUI_Sulit.SetActive(false);
        if (panelPauseUI_Sulit != null) panelPauseUI_Sulit.SetActive(false);

        BuatSoalMatematikaSulit();
    }

    public void BuatSoalMatematikaSulit()
    {
        if (gameSulitSelesai) return;

        int angkaA = Random.Range(1, 10);
        int angkaB = Random.Range(1, 10);
        jawabanBenarPerkalian = angkaA * angkaB;

        if (teksSoalUI_Sulit != null) teksSoalUI_Sulit.text = angkaA + " x " + angkaB + " = ?";

        int jalurBenar = Random.Range(0, 3);
        SetAngkaKeKotakSulit(scriptBoxKiri_Sulit, jalurBenar == 0, jawabanBenarPerkalian);
        SetAngkaKeKotakSulit(scriptBoxTengah_Sulit, jalurBenar == 1, jawabanBenarPerkalian);
        SetAngkaKeKotakSulit(scriptBoxKanan_Sulit, jalurBenar == 2, jawabanBenarPerkalian);
    }

    void SetAngkaKeKotakSulit(AnswerBox kotak, bool apakahIniBenar, int jawabanBetul)
    {
        if (kotak == null) return;
        kotak.apakahBenar = apakahIniBenar;

        if (apakahIniBenar)
        {
            kotak.nilaiJawaban = jawabanBetul;
        }
        else
        {
            int jawabanSalah = jawabanBetul + Random.Range(-5, 6);
            if (jawabanSalah == jawabanBetul) jawabanSalah += 3;
            kotak.nilaiJawaban = jawabanSalah;
        }
        kotak.UpdateTeksAngka();
    }

    public void CekJawabanMobil(bool statusJawaban)
    {
        if (gameSulitSelesai) return;

        jumlahSoalDikerjakan++;

        if (statusJawaban == true)
        {
            skorSulit += 15;
            if (teksSkorUI_Sulit != null) teksSkorUI_Sulit.text = "SKOR : " + skorSulit;
            if (pemutarSuara_Sulit != null && sfxJawabanBenar_Sulit != null)
            {
                pemutarSuara_Sulit.PlayOneShot(sfxJawabanBenar_Sulit);
            }
        }
        else
        {
            nyawaSulit--;
            if (teksNyawaUI_Sulit != null) teksNyawaUI_Sulit.text = "NYAWA : " + nyawaSulit;
            if (pemutarSuara_Sulit != null && sfxJawabanSalah_Sulit != null)
            {
                pemutarSuara_Sulit.PlayOneShot(sfxJawabanSalah_Sulit);
            }

            if (nyawaSulit <= 0)
            {
                ProsesGameOverSulit();
                return;
            }
        }

        if (jumlahSoalDikerjakan >= maksimalSoal && nyawaSulit > 0)
        {
            ProsesMenangSulit();
            return;
        }

        PindahkanKotakKeDepanSulit();
        BuatSoalMatematikaSulit();
    }

    void PindahkanKotakKeDepanSulit()
    {
        float posisiZBaru = scriptBoxTengah_Sulit.transform.position.z + jarakMajuKotakBaru_Sulit;
        scriptBoxKiri_Sulit.transform.position = new Vector3(scriptBoxKiri_Sulit.transform.position.x, scriptBoxKiri_Sulit.transform.position.y, posisiZBaru);
        scriptBoxTengah_Sulit.transform.position = new Vector3(scriptBoxTengah_Sulit.transform.position.x, scriptBoxTengah_Sulit.transform.position.y, posisiZBaru);
        scriptBoxKanan_Sulit.transform.position = new Vector3(scriptBoxKanan_Sulit.transform.position.x, scriptBoxKanan_Sulit.transform.position.y, posisiZBaru);
    }

    void ProsesGameOverSulit()
    {
        gameSulitSelesai = true;
        if (panelGameOverUI_Sulit != null) panelGameOverUI_Sulit.SetActive(true);

        if (pemutarSuara_Sulit != null && sfxGameOver_Sulit != null)
        {
            pemutarSuara_Sulit.PlayOneShot(sfxGameOver_Sulit);
        }

        MatikanMobilSulit();
    }

    void ProsesMenangSulit()
    {
        gameSulitSelesai = true;
        if (panelMenangUI_Sulit != null) panelMenangUI_Sulit.SetActive(true);

        GameObject objekMusik = GameObject.Find("MusikPermainan");
        if (objekMusik != null)
        {
            AudioSource bgm = objekMusik.GetComponent<AudioSource>();
            if (bgm != null) bgm.Stop();
        }

        if (pemutarSuara_Sulit != null && sfxMenang_Sulit != null)
        {
            pemutarSuara_Sulit.PlayOneShot(sfxMenang_Sulit);
        }

        MatikanMobilSulit();
    }

    void MatikanMobilSulit()
    {
        PlayerController mobil = FindFirstObjectByType<PlayerController>();
        if (mobil != null)
        {
            mobil.MatikanMobil();
        }
    }

    // ==========================================
    // KHUSUS KODE BARU FITUR KONTROL NAVIGASI (SULIT)
    // ==========================================

    public void GameStopDitabrak_Sulit()
    {
        if (panelPauseUI_Sulit != null)
        {
            panelPauseUI_Sulit.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void GameLanjutkan_Sulit()
    {
        if (panelPauseUI_Sulit != null)
        {
            panelPauseUI_Sulit.SetActive(false);
            Time.timeScale = 1f; // Mengembalikan mobil berjalan
        }
    }

    public void UlangiLevel_Sulit()
    {
        Time.timeScale = 1f;
        // LOGIKA BARU: Mengulang scene berdasarkan nomor index panggung yang sedang aktif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void KembaliKeMenuUtama_Sulit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
