using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    Transform player;
    Transform radar;
    Transform canvas;
    float radarRange = 30f;
    float radarScale;
    [Header ("Blip Prefabs")]
    public GameObject sharkBlip;
    public GameObject treasureBlip;

    // Start is called before the first frame update
    void Start()
    {
        radar = this.transform;
        player = GameObject.Find("Player").transform;
        canvas = GameObject.Find("UserInterface").transform;
        radarScale = 50 / radarRange;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBlips();
        Scan("Shark", sharkBlip);
        Scan("Treasure", treasureBlip);
    }

    // destroy blips at each cycle
    void DestroyBlips() {
        GameObject[] blips = GameObject.FindGameObjectsWithTag("Blip");
        foreach (GameObject blip in blips) {
            Destroy(blip);
        }
    }

    void Scan(string tag, GameObject blipPrefab) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects) {
            Vector3 pos = obj.transform.position;

            // only want the distance on the x and z axis
            Vector3 d = pos - player.position;
            d.y = 0;

            // get final distance
            float distance = d.magnitude;

            // only display objects within 10 units
            if (distance <= radarRange) {

                Vector2 blipPos = CalculateRadarPosition(d);
                // Debug.Log("Blip Position: " + blipPos);
                DrawBlip(blipPos , blipPrefab);
            }
        }
    }

    Vector2 CalculateRadarPosition(Vector3 pos) {

        // scale position
        float x = pos.x * radarScale;
        float y = pos.z * radarScale;

        // player position is taken as origin so rotation is around y
        float r = player.rotation.eulerAngles.y;
        // change to degrees
        r = r * (Mathf.PI/180);

        //Debug.Log("Rotation around origin: " + r);

        // player is considered origin in radar
        float blipX = x * Mathf.Cos(r) - y * Mathf.Sin(r);
        float blipY = y * Mathf.Cos(r) + x * Mathf.Sin(r);

        //Debug.Log("scaled:" + x + " , " + y);
        //Debug.Log("rotated:" + blipX + " , " + blipY);

        return new Vector2(blipX, blipY);
    }

    void DrawBlip(Vector2 pos, GameObject prefab){
        GameObject newBlip = Instantiate(prefab, radar, false);
        newBlip.transform.SetParent(radar);
        newBlip.transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }
}
