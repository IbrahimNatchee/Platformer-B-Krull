using UnityEngine;
using System.Collections;

public class KillScript : MonoBehaviour {

    //this script is to kill anything (object) that touches the death plane


    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("DeathPlane"))
        {
            Destroy(this.gameObject);
        }
    }

}