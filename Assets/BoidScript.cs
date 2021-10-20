//////////////////////////////////////////////////////////////////
//                       BOID SIMULATION                        //
//                     By - ANAV CHAUDHARY                      //
//                                                              //
//                                                              //
//          Reference - Psuedocode by Conrad Parker             //
//   (http://www.vergenet.net/~conrad/boids/pseudocode.html)    //
//////////////////////////////////////////////////////////////////

//This script Handles the compelete boid simultaion as a central controller
//Each Boid is a Prefab with the BoidObject Script that stores velocity and handles its rotation 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidScript : MonoBehaviour
{

    [SerializeField]
    GameObject Boid;  //Reference to Boid Prefab Containing Sprite

    GameObject[] BoidArray;  //Array to keep reference of flock

    int NumBoids = 100;  //Number of boids to be displayed

    float MaxSpeed = 30.0f;  //MaxSpeed of one Boid

    float WIDTH = 40.0f, HEIGHT = 15.0f;  //Bounds

    [SerializeField]
    Slider CohesionSlider;
    [SerializeField]
    Slider SeperationSlider;
    [SerializeField]
    Slider AlignmentSlider;
    [SerializeField]
    Slider VisualSlider; // Visual Range

    // Start is called before the first frame update
    void Start()
    {
        //Random Initialisation of Flock

        BoidArray = new GameObject[NumBoids];
        for (int i = 0; i<NumBoids; i++) {
            float PosX = (WIDTH - (WIDTH * Random.Range(0.05f, 1.95f)));
            float PosY = (HEIGHT - (HEIGHT * Random.Range(0.05f, 1.95f)));
            BoidArray[i] = Instantiate(Boid, new Vector2(PosX, PosY), Quaternion.identity);
            BoidArray[i].GetComponent<BoidObject>().velocity = new Vector2 (Random.Range(-30.0f, 30.0f), Random.Range(-30.0f, 30.0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 seperationVector, alignmentVector, cohesionVector;

        foreach (GameObject boid in BoidArray) {

            // 3 Rules a Boid must follow, as described by Craig Reynolds (http://www.red3d.com/cwr/boids/) 
            seperationVector = Seperation(boid);
            alignmentVector = Alignment(boid);
            cohesionVector = Cohesion(boid);

            // Setting Velocity after applying the 3 rules + clamping to ensure finite speed
            boid.GetComponent<BoidObject>().velocity += seperationVector + alignmentVector + cohesionVector;
            if (boid.GetComponent<BoidObject>().velocity.magnitude > MaxSpeed) {
                boid.GetComponent<BoidObject>().velocity = (boid.GetComponent<BoidObject>().velocity / boid.GetComponent<BoidObject>().velocity.magnitude) * MaxSpeed;
            }
            
            // Maintaining Flock inside Bounds
            boid.transform.position += new Vector3 (boid.GetComponent<BoidObject>().velocity.x, boid.GetComponent<BoidObject>().velocity.y, 0.0f) * (Time.deltaTime/2.0f);
            if ((boid.transform.position.x) > WIDTH) {
                boid.GetComponent<BoidObject>().velocity = new Vector2 (-30.0f, boid.GetComponent<BoidObject>().velocity.y);
                
            }
            if ((boid.transform.position.x) < -WIDTH + 20.0f) { //Special offset to keep flock from going underneath the sliders
                boid.GetComponent<BoidObject>().velocity = new Vector2 (30.0f, boid.GetComponent<BoidObject>().velocity.y);
            }
            if ((boid.transform.position.y) > HEIGHT) {
                boid.GetComponent<BoidObject>().velocity = new Vector2 (boid.GetComponent<BoidObject>().velocity.x, -30.0f);
            }
            if ((boid.transform.position.y) < -HEIGHT) {
                boid.GetComponent<BoidObject>().velocity = new Vector2 (boid.GetComponent<BoidObject>().velocity.x, 30.0f);
            }
        }

    }

    // Seperation Rule - Boids steer away form other boids to avoid collision
    // here the boids are slightly nugded each frame they are within a certain distance of another boid
    Vector2 Seperation(GameObject Boid) {
        Vector2 seperate = new Vector2(0.0f, 0.0f);

        foreach(GameObject boid in BoidArray) {
            if (boid != Boid) {
                Vector2 distance = (Vector2)boid.transform.position - (Vector2)Boid.transform.position;
                if (distance.magnitude < 1.0f) {
                    seperate = seperate - distance;
                }
            }
        }

        return seperate * SeperationSlider.value;
    }

    // Alignment Rule - Boids try to align themselves with the rest of the flock
    Vector2 Alignment(GameObject Boid) {
        Vector2 percievedVelocity = new Vector2 (0.0f, 0.0f);
        float num = 0;
        foreach(GameObject boid in BoidArray) {
            float distance = (boid.transform.position - Boid.transform.position).magnitude;
            if (boid != Boid && distance < VisualSlider.value) {
                percievedVelocity += Boid.GetComponent<BoidObject>().velocity;
                num++;
            }
        }

        if (num > 0) {
            percievedVelocity /= (num);
        }
        
        return ((percievedVelocity - Boid.GetComponent<BoidObject>().velocity)* AlignmentSlider.value);
    }

    // Cohesion Rule - Boids try to move to the percieved centre of mass of the flock
    Vector2 Cohesion(GameObject Boid) {
        Vector2 positionCOM = new Vector2 (0.0f, 0.0f);
        float num = 0;
        foreach (GameObject boid in BoidArray) {
            float distance = (boid.transform.position - Boid.transform.position).magnitude;
            if (boid != Boid && distance < VisualSlider.value) {
                positionCOM += (Vector2)boid.transform.position;
                num++;
            }
        }

        if (num > 0) {
            positionCOM /= (num);
        }

        return ((positionCOM - (Vector2)Boid.transform.position) * CohesionSlider.value);
    }  
}
