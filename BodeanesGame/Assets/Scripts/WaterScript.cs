using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterScript : MonoBehaviour {

    public GameObject _Object;

    public List<GameObject> m_Waters = new List<GameObject>();

    public Sprite[] m_SpriteList;
    int _iLastInt;

    public bool m_bAbove;
    public bool m_bBelow;
    public bool m_bLeft;
    public bool m_bRight;

    public bool m_bTopLeft;
    public bool m_bTopRight;
    public bool m_bBotLeft;
    public bool m_bBotRight;

    // Use this for initialization
    void Start()
    {
        _Object = GameObject.FindGameObjectWithTag("GameManager");
        UpdateSprite();
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
        m_bTopLeft = false;
        m_bTopRight = false;
        m_bBotLeft = false;
        m_bBotRight = false;

        List<GameObject> SecondLayer = _Object.GetComponent<MouseBehaviour>().m_SecondLayer;


        for (int i = 0; i < SecondLayer.Count; ++i)
        {
            if (SecondLayer[i].tag == "Water")
            {
                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y + 1)
                {
                    m_bAbove = true;
                }
                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y - 1)
                {
                    m_bBelow = true;
                }
                if (SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y && SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x - 1)
                {
                    m_bLeft = true;
                }
                if (SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y && SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x + 1)
                {
                    m_bRight = true;
                }

                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x - 1 && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y + 1)
                {
                    
                    if (m_bAbove && m_bLeft)
                    {
                        Debug.Log("TopLeft");
                        m_bTopLeft = true;
                    }
                }
                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x + 1 && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y + 1)
                {
                   
                    if (m_bAbove && m_bRight)
                    {
                        Debug.Log("TopRight");
                        m_bTopRight = true;
                    }
                }
                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x - 1 && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y - 1)
                {
                    
                    if (m_bBelow && m_bLeft)
                    {
                        Debug.Log("BotLeft");
                        m_bBotLeft = true;
                    }
                }
                if (SecondLayer[i].GetComponent<Transform>().position.x == GetComponent<Transform>().position.x + 1 && SecondLayer[i].GetComponent<Transform>().position.y == GetComponent<Transform>().position.y - 1)
                {
                    
                    if (m_bBelow && m_bRight)
                    {
                        Debug.Log("BotRight");
                        m_bBotRight = true;
                    }
                }


                if (m_bAbove) m_Waters[0].SetActive(true);
                else m_Waters[0].SetActive(false);

                if (m_bBelow) m_Waters[1].SetActive(true);
                else m_Waters[1].SetActive(false);

                if (m_bLeft) m_Waters[2].SetActive(true);
                else m_Waters[2].SetActive(false);

                if (m_bRight) m_Waters[3].SetActive(true);
                else m_Waters[3].SetActive(false);

                if (m_bTopLeft) m_Waters[4].SetActive(true);
                else m_Waters[4].SetActive(false);

                if (m_bTopRight) m_Waters[5].SetActive(true);
                else m_Waters[5].SetActive(false);

                if (m_bBotLeft) m_Waters[6].SetActive(true);
                else m_Waters[6].SetActive(false);

                if (m_bBotRight) m_Waters[7].SetActive(true);
                else m_Waters[7].SetActive(false);

            }
        }


        


    }
}
