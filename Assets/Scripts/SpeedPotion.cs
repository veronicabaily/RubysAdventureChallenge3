using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    public AudioClip collectedClip;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.speed < controller.maxSpeed)
            {
                controller.ChangeSpeed(2);
                Destroy(gameObject);
            
                controller.PlaySound(collectedClip);
            }
        }
    }
}