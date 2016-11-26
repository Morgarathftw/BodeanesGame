using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseBehaviour : MonoBehaviour {

    public GameObject Grass;
    public GameObject Water;

    public enum MouseState { INACTIVE, ACTIVE }; // Inactive = nothing, Active = placing an object
    public enum TerrainType { NOTHING, GRASS, WATER }; //object being placed

    public int WorldHeight = 18;
    public int WorldWidth = 32;

    public struct WorldInfo
    {
        public TerrainType _TerrainType;
        public Vector2 _TerrainPosition;
    }

    public List<WorldInfo> m_WorldInfo = new List<WorldInfo>();

    public Sprite[] m_CurrentSprite; // 0 = nothing, 1 = grass, 2 = water

    public Vector2 LastPosition;
    public Vector2 MousePosition;

    public MouseState m_MouseState;
    public TerrainType m_TerrainType;

    // Use this for initialization
    void Start () {
        MousePosition = new Vector2(0.0f, 0.0f);
        LastPosition = new Vector2(0.0f, 0.0f);

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

            if (MousePosition != LastPosition) //If this mouse grid position is different from the last
            {
                LastPosition = MousePosition; //Set position
                GetComponent<Transform>().position = new Vector3(LastPosition.x, LastPosition.y, GetComponent<Transform>().position.z);
                Debug.Log("x: " + LastPosition.x + "  y: " + LastPosition.y);
            }
            HandleLeftClick();
        }   
    }


    void GetInput()
    {
        //Enabke mouse hover Select Terrain
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
        if (Input.GetMouseButton(0)) //Mousedown right click
        {
            int _iCounter = 0;
            //int _iIndex = -1;
            Debug.Log("Size of array = " + m_WorldInfo.Count);
            for (int i = 0; i < m_WorldInfo.Count; ++i)
            {
                Debug.Log("Array index = " + i);
                if (m_WorldInfo[i]._TerrainPosition == LastPosition)
                {
                    Debug.Log("Position exists");
                    _iCounter += 1;
                    break;
                }
            }

            if (_iCounter == 0)
            {
                Debug.Log("Making new info");
                WorldInfo _tempInfo;
                _tempInfo._TerrainPosition = LastPosition;
                _tempInfo._TerrainType = m_TerrainType;
                m_WorldInfo.Add(_tempInfo);

                if (m_TerrainType == TerrainType.GRASS)
                {
                    GameObject NewTerrain = (GameObject)Instantiate(Grass);
                    NewTerrain.transform.position = new Vector3(LastPosition.x, LastPosition.y, 0.0f);
                }
                else if (m_TerrainType == TerrainType.WATER)
                {
                    GameObject NewTerrain = (GameObject)Instantiate(Water);
                    NewTerrain.transform.position = new Vector3(LastPosition.x, LastPosition.y, 0.0f);
                }          
               
                Debug.Log("Info pushed");
            }

            Debug.Log("Leaving struct");
        }
    }

}
