using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

    public bool On;
    public float OnTimer;
    public float OffTimer;

	int num;
	int posX;
	int posY;
	GridMap map;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!On)
        {
            GetComponent<ParticleSystem>().Play();
            GetComponent<BoxCollider>().enabled = false;
            Invoke("GateOnTimer", OffTimer);
        }
        else
        {
            
            GetComponent<BoxCollider>().enabled = true;
            Invoke("GateOffTimer", OnTimer);
        }
    }


    void GateOffTimer()
    {
        On = false;
    }

    void GateOnTimer()
    {
        On = true;
    }

	public void GetListing(int count, int xPos, int yPos, GameObject m){
		num = count;
		posX = xPos;
		posY = yPos;
		map = m.GetComponent<GridMap> ();
	}
}
