using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    /* 1) gotta make the enemy move and patroll, so what do we need?
    //- we need access to its Transform and access to its rigid body*/

    
    //Private Instance Variables
    private Transform _transform;
    //RigidBody2D is a built-in in unity 
    private Rigidbody2D _rigidbody;

    //part of step 2 to check if enemy is grounded
    private bool _isGrounded;

    //part of step four to detect if there is ground ahead, this variable will be passing on the boolean value to the instance variables
    //sightstarts and sight ends that are connected to the game objects.
    private bool GroundAhead;

    //this is part of step five where we are instantiating the game object LineOfSight, but first we gotta do some check, we need a boolean
    //variable to tell the object that player is detected, now act differently.
    private bool _IsPlayerDetected;

    //PUBLIC INSTANCE VARIABLES

    //this is for testing
    public float Speed= 5f;
    //part of step 5 where we define max speed if player is detected
    public float MaxSpeeed = 4f;


    // now we initiate step four where we are setting the object with three different GameObjects,SightStarts, SightEnds and 
    //we do this so we make sure the enemy does not fall off the platform and can flip w.e he is on edge, we the variables public so we can connect them in unity
    //to their respective game objects, remember they are related to distance and movement, so tey are obviously a Trasform function
    public Transform SightStarts;
    public Transform SightEnds;

    //5) Here we start Step 5 where we initiate the line of sight so if the player is spotted close by the enemy will run faster at him
    public Transform LineOfSight;
        
        // Use this for initialization
    void Start () {
        //we are making a reference to this object's Transform and RigidBody2D components
        this._transform = GetComponent<Transform>();
        this._rigidbody = GetComponent<Rigidbody2D>();

        //Step 2 since we are in the air first we initialize it as false
        this._isGrounded = false;
        //this is part of step four, we initiating GroundAhead instance variable as true otherwise it wont know what to do
        this.GroundAhead = true;

        //next part of step 5 now we are defininf the beggining variable for _IsPlayerDetected
        this._IsPlayerDetected = false;
    }

    // Update is called once per frame

    /* 2) //once we got our transform and rigid body or add forces we use Updates, we wanna use FixedUpdates so we edit the name
     when it comes to physics, rigid body or anything like that, USE FixedUpdates
     if we wanna update regular stuff like a button click or etc we use Updates
     we wanna check if enemy is ground or not*/

    void FixedUpdate()
    {

        /*Step 3) move the object in the direction of his local scale
         first we get access to rigidbody, then we add the velocity of the rigidbody to a new vector 2 object thus
         the velocity for this vector on an x scale is gonna be positive towards right if scale is 1 to 1 or -1 he is moving to the left
         then next step is multiply by a scaler value a non vector to determine the speed of the object so we go up n make public variable Speed just for testing*/

        /*this code would have worked this._rigidbody.velocity = new Vector2(this._transform.localScale.x,0) * this.Speed;
        but we update it into the following to make it only work when the object hit the ground, _Grounded=true.*/

        //now this code is better to check
        if (this._isGrounded) {
            this._rigidbody.velocity = new Vector2(this._transform.localScale.x, 0) * this.Speed;

            //now this is the last part of step four, here ur not gonna memorize this but understand it, we are saying that sight start.position is the beggining
            //and sightends object is the end for the gamecharacter to observe as a way of measurment to a LAYER we will coat on the platform that we will call Solid.
            //the layer will work as a piece of skin to further tell that if the endsight isnt touching it, do something else than moving.
            this.GroundAhead = Physics2D.Linecast(
                this.SightStarts.position,
                this.SightEnds.position,
                1 << LayerMask.NameToLayer("Solid"));

            /*here this is an important step of Step 5, basically a cut paste from the previous step n previous code, we defined the _IsPlayerAhead boolean variable
            we definied a new object LineOfSight, now we coated the player with a new skin layer called Player and gonna tell the enemy if u see that skin within
            LOS break'em in half!*/

            this._IsPlayerDetected = Physics2D.Linecast(
               this.SightStarts.position, //starts from SightStarts
               this.LineOfSight.position, //ends with Line of Sight
               1 << LayerMask.NameToLayer("Player")); //layer of Player


            //this is just for DEBUGGING and TESTING the whole GroundAhead and flip function
            //it'll draw a line from startsight object to start end object
            //to test they are both function and flips object once it reaches end of the line.
            Debug.DrawLine(this.SightStarts.position, this.SightEnds.position);

            //last part of step 5, testing the LineOfSight detection of Player by making a line between SightsTARTS and LOS
            Debug.DrawLine(this.SightStarts.position, this.LineOfSight.position);

            // this is also part of part four last part as it is included in the if condition _isGrounded and mentions that if 
            //GroundAhead is false, flip the game character.
            if (this.GroundAhead == false)
            {
                //flip the object if there is no ground ahead
                this._flip();
            }

            // this is also part of part 5 last part if player is detected increase speed to MaximumSpeed
            if (this._IsPlayerDetected){
                this.Speed *= 2;
            }
            if (this.Speed >= MaxSpeeed){
                this.Speed = MaxSpeeed;
            }

        }
    }

       
    //part of Step 3 now this is great but what you wanna do is whether he is grounded or not, what's the point of him moving if he can do it floating
    // in air? this is where we use the OnCollisionStay2D function, which says if on collision the object is staying (grounded)

    //object is grounded if it stays on platform
    private void OnCollisionStay2D(Collision2D other){

        if (other.gameObject.CompareTag("Platform")){

            this._isGrounded=true;
        }

        

    }

    //part of Step 3 object is Not grounded if it leaves platform
    private void OnCollisionExit2D(Collision2D other){
        if (other.gameObject.CompareTag("Platform")) { 
        this._isGrounded = false;
        }
    }


    //This here is STEP 6 in which the enemy if met with another enemy will collide with one another and flip directions
    private void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Enemy")){
            this._flip();
        }
    }

    //the flipping should occur once the _flip function is called in the offside of the GroundAhead condition
    //this flip method will flip the character's bitmap across the x-axis
    //it will flip his eyes to the right or the left
    private void _flip()
    {
        //the flip function from step four isnt gonna be using the isFacingRight variable
        //but will be the _transform.localscale that we assigned to the platform "Solid" 
        //BTW LOCALSCALE.x =1 means it is facing right.
        if (this._transform.localScale.x == 1)
        {

            //flip and go the other way where x is -1
            this._transform.localScale = new Vector2(-1f, 1f);
        }
        //if facing left then x-axis is -1 and y axis =1
        else
        {
            this._transform.localScale = new Vector2(1f, 1f);
        }
    }

}



