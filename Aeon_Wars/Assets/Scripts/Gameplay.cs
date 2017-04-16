using UnityEngine;
using System.Collections;

public class Gameplay : MonoBehaviour {
    public Ship defaultShip;
    public ShipUI playerUI;
    public Menu GameOverMenu;
    private Ship shipchoice;
    private EnemySpawner enemySpawner;

    private Ship player;

    private void Awake()
    {
        player = null;
        shipchoice = defaultShip;
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.EnemySpawned += playerUI.OnEnemySpawned;
        GameOverMenu.setDisplay(false);//should not be displayed
    }

    public void SelectShip(Ship shipObject)
    {
        shipchoice = shipObject;
    }

    public void StartGame()
    {
        player = (Ship)Instantiate(shipchoice, transform.position, Quaternion.identity);
        player.ShipDestroyed += OnShipDestroyed;
        playerUI.Register(player);
        enemySpawner.Register(player);
        TurnOffCursor();
    }

    private void OnShipDestroyed(Ship shipobject)
    {
        shipobject.ShipDestroyed -= OnShipDestroyed;
        Invoke("GameOver", 3.0f);
    }

    private void GameOver()
    {
        GameOverMenu.setDisplay(true);
        TurnOnCursor();
    }

    public void SetKeyboard()
    {
        firebutton = "Submit";
        movementx = "Horizontal";
        movementy = "Vertical";
        usingmouse = false;
    }

    public void SetMouse()
    {
        firebutton = "Fire1";
        movementx = "Mouse X";
        movementy = "Mouse Y";
        usingmouse = true;
    }

    public static void TurnOnCursor()
    {
        if(usingmouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public static void TurnOffCursor()
    {
        if(usingmouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public static string FireButton { get { return firebutton; } }
    public static string MovementX { get { return movementx; } }
    public static string MovementY{ get { return movementy; } }
    private static string firebutton = "Submit";
    private static string movementx = "Horizontal";
    private static string movementy = "Vertical";
    private static bool usingmouse = false;

}
