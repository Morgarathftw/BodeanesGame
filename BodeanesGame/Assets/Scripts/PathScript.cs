using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathScript : MonoBehaviour
{

    GameObject _Object;
    public Sprite[] m_SpriteList;
    int _iLastInt;

    bool m_bAbove;
    bool m_bBelow;
    bool m_bLeft;
    bool m_bRight;

    // Use this for initialization
    void Start()
    {
        _Object = GameObject.FindGameObjectWithTag("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (_Object.GetComponent<MouseBehaviour>().m_iTerrainChanges != _iLastInt)
        {
            _iLastInt = _Object.GetComponent<MouseBehaviour>().m_iTerrainChanges;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        m_bAbove = false;
        m_bBelow = false;
        m_bLeft = false;
        m_bRight = false;

        List<GameObject> SecondLayer = _Object.GetComponent<MouseBehaviour>().m_SecondLayer;


        for (int i = 0; i < SecondLayer.Count; ++i)
        {
            if (SecondLayer[i].tag == "Path")
            {
                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y + 1)
                {
                    m_bAbove = true;
                    Debug.Log("Above");
                }
                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y - 1)
                {
                    m_bBelow = true;
                    Debug.Log("Below");
                }
                if (SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y && SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x - 1)
                {
                    m_bLeft = true;
                    Debug.Log("Left");
                }
                if (SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y && SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x + 1)
                {
                    m_bRight = true;
                    Debug.Log("Right");
                }
            }
        }

        if (m_bAbove && m_bBelow && m_bLeft && m_bRight) //Crossroads
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[0];
        }
        else if (m_bAbove && m_bBelow && m_bLeft && !m_bRight) //3 right
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[1];
        }
        else if (m_bAbove && m_bBelow && !m_bLeft && m_bRight) //3 left
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[2];
        }
        else if (m_bAbove && !m_bBelow && m_bLeft && m_bRight) //3 below
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[3];
        }
        else if (!m_bAbove && m_bBelow && m_bLeft && m_bRight) //3 up
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[4];
        }
        else if (m_bAbove && m_bBelow && !m_bLeft && !m_bRight) // vert
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[5];
        }
        else if (m_bAbove && !m_bBelow && m_bLeft && !m_bRight) // bot right
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[6];
        }
        else if (!m_bAbove && m_bBelow && m_bLeft && !m_bRight) // top right
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[7];
        }
        else if (m_bAbove && !m_bBelow && !m_bLeft && m_bRight) // bot left
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[8];
        }
        else if (!m_bAbove && m_bBelow && !m_bLeft && m_bRight) // top left
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[9];
        }
        else if (!m_bAbove && !m_bBelow && m_bLeft && m_bRight) // horiz
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[10];
        }
        else if (m_bAbove && !m_bBelow && !m_bLeft && !m_bRight) //up
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[11];
        }
        else if (!m_bAbove && m_bBelow && !m_bLeft && !m_bRight) //down
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[12];
        }
        else if (!m_bAbove && !m_bBelow && m_bLeft && !m_bRight) //left
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[13];
        }
        else if (!m_bAbove && !m_bBelow && !m_bLeft && m_bRight) //right
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[14];
        }
        else if (!m_bAbove && !m_bBelow && !m_bLeft && !m_bRight) //middle
        {
            GetComponent<SpriteRenderer>().sprite = m_SpriteList[15];
        }
    }
}
