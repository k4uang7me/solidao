using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Diretor : MonoBehaviour
{
    //player
    [SerializeField] private Player player;
    //tela da morte
    [SerializeField] private GameObject telaMorte;
    //audio source
    [SerializeField] private AudioSource audioPlayer;
    //som de morte
    [SerializeField] private AudioClip morte;
    // texto pontos
    //Avisos
    [Header("Aviso Missão")]
    [SerializeField] private TextMeshProUGUI avisoMissao;
    [SerializeField] private AudioClip somMIssao;
    [SerializeField] private GameObject luzSaida;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        audioPlayer = GetComponent<AudioSource>();
        telaMorte.SetActive(false);
        //telaVitoria.SetActive(false
        luzSaida.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.VerificaSePlayerEstaVivo())
        {
            audioPlayer.PlayOneShot(morte);
            telaMorte.SetActive(true);
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Replay2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private static void Sair()
    {
        Application.Quit();
    }

}