using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelEnd : MonoBehaviour
{
    public GameObject Panel;
    public GameObject MenuButton;
    int i = 0;
    private void Start()
    {
        //GameManager.getInstance().goPlayer.GetComponent<PlayerMovement>().pePanelEnd = this;
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartAnim()
    {
        if (i == 0)
        {
            Panel.GetComponent<Animation>().Play();
            i++;
        }
    }
}
