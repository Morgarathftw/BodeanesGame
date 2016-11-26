using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseBehaviour : MonoBehaviour {

    public GameObject Grass;
    public GameObject Water;

    public enum MouseState { INACTIVE, ACTIVE }; // Inactive = nothing, Active = placing an object
    public enum TerrainType { NOTHING, SELL, GRASS, WATER }; //object being placed

    public List<GameObject> m_WorldInfo = new List<GameObject>();

    public Sprite[] m_CurrentSprite; // 0 = nothing, 1 = grass, 2 = water

    public Vector3 MousePosition;

    public MouseState m_MouseState;
    public TerrainType m_TerrainType;

    // Use this for initialization
    void Start () {
        MousePosition = new Vector3(0.0f, 0.0f, 0.0f);
        MousePosition = new Vector3(0.0f, 0.0f, 0.0f);

        m_MouseState = MouseState.INACTIVE;
        m_TerrainType = TerrainType.NOTHING;
        GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[0];
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
            HandleLeftClick();
        }   
    }


    void GetInput()
    {
        //Enable mouse hover Select Terrain
        if (Input.GetKeyDown("s")) //Keydown 1
        {
            m_MouseState = MouseState.ACTIVE;
            m_TerrainType = TerrainType.SELL;
            GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) //Keydown 1
        {
            m_MouseState = MouseState.ACTIVE;
            m_TerrainType = TerrainType.GRASS;
            GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //Keydown 2
        {
            m_MouseState = MouseState.ACTIVE;
            m_TerrainType = TerrainType.WATER;
            GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[2];
        }
        //Disable mouse hover / Deselect terrain
        else if (Input.GetKeyDown(KeyCode.Escape)) //Keydown escape
        {
            m_MouseState = MouseState.INACTIVE;
            m_TerrainType = TerrainType.NOTHING;
            GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[0];
        }
        else if (Input.GetMouseButton(1)) //Mousedown right click
        {
            m_MouseState = MouseState.INACTIVE;
            m_TerrainType = TerrainType.NOTHING;
            GetComponent<SpriteRenderer>().sprite = m_CurrentSprite[0];
        }
    }

    void HandleLeftClick()
    {
        if (Input.GetMouseButton(0)) //Mousedown left click
        {
            bool _bExistingTile = false;
            int _iIndex = -1;
            for (int i = 0; i < m_WorldInfo.Count; ++i)
            {
                if (m_WorldInfo[i].GetComponent<Transform>().position == MousePosition)
                {
                    _bExistingTile = true;
                    _iIndex = i;
                    break;
                }
            }

            if (_bExistingTile == false)
            {
                if (m_TerrainType == TerrainType.GRASS)
                {
                    CreateTerrain();
                }
                else if (m_TerrainType == TerrainType.WATER)
                {
                    CreateTerrain();
                }          
            }
            else
            {
                if (m_WorldInfo[_iIndex].tag == "Grass" && m_TerrainType == TerrainType.WATER)
                {
                    DeleteTerrain(_iIndex);
                    CreateTerrain();
                }
                else if (m_WorldInfo[_iIndex].tag == "Water" && m_TerrainType == TerrainType.GRASS)
                {
                    DeleteTerrain(_iIndex);
                    CreateTerrain();
                }
                else if (m_TerrainType == TerrainType.SELL)
                {
                    DeleteTerrain(_iIndex);
                }
            }
        }
    }


    void CreateTerrain() //Create a block of terrain at cursor position = to the current mouse state
    {
        //Initialise variables for the instantiated object
        GameObject NewTerrain;
        Vector3 Position = new Vector3(MousePosition.x, MousePosition.y, 0.0f);
        Quaternion Rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        if (m_TerrainType == TerrainType.GRASS)
        {
            NewTerrain = (GameObject)Instantiate(Grass, Position, Rotation);
            m_WorldInfo.Add(NewTerrain);
        }
        else if (m_TerrainType == TerrainType.WATER)
        {
            NewTerrain = (GameObject)Instantiate(Water, Position, Rotation);
            m_WorldInfo.Add(NewTerrain);
        }     
    }

    void DeleteTerrain(int _iIndex) //Delete the block below the mouse
    {
        GameObject.Destroy(m_WorldInfo[_iIndex]);
        m_WorldInfo.RemoveAt(_iIndex);
    }
}
