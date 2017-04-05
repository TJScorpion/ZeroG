using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {
	// array holding GridMap's collision map
	int[,] map;
	// collision map width and height
	int width;
	int height;


	// position of the spawn point assigned to this pathfinder
	int xPos;
	int zPos;

	int castCount = 0;
	bool goalHit = false;

	// positions and directions of the valid paths are saved and in a stack
	// and then popped off later to draw them

	// Directions
	List<int> pathToDraw;
	// X and Z positions
	List<int> posToDraw;
	// List holding every place the pathfinder has found so far
	//  used to check if we have been somewhere so we dont have infinite loops
	List<int> placesIveSeen;

	// index for the stack of directions to draw in
	int drawingDir;
	// index for the stack of positions to draw the indexes at
	int drawingPos;



	// square object used to mark the path
	GameObject point;

	// different points for each spawn point
	[Tooltip("Square for marking path from spawnpoint A")]
	public GameObject pointA;
	[Tooltip("Square for marking path from spawnpoint B")]
	public GameObject pointB;
	[Tooltip("Square for marking path from spawnpoint X")]
	public GameObject pointX;
	[Tooltip("Square for marking path from spawnpoint Y")]
	public GameObject pointY;
	// used to hold the position for the point to be spawned
	Vector3 spawnPos;

	bool cast(int dir, int xP, int zP){
		bool one = true;
		bool two = true;
		int check = 1;
		castCount++;
		if (castCount < 100) {
			
		//	Debug.Log ("casting at " + xP + ", " + zP);
		//	Debug.Log (dir);

			int x = xP;
			int z = zP;

			if (dir == 0) {
				z++;
			} else if (dir == 1) {
				x--;
			} else if (dir == 2) {
				z--;
			} else if (dir == 3) {
				x++;
			} else {
				check = -1;
				Debug.Log ("Error wrong direction");
			}

			int c = 0;
			while (check > 0 && c < 100) {

				if (x < width && x >= 0 && z < height && z >= 0) {
					
					if (map [x, z] == 0) {
						
						if (dir == 0) {
							z++;
						} else if (dir == 1) {
							x--;
						} else if (dir == 2) {
							z--;
						} else if (dir == 3) {
							x++;
						}
					} else if (map [x, z] == 200) {
						//Debug.Log (Mathf.Abs (x - xP));
						//Debug.Log (Mathf.Abs (z - zP));

						if (Mathf.Abs (x - xP) < 2 && Mathf.Abs (z - zP) < 2) {
							
							check = -1;
								
						} else {
							
							check = -2;
							if (dir == 0) {
								z--;
							} else if (dir == 1) {
								x++;
							} else if (dir == 2) {
								z++;
							} else if (dir == 3) {
								x--;
							}
						}

					} else if (map [x, z] == 100) {
						
						check = -3;
						one = true;
						two = true;
						goalHit = true;
						//Debug.Log ("gooooal");

					} else {
						
						check = -1;
						//Debug.Log ("what is this " + map [x, z]); 
					}
				} else {
					check = -1;
				}
					
			}

			if (check == -2) {
				
				bool found = false;

				for (int i = 0; i < placesIveSeen.Count; i+=2) {
					if (placesIveSeen [i] == x) {
						if (placesIveSeen [i + 1] == z) {
							found = true;
							i = placesIveSeen.Count;
							one = false;
							two = false;
						}
					}
				}


				if (!found) {
					placesIveSeen.Add (x);
					placesIveSeen.Add (z);
					if (dir == 0 || dir == 2) {
						one = cast (1, x, z);
						two = cast (3, x, z);
					} else if (dir == 1 || dir == 3) {
						one = cast (0, x, z);
						two = cast (2, x, z);
					}

				}
			} else if (check > -2) {
				one = false;
				two = false;

			}
				

			if (one == false && two == false) {
				return false;
			} else {
				pathToDraw.Add (dir);
				posToDraw.Add (xP);
				posToDraw.Add (zP);
				return true;

			}
		} else {
			return false;
		}




	}

	void drawCast(int dir, int xP, int zP){
			int check = 1;
			int x = xP;
			int z = zP;
		if (dir == 0) {
			z++;
		} else if (dir == 1) {
			x--;
		} else if (dir == 2) {
			z--;
		} else if (dir == 3) {
			x++;
		} else {
			check = -1;
			Debug.Log ("Error drawing wrong direction");
		}

		int count = 0;
		while (check > 0 && count < 100) {
				if (x < width && x > 0 && z < height && z > 0) {
					if (map [x, z] == 0) {
						spawnPos = new Vector3 (x, 0, z);
						Instantiate (point, spawnPos, gameObject.transform.rotation);
						if (dir == 0) {
							z++;
						} else if (dir == 1) {
							x--;
						} else if (dir == 2) {
							z--;
						} else if (dir == 3) {
							x++;
						}
					} else if (map [x, z] == 200) {
						check = -2;
						if (dir == 0) {
							z--;
						} else if (dir == 1) {
							x++;
						} else if (dir == 2) {
							z++;
						} else if (dir == 3) {
							x--;
						}
					} else if (map [x, z] == 100) {
						//done = true;
					if (dir == 0) {
						z--;
					} else if (dir == 1) {
						x++;
					} else if (dir == 2) {
						z++;
					} else if (dir == 3) {
						x--;
					}
						check = -1;
					} else {
						check = -1;
					}
				} else {
					check = -1;
				}
			}
		drawingDir--;
		drawingPos -= 2;
			if (drawingPos > 0) {

				drawCast (pathToDraw [drawingDir], posToDraw [drawingPos - 1], posToDraw [drawingPos]);
			}
	}


	/*
	 * 
	 * Function called by GridMap.cs
	 * Passes all of the data needed for the pathFinder
	 * 
	 * colMap - copy of GridMap's 2D array used for collision detection
	 * mWidth - width of collsionMap
	 * mHeight - height of collsionMap
	 * spawnId - used to tell pathfinder which color path tp use for which type of spawn point
	 * 
	 */

	public void GetMap(int[,] colMap, int mWidth, int mHeight, int spawnId){

		string id;
		// ID of spawn point
		if (spawnId == 0) {
			point = pointA;
			id = "A";
		} else if (spawnId == 1) {
			point = pointB;
			id = "B";
		} else if (spawnId == 2) {
			point = pointX;
			id = "X";
		} else {
			point = pointY;
			id = "Y";
		}

		// collision map's variables
		map = colMap;
		width = mWidth;
		height = mHeight;
		xPos = (int)transform.position.x;
		zPos = (int)transform.position.z;

		// Initializing Lists
		pathToDraw = new List<int> ();
		posToDraw = new List<int> ();
		placesIveSeen = new List<int> ();

		/* Casting a check in every direction
		 * 
		 * 0 - North
		 * 1 - West
		 * 2 - South
		 * 3 - East
		 * 
		 * */

		cast (0, xPos, zPos);
		castCount = 0;

		castCount = 0;
		cast (1, xPos, zPos);

		castCount = 0;
		cast (2, xPos, zPos);

		castCount = 0;
		cast (3, xPos, zPos);

		// Checking for valid path and then drawing the path
		if (pathToDraw.Count > 0 && goalHit) {
			drawingDir = pathToDraw.Count - 1;
			drawingPos = posToDraw.Count - 1;
			drawCast (pathToDraw [drawingDir], posToDraw[drawingPos -1], posToDraw[drawingPos]);
		} else {
			Debug.LogError ("ERROR NO PATH ON " + id );
		}
			
	}
		
}
