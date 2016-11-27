using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseBehaviour : MonoBehaviour {

    public Vector2 m_CameraPos = new Vector2(0.0f, 0.0f);

    public GameObject Grass;
    public GameObject Water;
    public GameObject PathPrefab;

    public enum MouseState { INACTIVE, ACTIVE }; // Inactive = nothing, Active = placing an object
    public enum TerrainType { NOTHING, SELL, GRASS, WATER, PATH }; //object being placed

    public List<GameObject> m_FirstLayer = new List<GameObject>();
    public List<GameObject> m_SecondLayer = new List<GameObject>();

    public List<GameObject> m_Cursors = new List<GameObject>();
    public List<GameObject> m_Sprites = new List<GameObject>();

    public Sprite[] m_CurrentSprite; // 0 = nothing, 1 = grass, 2 = water

    public Vector3 MousePosition;

    public MouseState m_MouseState;
    public TerrainType m_TerrainType;

    public int m_iCursorSize = 1;
    int m_iDeletingLayer = 0;
    TerrainType m_TerrainLock = TerrainType.NOTHING;

    public int m_iTerrainChanges = 0;

    // Use this for initialization
    void Start () {
        MousePosition = new Vector3(0.0f, 0.0f, 0.0f);

        m_MouseState = MouseState.INACTIVE;
        m_TerrainType = TerrainType.NOTHING;
        //GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[0];
        for (int i = 1; i < 5; ++ i)
        {
            m_Cursors[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        GetInput();

        if (m_MouseState == MouseState.INACTIVE)
        {
            GetComponent<Transform>().position = new Vector3(-999.0f, 999.0f, GetComponent<Transform>().position.z);
        }
        else
        {
            Vector3 MousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Check new mouse position
            MousePosition.x = Mathf.Floor(MousePos3D.x + 0.5f);
            MousePosition.y = Mathf.Floor(MousePos3D.y + 0.5f);

            GetComponent<Transform>().position = new Vector3(MousePosition.x, MousePosition.y, GetComponent<Transform>().position.z);

            //Pick shape here
            

            for (int i = 0; i < m_iCursorSize; ++i)
            {
                if (MousePosition.y + i <= m_CameraPos.y + 17.0f)
                {
                    for (int j = 0; j < m_iCursorSize; ++j)
                    {
                        if (MousePosition.x + j <= m_CameraPos.x + 31.0f)
                        {
                            HandleLeftClick(MousePosition.x + j, MousePosition.y + i);
                        }
                    }
                }              
            }
        }   
    }


    void GetInput()
    {
        //Enable mouse hover Select Terrain
        if (Input.GetKeyDown("s")) //Keydown 1
        {
            m_MouseState = MouseState.ACTIVE;
            m_TerrainType = TerrainType.SELL;
            ChangeSprites(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) //Keydown 1
        {
            m_MouseState = MouseState.ACTIVE;
            m_TerrainType = TerrainType.GRASS;
            ChangeSprites(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //Keydown 2
        {
            m_MouseState = MouseState.ACTIVE;
            m_TerrainType = TerrainType.WATER;
            ChangeSprites(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) //Keydown 2
        {
            m_MouseState = MouseState.ACTIVE;
            m_TerrainType = TerrainType.PATH;
            ChangeSprites(3);
        }
        //Disable mouse hover / Deselect terrain
        else if (Input.GetKeyDown(KeyCode.Escape)) //Keydown escape
        {
            m_MouseState = MouseState.INACTIVE;
            m_TerrainType = TerrainType.NOTHING;          
        }
        else if (Input.GetMouseButton(1)) //Mousedown right click
        {
            m_MouseState = MouseState.INACTIVE;
            m_TerrainType = TerrainType.NOTHING;         
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && m_iCursorSize != 5 && m_MouseState == MouseState.ACTIVE) //Up arrow, increase cursor size
        {
            m_iCursorSize++;
            m_Cursors[m_iCursorSize - 1].SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && m_iCursorSize != 1 && m_MouseState == MouseState.ACTIVE) //Down arrow, decrease cursor size
        {
            m_Cursors[m_iCursorSize - 1].SetActive(false);
            m_iCursorSize--;
        }
    }

    void ChangeSprites(int _iSprite)
    {
        for (int i = 0; i < m_Sprites.Count; ++i)
        {
            m_Sprites[i].GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[_iSprite];
        }
    }

    void HandleLeftClick(float _fx, float _fy)
    {
        if (Input.GetMouseButton(0)) //Mousedown left click
        {           
            //Check for grass
            bool _bGrassExists = false;
            int _iGrassIndex = -1;

            //Check for 2nd layer
            bool _bSecondLayerExists = false;
            int _iSecondLayerIndex = -1;
            TerrainType _SecondLayerTerrain = TerrainType.NOTHING;

            //Check for grass existance
            for (int i = 0; i < m_FirstLayer.Count; ++i)
            {
                if (m_FirstLayer[i].GetComponent<Transform>().position.x == _fx && m_FirstLayer[i].GetComponent<Transform>().position.y == _fy)
                {
                    _bGrassExists = true;
                    _iGrassIndex = i;
                    break;
                }
            }
            //If grass exists, what is on top of it
            if (_bGrassExists)
            {
                for (int i = 0; i < m_SecondLayer.Count; ++i)
                {
                    if (m_SecondLayer[i].GetComponent<Transform>().position.x == _fx && m_SecondLayer[i].GetComponent<Transform>().position.y == _fy)
                    {
                        _bSecondLayerExists = true;
                        _iSecondLayerIndex = i;
                        _SecondLayerTerrain = ReturnTerrainType(_iSecondLayerIndex);
                        break;
                    }
                }
            }
            else //If grass doesnt exist, and grass is selected. Place grass
            {
                if (m_TerrainType == TerrainType.GRASS)
                {
                    CreateTerrain(_fx, _fy);
                }
            }

            //Placing a tile on top of grass
            if (_bGrassExists)
            {
                if (m_TerrainType == TerrainType.WATER && !_bSecondLayerExists) //If placing water on grass
                {
                    CreateTerrain(_fx, _fy);
                }
                else if (m_TerrainType == TerrainType.PATH && !_bSecondLayerExists) //If placing water on grass
                {
                    CreateTerrain(_fx, _fy);
                }
                else if (m_TerrainType == TerrainType.SELL && _bSecondLayerExists && (m_iDeletingLayer == 1 || m_iDeletingLayer == 0)) //If selling second layer
                {
                    m_iDeletingLayer = 1;
                    if (m_TerrainLock == TerrainType.NOTHING)
                    {
                        m_TerrainLock = _SecondLayerTerrain;
                    }
                    if (m_TerrainLock == _SecondLayerTerrain)
                    {
                        DeleteSecondLayer(_iSecondLayerIndex);
                    }                    
                }
                else if (m_TerrainType == TerrainType.SELL && !_bSecondLayerExists && (m_iDeletingLayer == 2 || m_iDeletingLayer == 0)) //If selling grass
                {
                    m_iDeletingLayer = 2;
                    DeleteGrassLayer(_iGrassIndex);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_iDeletingLayer = 0;
            m_TerrainLock = TerrainType.NOTHING;
        }
    }


    void CreateTerrain(float _fx, float _fy) //Create a block of terrain at cursor position = to the current mouse state
    {
        m_iTerrainChanges++;
        //Initialise variables for the instantiated object
        GameObject NewTerrain;
        Quaternion Rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        if (m_TerrainType == TerrainType.GRASS)
        {
            Vector3 Position = new Vector3(_fx, _fy, Grass.GetComponent<Transform>().position.z);
            NewTerrain = (GameObject)Instantiate(Grass, Position, Rotation);
            m_FirstLayer.Add(NewTerrain);
        }
        else if (m_TerrainType == TerrainType.WATER)
        {
            Vector3 Position = new Vector3(_fx, _fy, Water.GetComponent<Transform>().position.z);
            NewTerrain = (GameObject)Instantiate(Water, Position, Rotation);
            m_SecondLayer.Add(NewTerrain);
        }
        else if (m_TerrainType == TerrainType.PATH)
        {
            Vector3 Position = new Vector3(_fx, _fy, PathPrefab.GetComponent<Transform>().position.z);
            NewTerrain = (GameObject)Instantiate(PathPrefab, Position, Rotation);
            m_SecondLayer.Add(NewTerrain);
        }
    }

    void DeleteGrassLayer(int _iIndex) //Delete the block below the mouse
    {
        m_iTerrainChanges--;
        GameObject.Destroy(m_FirstLayer[_iIndex]);
        m_FirstLayer.RemoveAt(_iIndex);
    }

    void DeleteSecondLayer(int _iIndex) //Delete the block below the mouse
    {
        m_iTerrainChanges--;
        GameObject.Destroy(m_SecondLayer[_iIndex]);
        m_SecondLayer.RemoveAt(_iIndex);
    }

    TerrainType ReturnTerrainType(int _iIndex)
    {
        TerrainType _TempTerrain = TerrainType.NOTHING;

        if (m_SecondLayer[_iIndex].tag == "Water")
        {
            _TempTerrain = TerrainType.WATER;
        }
        else if (m_SecondLayer[_iIndex].tag == "Path")
        {
            _TempTerrain = TerrainType.PATH;
        }

        return _TempTerrain;
    }
}
