using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridMap : MonoBehaviour {


	[Tooltip("Drag your level image here to test")]public Texture2D map; //must be RW enabled and a TGA file
	public int[,] colMap;
    private bool generateArray = false;
	public GameObject smallAsteroid;
    private GameObject bouncePad;
	private GameObject gate;
	private GameObject xSpawn;
	private GameObject bSpawn;
	private GameObject ySpawn;
	private GameObject aSpawn;

    private GameObject X;
    private GameObject B;
    private GameObject Y;
    private GameObject A;
	private GameObject p1;
	private GameObject p2;
	private GameObject p3;
	private GameObject p4;

    private GameObject goal;
	private GameObject spaceMine;
	private GameObject movingAsteroid;

private GameObject[] PlayerControl = new GameObject[4];
	private Color bSpawnSpot = new Color32(255,0,0,255);
	private Color ySpawnSpot = new Color32(255,235,0,255);
	private Color aSpawnSpot = new Color32(0,255,0,255);
	private Color xSpawnSpot = new Color32(0,0,255,255);
	private Color goalSpot = new Color32(255,0,255,255);
	private Color asteroidSpot = new Color32(0,0,0,255);
	private Color mineSpot = new Color32(0,255,255,255);
	//private Color gateSpot = new Color32 (150, 90, 0, 255);
	private Color bounce = new Color32 (50, 0, 120, 255);
	private Color gateVert = new Color32 (150, 90, 0, 255);
	private Color gateHorz = new Color32 (150, 0, 90, 255);

	private List<GameObject> mines = new List<GameObject> ();
	private int mineCount = 0;

	private List<GameObject> bouncePads = new List<GameObject> ();
	private int bounceCount = 0;

	private List<GameObject> gates = new List<GameObject> ();
	private int gateCount = 0;

	private List<GameObject> asteroids = new List<GameObject>();
	private List<MeshRenderer> asteroidRenders = new List<MeshRenderer>();

	//public StudioEventEmitter music;
	//[HideInInspector]
	public Text text;//spawn text
	//[HideInInspector]
	public GameObject lights;
	//[HideInInspector]
	public GameObject red;
	//[HideInInspector]
	public GameObject orange;
	//[HideInInspector]
	public GameObject yellow;
	//[HideInInspector]
	public GameObject green;
	public Sprite redLit;
	public Sprite redDim;
	public Sprite orangeDim;
	public Sprite orangeLit;
	public Sprite yellowDim;
	public Sprite yellowLit;
	public Sprite greenDim;
	public Sprite greenLit;
	[Tooltip("Set time delay for level preview")]public float startTimer;

	[Tooltip("Pathfinding prefab, used for level testing")] public GameObject finder;

	private SpriteRenderer myOrange;
	private SpriteRenderer myYellow;
	private SpriteRenderer myGreen;
	private SpriteRenderer myRed;
	private float timer = 6f;
	private GameObject player;
	private GameObject options;
	private bool gameStart;
	[HideInInspector] public bool playing = false;
	public Object[] levels;
	[HideInInspector]public int levelNum;


	void Awake(){
		levelNum = GameOptions.randomNumber;
		levels = Resources.LoadAll ("Art/2D/Level Layouts/Spring 2017/Becker");
		PlayerControl[0] = GameObject.Find ("Player1");
		PlayerControl[1] = GameObject.Find ("Player2");
		PlayerControl[2] = GameObject.Find ("Player3");
		PlayerControl[3] = GameObject.Find ("Player4");
		xSpawn = Resources.Load("Prefabs/Spawn_X") as GameObject;
		bSpawn = Resources.Load("Prefabs/Spawn_B") as GameObject;
		ySpawn = Resources.Load("Prefabs/Spawn_Y") as GameObject;
		aSpawn = Resources.Load("Prefabs/Spawn_A") as GameObject;
		goal = Resources.Load("Prefabs/Goal") as GameObject;
		spaceMine = Resources.Load("Prefabs/Bomb_Asteroid") as GameObject;
		bouncePad = Resources.Load("Prefabs/Bounce") as GameObject;
		gate = Resources.Load ("Prefabs/Electric_Gate") as GameObject;

	}

	IEnumerator Levels(){
		Debug.Log ("levels");
		//levels = new Texture2D[GameOptions.maxLevels];
		Debug.Log ("build array");
		yield return new WaitForSeconds(3);
		levels = Resources.LoadAll ("Art/2D/Level Layouts/Spring 2017/Becker");
		Debug.Log ("load levels");
	}
    // Use this for initialization
    void Start () {
        //FMOD
        //Start of level ambiance
        if (SceneManager.GetActiveScene().name == "Arena")
        {
            map = (Texture2D)levels[levelNum];
        }
		p2 = GameObject.Find("Player2");
		p3 = GameObject.Find ("Player3");
		p4 = GameObject.Find ("Player4");
		playing = false;
		lights.SetActive (false);
		gameStart = false;
		text.text = "";
        myRed = red.GetComponent<SpriteRenderer>();
		myOrange = orange.GetComponent<SpriteRenderer>();
		myYellow = yellow.GetComponent<SpriteRenderer>();
		myGreen = green.GetComponent<SpriteRenderer>();
		if (!GameOptions.player2) {
			p2.SetActive (false);
		}
		if (!GameOptions.player3) {
			p3.SetActive (false);
		}
		if (!GameOptions.player4) {
			p4.SetActive (false);
		}
		generateArray = true;
    }


	IEnumerator GameStart(){
		yield return new WaitForSeconds(startTimer);
		foreach(MeshRenderer rend in asteroidRenders){
			rend.enabled = false;
			//Debug.Log("rend off");
		}
		text.text = "Choose Your Launch Pad!";

		//GameObject.Find("Goal").transform.position = GameObject.Find ("Goal(Clone)").transform.position;
		//Destroy(GameObject.Find ("Goal(Clone)"));
		PlayerControl[0].GetComponent <PlayerController> ().playerReady = false;


		if (GameOptions.player2) {
			PlayerControl[1].GetComponent <PlayerController> ().playerReady = false;
		} else {
			PlayerControl[1].GetComponent <PlayerController> ().playerReady = true;
		}
		if (GameOptions.player3) {
			PlayerControl[2].GetComponent <PlayerController> ().playerReady = false;
		} else {
			PlayerControl[2].GetComponent <PlayerController> ().playerReady = true;
		}
		if (GameOptions.player4) {
			PlayerControl[3].GetComponent <PlayerController> ().playerReady = false;
		} else {
			PlayerControl[3].GetComponent <PlayerController> ().playerReady = true;
		}
		gameStart = true;

		gameStart = true;

    }

	void GetRenders(){
		foreach(GameObject go in asteroids){
			MeshRenderer render = go.GetComponent<MeshRenderer>();
			asteroidRenders.Add(render);
		}
		StartCoroutine("GameStart");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if(generateArray == true)
        {
            int xSize = map.width;
            int ySize = map.height;
			//Debug.Log (xSize);
			//Debug.Log (ySize);

			colMap = new int[xSize, ySize];
			//Color32[] pix = map.GetPixels32 ();
           // Vector2[] mapObjPos = new Vector2[xSize * ySize];
           // int[] mapObjs = new int[xSize * ySize];

            for (int horrizontalPixels = 0; horrizontalPixels < xSize; horrizontalPixels++)
            {
                for (int verticalPixels = 0; verticalPixels < ySize; verticalPixels++)
                {
                    if (map.GetPixel(horrizontalPixels, verticalPixels) == bSpawnSpot)
                    {
                        colMap[horrizontalPixels, verticalPixels] = 0;
                        SpawnControl.S.getB(horrizontalPixels, verticalPixels);
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;

                        Instantiate(bSpawn, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.Euler(90, 0, 0));
                    }
                    else if (map.GetPixel(horrizontalPixels, verticalPixels) == xSpawnSpot)
                    {
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;
                        colMap[horrizontalPixels, verticalPixels] = 0;
                        SpawnControl.S.getX(horrizontalPixels, verticalPixels);

                        Instantiate(xSpawn, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.Euler(90, 0, 0));
                    }
                    else if (map.GetPixel(horrizontalPixels, verticalPixels) == aSpawnSpot)
                    {
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;
                        colMap[horrizontalPixels, verticalPixels] = 0;
                        SpawnControl.S.getA(horrizontalPixels, verticalPixels);

                        Instantiate(aSpawn, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.Euler(-90, 0, 0));
                    }
                    else if (map.GetPixel(horrizontalPixels, verticalPixels) == ySpawnSpot)
                    {
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;
                        colMap[horrizontalPixels, verticalPixels] = 0;
                        SpawnControl.S.getY(horrizontalPixels, verticalPixels);

                        Instantiate(ySpawn, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.Euler(90, 0, 0));
                    }
                    else if (map.GetPixel(horrizontalPixels, verticalPixels) == goalSpot)
                    {
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;
                        colMap[horrizontalPixels, verticalPixels] = 100;
                        Instantiate(goal, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.identity);
						GameObject.Find("Goal").transform.position = GameObject.Find ("Goal(Clone)").transform.position;
						Destroy(GameObject.Find ("Goal(Clone)"));
                    }
                    else if (map.GetPixel(horrizontalPixels, verticalPixels) == asteroidSpot)
                    {
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;
                        colMap[horrizontalPixels, verticalPixels] = 200;
                        GameObject sa;
                        sa = Instantiate(smallAsteroid, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.identity) as GameObject;
                        asteroids.Add(sa);
                    }
                    else if (map.GetPixel(horrizontalPixels, verticalPixels) == Color.clear)
                    {
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;
                        //wGameObject la;
                        //la = Instantiate(bouncePad, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.identity) as GameObject;
                        //asteroids.Add(la);
                        colMap[horrizontalPixels, verticalPixels] = 0;
                        //Debug.Log("ppooo");
                    }
					else if (map.GetPixel(horrizontalPixels, verticalPixels) == mineSpot && GameOptions.Instance.minesEnabled == true)
                    {
                        //  mapObjs[horrizontalPixels * ySize + verticalPixels] = 1;
                        colMap[horrizontalPixels, verticalPixels] = 300 + mineCount;
                        GameObject sm;
                        sm = Instantiate(spaceMine, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.identity) as GameObject;
                        asteroids.Add(sm);
                        mines.Add(sm);
                        Bomb_Asteroid tmp = sm.GetComponent<Bomb_Asteroid>();
                        tmp.GetListing(mineCount, horrizontalPixels, verticalPixels, gameObject);
                        mineCount++;
                    }
                    else if (map.GetPixel(horrizontalPixels, verticalPixels) == bounce)
                    {
                        colMap[horrizontalPixels, verticalPixels] = 400 + bounceCount;
                        GameObject sm;
						sm = Instantiate(bouncePad, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.identity) as GameObject;
                        asteroids.Add(sm);
                        bouncePads.Add(sm);
                        BouncePad tmp = sm.GetComponent<BouncePad>();
                        tmp.GetListing(bounceCount, horrizontalPixels, verticalPixels, gameObject);
                        bounceCount++;
					} else if (map.GetPixel(horrizontalPixels, verticalPixels) == gateVert && GameOptions.Instance.gatesEnabled == true)
				{
					colMap[horrizontalPixels, verticalPixels] = 500 + gateCount;
					GameObject sm;
					sm = Instantiate(gate, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.identity) as GameObject;
					//asteroids.Add(sm);
					gates.Add(sm);
					Gate tmp = sm.GetComponentInChildren<Gate>();
					tmp.GetListing(gateCount, horrizontalPixels, verticalPixels, gameObject);
					gateCount++;
					}else if (map.GetPixel(horrizontalPixels, verticalPixels) == gateHorz && GameOptions.Instance.gatesEnabled == true)
				{
					colMap[horrizontalPixels, verticalPixels] = 500 + gateCount;
					GameObject sm;
					sm = Instantiate(gate, new Vector3(horrizontalPixels, 0, verticalPixels), Quaternion.Euler(0,90,0)) as GameObject;
					//asteroids.Add(sm);
					gates.Add(sm);
					Gate tmp = sm.GetComponentInChildren<Gate>();
					tmp.GetListing(gateCount, horrizontalPixels, verticalPixels, gameObject);
					gateCount++;
					
					}else if (horrizontalPixels == 0 || horrizontalPixels == map.width - 1 || verticalPixels == 0 || verticalPixels == map.height - 1) {
						colMap [horrizontalPixels, verticalPixels] = 000;
					} else {
						colMap [horrizontalPixels, verticalPixels] = 0;
					}
	
					//Debug.Log(map.GetPixel(horrizontalPixels, verticalPixels));
                
                    // mapObjPos[horrizontalPixels * ySize + verticalPixels] = new Vector2(horrizontalPixels, verticalPixels);
                }
            }



            generateArray = false;

            //Eww GameObject.Find
            B = GameObject.Find("Spawn_B(Clone)");
            X = GameObject.Find("Spawn_X(Clone)");
            Y = GameObject.Find("Spawn_Y(Clone)");
            A = GameObject.Find("Spawn_A(Clone)");
            
            GetRenders();
        }//end map generation
		else {
			if (Input.GetKeyUp ("x")) {
				//Debug.Log ("keyup");
				showPaths (SpawnControl.S.giveX(), 2);
			}
			if (Input.GetKeyUp ("y")) {
				//Debug.Log ("keyup");
				showPaths (SpawnControl.S.giveY(), 3);
			}
			if (Input.GetKeyUp ("a")) {
				//Debug.Log ("keyup");
				showPaths (SpawnControl.S.giveA(), 0);
			}
			if (Input.GetKeyUp ("b")) {
				//Debug.Log ("keyup");
				showPaths (SpawnControl.S.giveB(), 1);
			}

		}

		if (!playing) {
			if (gameStart) {
				if (PlayerControl[0].GetComponent<PlayerController>().playerReady && PlayerControl[1].GetComponent<PlayerController>().playerReady && PlayerControl[2].GetComponent<PlayerController>().playerReady && PlayerControl[3].GetComponent<PlayerController>().playerReady) {
					text.text = "";
					lights.SetActive (true);
					timer -= Time.deltaTime;
					myRed.sprite = redLit;
					if (timer < 3) {
						myRed.sprite = redDim;
						myOrange.sprite = orangeLit;
					}
					if (timer < 2) {
						myOrange.sprite = orangeDim;
						myYellow.sprite = yellowLit;
					}
					if (timer < 1) {
						myYellow.sprite = yellowDim;
						myGreen.sprite = greenLit;
					}
					if (timer < 0) {
						//FMOD
						//Start for game music
						PlayerControl [0].GetComponent<PlayerController> ().inMenu = false;
						if (GameOptions.player2)

							PlayerControl [1].GetComponent<PlayerController> ().inMenu = false;
                        if (GameOptions.player3)

							PlayerControl [2].GetComponent<PlayerController> ().inMenu = false;
                        if (GameOptions.player4)

							PlayerControl [3].GetComponent<PlayerController> ().inMenu = false;
						/*render = GetComponentsInChildren<MeshRenderer> ();
					foreach (MeshRenderer rend in render) {
						rend.enabled = false;
					}*/
						myGreen.sprite = greenDim;
						lights.SetActive (false);
                        A.GetComponent<SpriteRenderer>().enabled = false;
                        B.GetComponent<SpriteRenderer>().enabled = false;
                        Y.GetComponent<SpriteRenderer>().enabled = false;
                        X.GetComponent<SpriteRenderer>().enabled = false;
                        //Debug.Log("game start");
                        foreach (MeshRenderer rend in asteroidRenders){
							rend.enabled = true;
						}
						playing = true;
					}
				}
			}
		}
	
	}//end Update

	void showPaths(int[] location, int spawnId){
		int x = location [0];
		int z = location [1];

		Vector3 pos = new Vector3 (x, 0, z);
		GameObject tmp = Instantiate (finder, pos, gameObject.transform.rotation) as GameObject;
		PathFinder pf = tmp.GetComponent<PathFinder> ();
		pf.GetMap (colMap, map.width, map.height, spawnId);
	}
	public int getPos(int x, int y){
		return colMap [x, y];
	}

	public void BlowMine(int index){
		Bomb_Asteroid b = mines [index].GetComponent<Bomb_Asteroid> ();
		b.Explode ();
	}

    public int hitPad(int index)
    {
		int lastDir;
        BouncePad b = bouncePads[index].GetComponent<BouncePad>();
		lastDir = b.GetDirection ();
        return lastDir;
    }

	public bool hitGate(int index){
		bool on;
		Gate g = gates [index].GetComponentInChildren<Gate> ();
		on = g.On;
		return on;
	}

	public int getWidth(){
		return map.width;
	}

	public int getHeight(){
		return map.height;
	}
}
