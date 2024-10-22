using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class chave : MonoBehaviour
{
    [SerializeField] private int numeroChave;

    public GameObject CopiaDaChave()
    {
        gameObject.SetActive(false);
        return gameObject;
    }

    public int PegarNumeroChave()
    {
        return numeroChave;
    }


}
