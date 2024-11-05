using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
public void play()
{
    SceneManager.LoadScene("Inicio");
}
public void Mapa2()
{
   SceneManager.LoadScene("mapa2");
}
}