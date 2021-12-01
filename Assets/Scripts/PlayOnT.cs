using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnT : MonoBehaviour
{
    public AudioSource someSound;
    public bool HasBeenPressed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(HasBeenPressed == false)
            {
                someSound.Play();
                HasBeenPressed = true;
            }
            else
            {
                
            }
        }
    }
}
