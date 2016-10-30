/* Source File Name: CheckPointController
 * Author's Name: Ibrahim Natchee
 * Last Modified By: Ibrahim Natchee
 * Date Modified Last: October 29 2016
 * Program Description: To controll behaviour of checkpoints
 * Revision History: October 29 2016
 
 */

using UnityEngine;
using System.Collections;

public class CheckPointController : MonoBehaviour {

    //Public Instance Variables

    //since we dealing ith location again, guess what we makin variables with TRANSFORM TYPE
    public GameObject SpawnPoint;


    //Private Instance Variables
    private Transform _transform;

	// Use this for initialization
	void Start () {

        this._transform = GetComponent<Transform>();

        //this is just when we add more checkpoint they would all come equipped with sawnpoint (game object) as a reference.
        this.SpawnPoint = GameObject.FindWithTag("SpawnPoint");

	}
    void Update()
    {

    }
    //once player enters the CheckPoint this event is triggered 
    void OnTriggerEnter2D(Collider2D other){

        if (other.gameObject.CompareTag("Player")) {
            this.SpawnPoint.transform.position = this._transform.position;
        }
    }
	
	
}
