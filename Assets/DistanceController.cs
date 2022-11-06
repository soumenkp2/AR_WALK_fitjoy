/* -> This is a script written in C# that measures, tracks and calculates the location coordinates of a device in realtime.
   -> The position of the 'player' object is the position of the tracked device. Changing the position with change in device position 
      is handled by 'LocationProviderFactory.cs', 'ImmediatePositionWithLocationProvider.cs', and others in the Mapbox SDK resources.
   
   -> The changes in the position of 'player' when calculated is the distance travelled by the device.
   
   -> Rewards are spawned/ Instantiated/ created at specific distances. */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// DistanceController is called automatically at the start of the application.

public class DistanceController : MonoBehaviour
{

    // Declaration of GUI objects of the application.

    public Text distance_on_screen;
    public Transform user;
    public LineRenderer lineRenderer;
    public GameObject chest_reward;
    public GameObject spotLight;

    // Initialization of variables.

    public float timer;
    public float distance;
    public int int_distance;
    public int int_timer;

    // Private Variables

    int i, k;
    float virtual_distance;
    float real_distance;
    int start;
    int reward_dist;


    Vector3 position = new Vector3();

    [SerializeField] List<Vector3> position_list;


    void Start()
    {
        start = 1;                              // The time interval between collecting coordinates.
        reward_dist = 100;
    }

    void Update()
    {
        position = user.position;
        timer += Time.deltaTime;                                        // Gives sense of the real time.
        int_timer = (int)timer;

        if (start == int_timer)
        {
            start++;
            position_list.Add(position);                                // Adding the coordinates to a list.
            lineRenderer.positionCount = position_list.Count;

            if (start != 2)                                             // Need atleast two points to draw a line.
            {
                lineRenderer.SetPosition(k, position_list[position_list.Count - 1]);
                lineRenderer.SetPosition(k + 1, position_list[position_list.Count - 2]);
                k++;

                i = position_list.Count - 1;
                virtual_distance = DistanceBetweenPoints(position_list, i, i - 1);
                distance += virtual_distance;
                //distance *= 0.201078457f;
                real_distance = distance * 3.709071692f;                // 3.7... is the conversion factor of realtime cordinates to virtual unity coordinates.
                int_distance = (int)real_distance;

                distance_on_screen.text = "Distance: " + int_distance.ToString() + "m";
            }

            RewardHandler();


        }
    }

    float DistanceBetweenPoints(List<Vector3> position_list, int a, int b)
    {
        return Mathf.Sqrt(((position_list[a].x - position_list[b].x) * (position_list[a].x - position_list[b].x)) +
                    ((position_list[a].y - position_list[b].y) * (position_list[a].y - position_list[b].y)) +
                    ((position_list[a].z - position_list[b].z) * (position_list[a].z - position_list[b].z)));
    }

    void ChangeRewardDist()                                             // Generating rewards distances
    {
        reward_dist = reward_dist + (int)Random.Range((reward_dist * 0.7f), (reward_dist * 0.8f));
        Debug.Log(reward_dist);
    }

    public void Debuging()
    {
        distance += 2.5f;
    }

    void RewardHandler()
    {
        if (int_distance >= reward_dist - 10 && int_distance <= reward_dist + 10)       // Creation of 'Treasure Chest' 3D object using Instantiate at the desired reward positions.
        {
            ChangeRewardDist();
            Vector3 loc = new Vector3();
            loc.x = position.x + Random.Range(0.5f, 0.8f);
            loc.z = position.z + Random.Range(0.5f, 0.8f);
            Instantiate(chest_reward, new Vector3(loc.x, 3.0f, loc.z), Quaternion.identity);
            Instantiate(spotLight, new Vector3(loc.x, 0.7f, loc.z), Quaternion.identity);
        }
    }

}
