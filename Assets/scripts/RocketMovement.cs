using UnityEngine;
using UnityEngine.InputSystem ; 



public class RocketMovement : MonoBehaviour
{
    [SerializeField] InputAction thrust  ;  // button that we bind  -> input action 
    [SerializeField] float thrustStrength  = 700f ;
    [SerializeField] InputAction rotation ; 
    [SerializeField] float rotationStrength =  10f ;
    [SerializeField] AudioClip ThrustAudio;
    [SerializeField] ParticleSystem MainThrust; 
    [SerializeField] ParticleSystem LeftThrust; 
    [SerializeField] ParticleSystem RightThrust;
    [SerializeField]  int ParticleCount = 5; 

    Rigidbody  rb ;
    AudioSource audioSource; 
 

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>() ; 
        audioSource = GetComponent<AudioSource>() ; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        // checks every second for the us pressed  
        ProcessThrust() ; 
        // ProcessRotation() ;
        ProcessRotation() ; 
        
        
    }

    private void ProcessThrust() {

        if (thrust.IsPressed())
        {
            StartThrusting();

        }
        else
        {
            StopThrusting();
        }


    }

    private void StartThrusting()
    {
        // Vector3.up == (0,1,0) ; 
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        // play particle system  
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(ThrustAudio);
        }

        if (!MainThrust.isPlaying)
        {
            MainThrust.Play();

        }
    }

    private void StopThrusting()
    {
        MainThrust.Stop();
        audioSource.Stop();
    }

    

    private void ProcessRotation()
    {

        float RotationInput = rotation.ReadValue<float>();
        // -ve binding button has been pressed  
        if (RotationInput < 0)
        {
            RotateLeft();
        }
        else if (RotationInput > 0)
        {
            RotateRight();

        }
        else
        {
            StopRotation();
        }
    }

    private void RotateLeft()
    {
        RotationDoer(rotationStrength);
        if (!LeftThrust.isPlaying)
        {
            RightThrust.Stop();
            LeftThrust.Play();
        }
    }

    private void RotateRight()
    {
        RotationDoer(-rotationStrength);
        // transform.Rotate(Vector3.forward * -rotationStrength * Time.fixedDeltaTime) ; 
        if (!RightThrust.isPlaying)
        {
            LeftThrust.Stop();
            RightThrust.Play();
        }
    }


    private void StopRotation()
    {
        RightThrust.Stop();
        LeftThrust.Stop();
    }

    

    

    private void RotationDoer(float RotatePerFrame) {
        rb.freezeRotation = true   ;
        transform.Rotate(Vector3.forward * RotatePerFrame* Time.fixedDeltaTime) ; 
        rb.freezeRotation = false   ;   
    }
}
