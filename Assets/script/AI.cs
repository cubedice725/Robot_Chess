using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AI : MonoBehaviour
{
    // odb: object Detection Box
    public List<GameObject> odbList;
    public List<List<Vector3>> check2DList;

    private int m_width;
    private int m_length;

    public int Width { get { return m_width; } set { m_width = value; } }
    public int Length{ get { return m_length; } set { m_length = value; } }


    public void Setodb(int odbSetCount)
    {
        GameObject odbParent = GameObject.Find("Object Detection Box");
        GameObject odbPrefab = Resources.Load("Prefab/Object Detection Box", typeof(GameObject)) as GameObject;

        for (int i = 0; i < odbSetCount; i++)
        {
            odbList.Add(Instantiate(odbPrefab, odbParent.transform));
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
    }
    public void CheckBox()
    {
        for (int i = 0; i < m_width * m_length; i++)
        {
            print(odbList[i].GetComponent<ObjectDetectionBox>().Box);
        }
        for (int i = 0; i < odbList.Count; i++)
        {
            odbList[i].transform.position = new Vector3(0, -100, 0);
            odbList[i].SetActive(false);
        }
    }
    public void SetCheckBox()
    {
        for (int i = 0; i < m_width * m_length; i++)
        {
            odbList[i].transform.position = new Vector3(i / m_length, 1, i % m_length);
            odbList[i].SetActive(true);
        }
    }
}
