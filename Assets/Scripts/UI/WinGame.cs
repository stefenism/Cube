using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public GameObject winUI;
    bool ran = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerActor player)&& !ran)
        {
            ran = true;

            Invoke("EnableUI", 3);
            Invoke("QuitGame", 7);
            
        }
    }

    void EnableUI()
    {
        winUI.SetActive(true);
    }
    void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
