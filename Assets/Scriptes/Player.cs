using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool estaVivo = true;
    [SerializeField] private int ouro;
    [SerializeField] private int vida;
    [SerializeField] private int forcaPulo;
    [SerializeField] private float velocidade;
    [SerializeField] private bool temChave;
    private int pontos = 0;
    private Rigidbody rb;
    private bool estaPulando;
    private Vector3 anglerotation;
    private bool doublejump;
    private int countJump = 0;
    private bool isJumping = false;
    private AudioSource audioP;
    [SerializeField] private bool pegando;
    [SerializeField] private bool podepegar;
    [SerializeField] private List<GameObject> inventario = new List<GameObject>();
    [Header("son do personagem")]
    [SerializeField] private AudioClip pulo;
    [SerializeField] private AudioClip queda;
    [SerializeField] private AudioClip morte;
    [SerializeField] private AudioClip moeda;
    private Diretor diretor;
    // Start is called before the first frame update
    void Start()
    {
        temChave = false;
        pegando = false;
        anglerotation = new Vector3(0, 90, 0);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        diretor = FindObjectOfType<Diretor>();
    }

    // Update is called once per frame

    void Update()
    {
        if (estaVivo)
        {
            TurnAround();

            //andar
            if (Input.GetKey(KeyCode.W))

            {
                animator.SetBool("Andar", true);
                Walk();
            }
            else if (Input.GetKey(KeyCode.S))

            {
                animator.SetBool("AndarPraTras", true);
                animator.SetBool("Andar", false);
                Walk();
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))

            {
                animator.SetBool("Andar", false);
            }
            else

            {
                animator.SetBool("Andar", false);
                animator.SetBool("AndarPraTras", false);
            }

            //avitar o bug da movimenta��o
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))

            {
                animator.SetBool("Andar", false);
                animator.SetBool("AndarPraTras", false);
            }

            //pulo
            if (Input.GetKeyDown(KeyCode.Space) && !estaPulando)

            {
                animator.SetTrigger("Pular");
                Jump();
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetTrigger("Pegando");
                pegando = true;
            }

            if (Input.GetMouseButtonDown(0))

            {
                animator.SetTrigger("Ataque");
            }

            //correr
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))

            {
                animator.SetBool("Correndo", true);
                Walk(8);
            }
            else

            {
                animator.SetBool("Correndo", false);
            }

            if (!estaVivo)

            {
                animator.SetTrigger("EsaVivo");
                estaVivo = true;
            }
        }

        if(!estaVivo)
        {
            //Destroy(this.gameObject);
        }
        
    }

    private void Walk(float velo = 1)

    {
        if ((velo == 1))
        {
            velo = velocidade;
        }
        float fowardInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * fowardInput;
        Vector3 moveForward = rb.position + moveDirection * velo * Time.deltaTime;
        rb.MovePosition(moveForward);
    }

    private void Jump()

    {
        rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
        estaPulando = true;
        animator.SetBool("EstaNoChao", false);
    }

    private void TurnAround()

    {
        float sideInput = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(anglerotation * sideInput * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void OnCollisionEnter(Collision other)

    {
        if (other.gameObject.CompareTag("Chao") || other.gameObject.CompareTag("Plataforma"))
        {
            estaPulando = false;
            animator.SetBool("EstaNoChao", true);
        }
       
            if (other.gameObject.CompareTag("Morte"))
        {
            estaVivo = false;

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            gameObject.transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            gameObject.transform.parent = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.tag);

        if (Input.GetKeyDown(KeyCode.E))

        {
            animator.SetTrigger("Pegando");
            pegando = true;
        }

        if (other.gameObject.CompareTag("Chave") && pegando)

        {
            inventario.Add(Instantiate(other.gameObject.GetComponent<chave>().CopiaDaChave()));
            int numero = other.gameObject.GetComponent<chave>().PegarNumeroChave();
            Debug.LogFormat($"chave numero: {numero} foi inserida no inventario");
            Destroy(other.gameObject);
            temChave = false;
            pegando = false;

        }

        if (other.gameObject.CompareTag("porta") && pegando && temChave)

        {
            other.gameObject.GetComponent<Animator>().SetTrigger("abrindo");
            temChave = true;
        }

        if (other.gameObject.CompareTag("Bau") && pegando)

        {
            if (VerificaChave(other.gameObject.GetComponent<Bau>().PegarNumeroFechadura()))
            {
                other.gameObject.GetComponent<Animator>().SetTrigger("abrir");

            }
            else
            {
                Debug.Log("voc� n�o tem a chave");
            }
        }

            pegando = false;

    }
    private bool VerificaChave(int chave)

    {
        foreach (GameObject item in inventario)
        {
            if (item.gameObject.CompareTag("Chave"))
            {
                if (item.gameObject.GetComponent<chave>().PegarNumeroChave() == chave)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerExit(Collider other)
    {
        pegando = false;
    }

    private void PegarConteudoBau(GameObject bau)
    {
        Bau bauTesouro = bau.GetComponent<Bau>();

        ouro = bauTesouro.PegarOuro();

        if (bauTesouro.AcessarConteudoBau() != null)
        {
            foreach (GameObject item in bauTesouro.AcessarConteudoBau())
            {
                inventario.Add(item);
            }

            bauTesouro.RemoverConteudoBau();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("moeda"))
        {
            audioP.PlayOneShot(moeda);
            pontos++;
            Destroy(other.gameObject);
        }
    }

    public bool VerificaSePlayerEstaVivo()
    {
        return estaVivo;
    }



}

