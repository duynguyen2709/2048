using UnityEngine;

public enum MoveDirection
{
    Right ,
    Left ,
    Up ,
    Down ,
    UpLeft ,
    UpRight ,
    DownLeft ,
    DownRight
}

public class InputManager : MonoBehaviour {

	private GameManager gameManager;

	void Awake(){
		gameManager = GameObject.FindObjectOfType<GameManager>();
	}

	// Update is called once per frame
	void Update () {

		if(gameManager.state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.D)){
				gameManager.Move(MoveDirection.Right);
				gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
			}
			else if(Input.GetKeyDown(KeyCode.A)){
				gameManager.Move(MoveDirection.Left);
				gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
			}
			else if(Input.GetKeyDown(KeyCode.W)){
				gameManager.Move(MoveDirection.Up);
				gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
			}
			else if(Input.GetKeyDown(KeyCode.X)){
				gameManager.Move(MoveDirection.Down);
				gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
			}
            else if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.Move(MoveDirection.UpLeft);
                gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                gameManager.Move(MoveDirection.UpRight);
                gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                gameManager.Move(MoveDirection.DownLeft);
                gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                gameManager.Move(MoveDirection.DownRight);
                gameManager.PlaySound(gameManager.soundManager.mergeTile, 1.0f);
            }
        }
	}
}
