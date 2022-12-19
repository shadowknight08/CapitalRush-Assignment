using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float shootRange = 100f; // The maximum range of the raycast
    public int numRays = 360; // The number of rays to shoot
    Vector3 point1;
    Vector3 point2;
    public  GameObject emtyObject;

    public List<GameObject> cop_list = new List<GameObject>();


    LineRenderer lineRenderer;

    public List<Vector3> cop_list_direction = new List<Vector3>();

    public List<Vector3> cop_list_angle = new List<Vector3>();
    public List<Vector3> cop_list_final_angle = new List<Vector3>();

    public List<Vector3> cop_list_beetween_angle = new List<Vector3>();
    public LayerMask cops;
    public void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.forward* shootRange);
    }
    void Update()
    {
        // Check for input to shoot the raycasts
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("button clicked");

            Collider[] hits = Physics.OverlapBox(gameObject.transform.position, new Vector3(10, 5, 10),Quaternion.identity,LayerMask.GetMask("Cops"));

            Debug.Log(hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] != null)
                {
                    GameObject cop = hits[i].GetComponent<GameObject>();
                    if (cop != null)
                    {
                        cop_list.Add(cop);
                        Debug.Log("cop added");

                        Vector3 dir1 = (cop.transform.position - transform.position).normalized;
                        cop_list_direction.Add(dir1);
                    }

                }
            }

            StartCoroutine(FinalDirection());
        }
    }


    public IEnumerator FinalDirection()
    {
        for (int i = 0; i < cop_list_direction.Count; i++)
        {
            float angleX = Vector3.Angle(new Vector3(1, 0, 0), cop_list_direction[i]);
            float anglez = Vector3.Angle(new Vector3(0, 0, 1), cop_list_direction[i]);

            cop_list_angle.Add(new Vector3(angleX, 0,anglez));
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < cop_list_angle.Count; i++)
        {
            if (cop_list_angle[i].z >= 0f && cop_list_angle[i].z <= 90f)
            {
                cop_list_final_angle.Add(new Vector3(cop_list_angle[i].x,0,i));
            }
            else if (cop_list_angle[i].z >= 90f && cop_list_angle[i].z <= 180f)
            {
                cop_list_final_angle.Add(new Vector3(360f - cop_list_angle[i].x,0, i));
            }
        }

        cop_list_final_angle.Sort((a, b) => a.x.CompareTo(b.x));

        float val = cop_list_final_angle[0].x;

        for (int i = 0; i < cop_list_final_angle.Count; i++)
        {
            cop_list_final_angle[i] = new Vector3(cop_list_final_angle[i].x - val, 0,cop_list_final_angle[i].z);
        }

        for (int i = 0; i < cop_list_final_angle.Count - 1; i++)
        {
            cop_list_beetween_angle.Add(new Vector3(cop_list_final_angle[i + 1].x - cop_list_final_angle[i].x, cop_list_final_angle[i=1].z, cop_list_final_angle[i].z));
        }
        cop_list_beetween_angle.Add(new Vector3(360 - cop_list_final_angle[9].x, cop_list_final_angle[0].z, cop_list_final_angle[9].z));

        cop_list_beetween_angle.Sort((a, b) => a.x.CompareTo(b.x));

        for (int i = 0; i < cop_list_final_angle.Count; i++)
        {
            if (cop_list_beetween_angle[9].z == cop_list_final_angle[i].z)
            {
                // transform.localEulerAngles = new Vector3(0f, cop_list_final_angle[i].x + (cop_list_beetween_angle[9].x / 2) + val,0f );
                //lineRenderer.enabled = true;
                Debug.Log("angleFound");
            }
        }
    }
    // This method takes in an angle in degrees and returns a direction vector with the corresponding rotation
    Vector3 GetDirectionFromAngle(float angle,int numrays)
    {
        float realangle = 360 / numrays;
        // Convert the angle from degrees to radians
        float angleRad =  angle * Mathf.Deg2Rad;

        // Calculate the direction vector using the sine and cosine of the angle
        Vector3 direction = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));

        return direction;
    }
}
