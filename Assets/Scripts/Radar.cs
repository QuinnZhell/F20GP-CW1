using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    Transform player;
    Transform radar;
    Transform canvas;
    float radarRange = 10f;
    [Header ("Blip Prefabs")]
    public GameObject sharkBlip;
    public GameObject treasureBlip;

    // Start is called before the first frame update
    void Start()
    {
        radar = this.transform;
        player = GameObject.Find("Player").transform;
        canvas = GameObject.Find("UI").transform;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBlips();
        Scan("Enemy", sharkBlip);
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
        float x = pos.x * 5;
        float y = pos.z * 5;

        // player position is taken as origin so rotation is around y
        float r = player.rotation.y;

        // player is considered origin in radar
        float blipX = x * Mathf.Cos(r) - y * Mathf.Sin(r);
        float blipY = y * Mathf.Cos(r) + x * Mathf.Sin(r);

        return new Vector2(blipX, blipY);
    }

    void DrawBlip(Vector2 pos, GameObject prefab){
        GameObject newBlip = Instantiate(prefab, radar, false);
        newBlip.transform.SetParent(radar);
        newBlip.transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
    }
}
