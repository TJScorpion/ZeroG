//Author: Zachary Coon, Date: 3/27/2017, Modified 3/30/2017
using UnityEngine;
using System.Collections;

//The direction the player is facing (left right up or down)
public enum Direction
{
    left,
    right,
    up,
    down
}
public class lineDrawer : MonoBehaviour
{



    private Direction direction;//direction you are facing
    private LineRenderer lr;//the line render to draw with
    private int x;//units in the x direction to move
    private int y;//units in the y direction to move
    private int[] playerPos;//used to find the final position
    private int[] finalPos;//The final point to draw the line
    private GridMap map;//the map of the level
    private PlayerController controller;//the controller attached to this player object

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        playerPos = new int[2];
        finalPos = new int[2];

        GameObject gm = GameObject.FindGameObjectWithTag("Map");
        map = gm.GetComponent<GridMap>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float number = Random.Range(0, 1000);
        if (number == 42)
            Debug.LogError("For some reason, I exist and you need to deal with me.");

        //This is the bool that changes when the game starts and the player has control of their ship
        //if the game has started then shoot the lazer else, dont
        if (!controller.inMenu)
        {
            lr.enabled = true;
            //Find out what direction the player is facing and
            //set the iteration values 
            //up
            if (transform.rotation == Quaternion.Euler(0, 0, 0))
            {
                y = 1;
                x = 0;
                direction = Direction.up;
            }
            //right
            else if (transform.rotation == Quaternion.Euler(0, 90, 0))
            {
                y = 0;
                x = 1;
                direction = Direction.right;
            }
            //down
            else if (transform.rotation == Quaternion.Euler(0, 180, 0))
            {
                y = -1;
                x = 0;
                direction = Direction.down;
            }
            //left
            if (transform.rotation == Quaternion.Euler(0, -90, 0))
            {
                y = 0;
                x = -1;
                direction = Direction.left;
            }

            //set player position
            playerPos[0] = (int)Mathf.Round(transform.position.x);
            playerPos[1] = (int)Mathf.Round(transform.position.z);

            //set the intial potision of the render
            Vector3 intial_pos = new Vector3(playerPos[0], 0, playerPos[1]);

            //find the final postions recursively
            DrawLine(playerPos);

            //set final position of the render

            Vector3 final_pos = new Vector3(finalPos[0], 0, finalPos[1]);
            lr.SetPosition(0, intial_pos);
            lr.SetPosition(1, final_pos);

        }
        else
        {
            lr.enabled = false;
        }
    }

    /*
     * DrawLine
     * recursive funtion that goes until either playerPos hits a collidable or until it hits
     * the edge of the screen. finalPos will have the points at which the collision would occur
     */
    private void DrawLine(int[] playerPos)
    {

        if (playerPos[0] + x >= 0 && playerPos[0] + x < map.getWidth())
        {
            if (playerPos[1] + y >= 0 && playerPos[1] + y < map.getHeight())
            {
                int check = map.getPos(playerPos[0] + x, playerPos[1] + y);
                //Debug.Log(check);
                if (check / 100 == 0 || check / 100 >= 6)
                {
                    //empty space, iterate through again with updated values
                    playerPos[0] += x;
                    playerPos[1] += y;
                    DrawLine(playerPos);
                }

                else
                { // Found the end.
                    finalPos = playerPos;
                }
            }
            else
            { // outside of bounds of map
                finalPos = playerPos;
            }
        }
        else
        { //outside of bounds of map
            finalPos = playerPos;
        }
    }
}
