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

    //take a guess what this does, hint, you jump
    private float _jump;

    private bool _isFacingRight;

    //this variable will ensure that the player is grounded as soon we do that we can initiate the jump method
    private bool _isGrounded;

    //Public instance variables (for testing)
    // f signifies it's a float, if f is not added at the end it will not compile
    //Velocity is a variable within Unity
    public float Velocity = 10f;

    //this is strictly for jumping, why a 100f?why more than velocity? because gravity is pressing me down.
    public float JumpForce = 100f;

    //we want the camera to follow the player so we have to reference the camera object
    public Camera camera;

    //this variable is used as a spawn point to respawn player at a certain point of the game
    //keep in mind that any given moment we want something to do with LOCATION we use Transform built-in function from unity, as such we are making this variable
    //of type Transform

    public Transform SpawnPoint;


    [Header("Sound Clips")]
    // this will be the variable to instaniate everytime the player jumps, dies or breakdance
    public AudioSource JumpSound;
    public AudioSource DeathSound;


    
    // Use this for initialization
	void Start () {

        //lets not pollute our start method with a bunch of initialization for variables so we made the initialize method instead.
        this._initialize();
        
	}

    //this methos is used to make is move but first we have to check there is an input to start with
    void FixedUpdate() {

        //reason why we added the if to all the rest of the comments is if he falls he shouldnt be able to move, be halted by an object or flip
        //camera should always move whther he is falling or not
        //else everything else is in effect
        if (this._isGrounded) { 

        //check if input is present for movement
        //the Input.GetAxis("Horizontal" or "Vertical") is a built in function
        //go to unity-->Edit-->Project setting-->Input, you'll see the InputManager-->Horizatal-->ull see what keys activates it, where this built-in function is going
        this._move = Input.GetAxis("Horizontal");

        //now here we use this to sanitize the velocity, no decimals 1=moving right, -1=moving left, 0=not moving at all
        if (this._move > 0f) {
            this._move = 1;
            this._isFacingRight = true;
            this.flip();
        }
        else if (this._move < 0f) {
            this._move = -1;
            //we movem back or forth and then we say if he is moving right or left through the flip method
            this._isFacingRight = false;
            this.flip();
        }
        else{
            this._move = 0f;
        }
        

        //this if is to check if we have input for jumping
        if (Input.GetKeyDown(KeyCode.Space))
            {
                this._jump = 1f;

                //this will make a sound when the player jump
                this.JumpSound.Play ();
            }
        
        //now we adding the movement, reason why we dont add this to transform is because if we do so and it runs to a block it keeps going through it, non stop
        //so to stop the avatar at a block we have to use the _move variable as it does not interfere with _transform variable
        //AddForce is what we want to add it to our _move, multiplying _move with Velocity and then by 0f when touching a rigid body, his or another's 
        //to put him in a HALT!
        //then afterwards add ForceMode2D.Force is used to stop as opposed ForceMode2d.Impact where it would knock him out.
        //in addition we are grounded in this scenario so if jumping we multiply _jump variable with jumpforce and still including the ForceMode2d.Force.
        this._rigidbody.AddForce(new Vector2(this._move * this.Velocity, 
            this._jump*this.JumpForce), 
            ForceMode2D.Force);
}
        //if we not moving or jumping
        else {
            this._move = 0f;
            this._jump = 0f;

        }



        //now that we moved the player we want the camera to follow
        //we use the _transform variable since it is connected to Transform object and transfer it's x and y position with -10f in Z axis so we can look at screen
        //multiplied each x and y position for camera by 0.8 to add a bit of a lag so it wouldnt breath down our neck
        this.camera.transform.position = new Vector3 (
            this._transform.position.x,
            this._transform.position.y,
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
        this._isFacingRight = true;

        //in the beggining he is falling, so that's what we initiate
        this._isGrounded = false;
    }

    
    //this flip method will flip the character's bitmap across the x-axis
    //it will flip his eyes to the right or the left
    private void flip() {
        //if he is facing right then x-axis and y-axis is normalized to 1
        if (this._isFacingRight){

            this._transform.localScale = new Vector2(1f, 1f);}
        //if facing left then x-axis is -1 and y axis =1
        else {
            this._transform.localScale = new Vector2(-1f, 1f);}
    }


    //this is if player, avatar touches the death plane, one of the most important tools ull use is the OnCollisionStay/Enter2D and CompareTag built-in function
    private void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("DeathPlane")){

           


            //move the player's position to the spawn point's position

            //just saying if it touches the death plane, change the spawn point's position into the _transform's position which that, is equal tot he current Transform position
            this._transform.position = this.SpawnPoint.position;
            //this will initiate the death sound
            this.DeathSound.Play();
        }

    }




    //these two functions will simply define if we are grounded or not on top of platform. 
    //thus seeing if we can jump or not, we cant jump if we not grounded.
    private void OnCollisionStay2D(Collision2D other){
        if (other.gameObject.CompareTag ("Platform")){
            this._isGrounded = true; }

 }

    private void OnCollisionExit2D(Collision2D other){
        this._isGrounded = false;
    }
}
