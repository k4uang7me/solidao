using System;
using System.Collections.Generic;
using UnityEngine;

public class Play : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool estaVivo = true;
    [SerializeField] private int ouro;
    [SerializeField] private int vida;
    [SerializeField] private int forcaPulo;
    [SerializeField] private float velocidade;
    [SerializeField] private bool temChave;
    private Rigidbody rb;
    private bool estaPulando;
    private Vector3 anglerotation;
    [SerializeField] private bool pegando;
    [SerializeField] private bool podepegar;
    [SerializeField] private List<GameObject> inventario = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        temChave = false;
        pegando = false;
        anglerotation = new Vector3(0, 90, 0);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    void Update()
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
            animator.SetBool("andar", false);
            Walk();
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))

        {
            animator.SetBool("andar", false);
        }
        else

        {
            animator.SetBool("Andar", false);
            animator.SetBool("AndarPraTras", false);
        }

        //avitar o bug da movimentação
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

    private void OnCollisionEnter(Collision collision)

    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estaPulando = false;
            animator.SetBool("EstaNoChao", true);
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
                Debug.Log("você não tem a chave");
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

}

