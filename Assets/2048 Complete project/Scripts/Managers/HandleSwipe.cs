using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HandleSwipe : MonoBehaviour
{
    public GameObject test;
	// Update is called once per frame
	void Update () {
        
	    Vector2 curPos = test.transform.position;
	    
	  
        if (SwipeManager.IsSwipingDown())
        {
            curPos += Vector2.down;
        }
        else if (SwipeManager.IsSwipingUp())        
        {
            curPos += Vector2.up;
            
        }
        else if (SwipeManager.IsSwipingRight())
        {
            curPos += Vector2.right;
        }
        else if (SwipeManager.IsSwipingLeft())
        {
            curPos += Vector2.left;
            
        }
        else if (SwipeManager.IsSwipingDownLeft())
        {
            curPos += Vector2.left;
            curPos += Vector2.down;
            
        }
        else if (SwipeManager.IsSwipingDownRight())
        {
            curPos += Vector2.right;
            curPos += Vector2.down;
        }
        else if (SwipeManager.IsSwipingUpLeft())
        {
            curPos += Vector2.left;
            curPos += Vector2.up;
        }
        else if (SwipeManager.IsSwipingUpRight())
        {
            curPos += Vector2.right;
            curPos += Vector2.up;
        }
	    test.transform.position = curPos;
    }

}
