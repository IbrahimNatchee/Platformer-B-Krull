/* Source File Name: KillScript
 * Author's Name: Ibrahim Natchee
 * Last Modified By: Ibrahim Natchee
 * Date Modified Last: October 29 2016
 * Program Description: To game objects
 * Revision History: October 29 2016
 
 */
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