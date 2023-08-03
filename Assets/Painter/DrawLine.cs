using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using common;

public class DrawLine : MonoBehaviour
{
	[HideInInspector]
	public LineRenderer line;
	public bool isMousePressed;
	public List<Vector3> pointsList;
	private Vector3 mousePos;

    GameObject painter_obj;
    GameObject paint_group;
	// Structure for line points
	struct myLine
	{
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};
	//    -----------------------------------    
	void Awake ()
	{
		// Create line renderer component and set its property
		line = gameObject.AddComponent<LineRenderer> ();
        
		//line.material = new Material (Shader.Find ("Particles/Additive"));
        line.material = new Material(Resources.Load("Shader222") as Shader);
        
        //GameObject.FindGameObjectWithTag("check").GetComponent<UnityEngine.UI.Text>().text = "first";
        //Material pMaterial = Resources.Load("PaintMaterial") as Material;
        //line.material.color = Utils.draw_color;
        //line.material = pMaterial;
		line.SetVertexCount (0);
		line.SetWidth (0.1f, 0.1f);
		line.SetColors (Color.green, Color.green);

		line.useWorldSpace = true;    
		isMousePressed = false;
		pointsList = new List<Vector3> ();
        painter_obj = (GameObject)Resources.Load("Prefab/Game_Component", typeof(GameObject));
        paint_group = GameObject.FindGameObjectWithTag("paint_group");
        Debug.Log(painter_obj.name);
        
	}
	//    -----------------------------------    
	void Update ()
	{
        Debug.Log(Utils.can_draw);
        if (Utils.can_draw)
        {

            // If mouse button down, remove old line and set its color to green
            if (Input.GetMouseButtonDown(0) && !Utils.slider_check && !Utils.btn_clicked)
            {
                isMousePressed = true;
                line.SetColors(Utils.draw_color, Utils.draw_color);
                line.material.SetColor("_Color",Utils.draw_color);
                /*line.SetVertexCount (0);
                pointsList.RemoveRange (0, pointsList.Count);
                line.SetColors (Color.green, Color.green);*/
                //Debug.Log("mouse clicked");
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMousePressed = false;
                this.GetComponent<DrawLine>().enabled = false;
                GameObject inst_painter = Instantiate(painter_obj).gameObject;
                inst_painter.transform.SetParent(paint_group.transform);
                if (Utils.slider_check)
                {
                    Debug.Log("slider lost actions");
                    Utils.slider_check = false;
                }
            }
            // Drawing line when mouse is moving(presses)
            if (isMousePressed)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                if (!pointsList.Contains(mousePos))
                {
                    //Debug.Log("mousePos " + mousePos);
                    pointsList.Add(mousePos);
                    line.SetVertexCount(pointsList.Count);
                    line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
                    //if (isLineCollide ()) {
                    //isMousePressed = false;
                    //line.SetColors (Color.red, Color.red);
                    //}
                }
                Utils.can_apply = true;
            }
        }
	}
	//    -----------------------------------    
	// Following method checks is currentLine(line drawn by last two points) collided with line
	//    -----------------------------------    
	private bool isLineCollide ()
	{
		if (pointsList.Count < 2)
			return false;
		int TotalLines = pointsList.Count - 1;
		myLine[] lines = new myLine[TotalLines];
		if (TotalLines > 1) {
			for (int i=0; i<TotalLines; i++) {
				lines [i].StartPoint = (Vector3)pointsList [i];
				lines [i].EndPoint = (Vector3)pointsList [i + 1];
			}
		}
		for (int i=0; i<TotalLines-1; i++) {
			myLine currentLine;
			currentLine.StartPoint = (Vector3)pointsList [pointsList.Count - 2];
			currentLine.EndPoint = (Vector3)pointsList [pointsList.Count - 1];
			if (isLinesIntersect (lines [i], currentLine))
				return true;
		}
		return false;
	}
	//    -----------------------------------    
	//    Following method checks whether given two points are same or not
	//    -----------------------------------    
	private bool checkPoints (Vector3 pointA, Vector3 pointB)
	{
		return (pointA.x == pointB.x && pointA.y == pointB.y);
	}
	//    -----------------------------------    
	//    Following method checks whether given two line intersect or not
	//    -----------------------------------    
	private bool isLinesIntersect (myLine L1, myLine L2)
	{
		if (checkPoints (L1.StartPoint, L2.StartPoint) ||
		    checkPoints (L1.StartPoint, L2.EndPoint) ||
		    checkPoints (L1.EndPoint, L2.StartPoint) ||
		    checkPoints (L1.EndPoint, L2.EndPoint))
			return false;
		
		return((Mathf.Max (L1.StartPoint.x, L1.EndPoint.x) >= Mathf.Min (L2.StartPoint.x, L2.EndPoint.x)) &&
		       (Mathf.Max (L2.StartPoint.x, L2.EndPoint.x) >= Mathf.Min (L1.StartPoint.x, L1.EndPoint.x)) &&
		       (Mathf.Max (L1.StartPoint.y, L1.EndPoint.y) >= Mathf.Min (L2.StartPoint.y, L2.EndPoint.y)) &&
		       (Mathf.Max (L2.StartPoint.y, L2.EndPoint.y) >= Mathf.Min (L1.StartPoint.y, L1.EndPoint.y))
		       );
	}
}
//- See more at: http://www.theappguruz.com/blog/draw-line-mouse-move-detect-line-collision-unity2d-unity3d#sthash.QtKnvgCn.dpuf