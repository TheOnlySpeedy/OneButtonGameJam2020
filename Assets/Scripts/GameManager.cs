using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* Blocks */
    public GameObject ground;
    public GameObject air;
    
    /* Main Assets */
    public GameObject player;
    public TextAsset level;
    private GameObject _world;
    private Camera _mainCamera;
    
    /* Enemies */
    private GameObject _enemies;
    public GameObject smallEnemy;
    
    [SerializeField]
    private Vector3 _north;
    [SerializeField]
    private Vector3 _east;
    [SerializeField]
    private Vector3 _south;
    [SerializeField]
    private Vector3 _west;

    public GameManager(GameObject player)
    {
        this.player = player;
    }

    private void Awake()
    {
        _north = Vector3.forward;
        _east = Vector3.right;
        _south = Vector3.back;
        _west = Vector3.left;
        
        _world = GameObject.Find("World");
        _mainCamera = Camera.main;
        
        _enemies = GameObject.Find("Enemies");
        
        var lines = level.text.Split('\r');

        for (var y = 0; y < lines.Length; y++)
        {
            var cells = lines[y].Split(',');

            for (var x = 0; x < cells.Length; x++)
            {
                var cell = cells[x].ToUpper();
                // skip if empty
                if (cell.Length < 1)
                {
                    continue;
                }
                
                var data = cell.Split('-');
                // first char of first string of data string[]
                var cellObject = data[0][0];

                var posX = x;
                var posY = -y;

                Vector3 lookDir;
                switch (cellObject)
                {
                    case 'E':
                        SpawnGameObject(posX, posY, ground);

                        lookDir = GetLookDir(data[1]);
                        SpawnEnemy(posX, posY, lookDir);
                        break;
                    case 'S':
                        SpawnGameObject(posX, posY, ground);

                        lookDir = GetLookDir(data[1]);
                        SpawnPlayer(posX, posY, lookDir);
                        
                        _mainCamera.GetComponent<CameraMovement>().target = GameObject.Find("Player");
                        break;
                    case 'X':
                        SpawnGameObject(posX, posY, ground);
                        break;
                    default:
                        SpawnGameObject(posX, posY);
                        break;
                }
            }
        }
    }

    private void SpawnGameObject(int x, int y)
    {
        SpawnGameObject(x, y, air);
    }

    private void SpawnGameObject(int x, int y, GameObject block)
    {
        Instantiate(
            block, 
            new Vector3(x, 0, y), 
            transform.rotation,
            _world.transform
        );
    }

    private Vector3 GetLookDir(string direction)
    {
        return direction switch
        {
            "N" => _north,
            "E" => _east,
            "S" => _south,
            "W" => _west,
            _ => _south
        };
    }

    private void SpawnPlayer(int x, int y, Vector3 lookDir)
    {
        var playerObject = Instantiate(player, new Vector3(x,0,y), transform.rotation);
        playerObject.name = "Player";
        playerObject.transform.LookAt(playerObject.transform.position + lookDir);
    }

    private void SpawnEnemy(int x, int y, Vector3 lookDir)
    {
        var enemyObject = Instantiate(
            smallEnemy, 
            new Vector3(x, 0, y), 
            transform.rotation,
            _enemies.transform
        );
        
        enemyObject.transform.LookAt(enemyObject.transform.position + lookDir);
    }
}
