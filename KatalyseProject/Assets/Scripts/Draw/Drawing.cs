using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public float fBrushSize;
    public GameObject goBrush;

    private List<GameObject> lgoDraw;


    private void Start()
    {
        lgoDraw = new List<GameObject>();
        GameManager.getInstance().dDrawing = this;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(Ray, out hit))
            {
                if (hit.transform.tag == "AreaForDraw")
                {
                    GameObject go = Instantiate(goBrush, hit.point + Vector3.up * 0.1f, Quaternion.AngleAxis(90, Vector3.right), transform);
                    lgoDraw.Add(go);
                    go.transform.localScale = Vector3.one * fBrushSize;
                }
            }
        }
    }

    public void DeleteDraw()
    {
        print("Delete all");
        foreach (var item in lgoDraw)
        {
            Destroy(item);
        }
    }
}
