using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

//Private instance variables
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    //this variable is used to be able to move after assigning the _transform, _rigidbody to the current Transform/Rigidbody2d values
    //adding input from keyboard just for movement
    //Why did we use float as opposed to an integer or double? 
    //float values represent real values in real world such as height, weight and speed, float measures continous quantities.
    private float _move;

    //Public instance variables (for testing)
    // f signifies it's a float, if f is not added at the end it will not compile
    //Velocity is a variable within Unity
    public float Velocity = 10f;


    //we want the camera to follow the player so we have to reference the camera object
    public Camera camera;

	// Use this for initialization
	void Start () {

        //lets not pollute our start method with a bunch of initialization for variables so we made the initialize method instead.
        this._initialize();

	}

    //this methos is used to make is move but first we have to check there is an input to start with
    void FixedUpdate() {
        //check if input is present for movement
        //the Input.GetAxis("Horizontal" or "Vertical") is a built in function
        //go to unity-->Edit-->Project setting-->Input, you'll see the InputManager-->Horizatal-->ull see what keys activates it, where this built-in function is going
        this._move = Input.GetAxis("Horizontal");

        //now here we use this to sanitize the velocity, no decimals 1=moving right, -1=moving left, 0=not moving at all
        if (this._move > 0f) {
            this._move = 1;
        }
        else if (this._move < 0f) {
            this._move = -1;
        }
        else{
            this._move = 0f;
        }
        Debug.Log(this._move);

        //now we adding the movement, reason why we dont add this to transform is because if we do so and it runs to a block it keeps going through it, non stop
        //so to stop the avatar at a block we have to use the _move variable as it does not interfere with _transform variable
        //AddForce is what we want to add it to our _move, multiplying _move with Velocity and then by 0f when touching a rigid body, his or another's 
        //to put him in a HALT!
        //then afterwards add ForceMode2D.Force is used to stop as opposed ForceMode2d.Impact where it would knock him out.
        this._rigidbody.AddForce(new Vector2(this._move * this.Velocity, 0f), ForceMode2D.Force);

        //now that we moved the player we want the camera to follow
        //we use the _transform variable since it is connected to Transform object and transfer it's x and y position with -10f in Z axis so we can look at screen
        //multiplied each x and y position for camera by 0.8 to add a bit of a lag so it wouldnt breath down our neck
        this.camera.transform.position = new Vector3 (
            this._transform.position.x*0.8f,
            this._transform.position.y*0.8f,
            -10f);
    }

    // Update is called once per frame
    void Update () {
	
	}

    //this method is sued to initialize variables and objects when called
private void _initialize() {

        //lets initialize the _transform and _rigidbody variables by assigning them the same values as the current Transform and Rigibody2d
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._move = 0f;
    }

}
