﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Controller : MonoBehaviour { // TODO: rename

  public struct FaceParams {
    public int id;
    public float x;
    public float y;
    public float r;

    public FaceParams(int id, float x, float y, float r) {
      this.id = id;
      this.x = x;
      this.y = y;
      this.r = r;
    }

    public FaceParams(string args) {
      string[] xy = args.Split (';');
      id = int.Parse (xy [0]);
      if (xy.Length == 4) {
        x = float.Parse (xy [1]);
        y = float.Parse (xy [2]);
        r = float.Parse (xy [3]);
      } else {
        x = 0;
        y = 0;
        r = 0;
      }
    }

    public string ToString() {
      return string.Format("{0};{1};{2};{3}", id, x, y, r);
    }
  }

  private Dictionary<int, GameObject> faces = new Dictionary<int, GameObject>();
  private Color[] colors = { Color.red, Color.green, Color.blue };
  private int colorIndex = 0;

  public void FaceEnter(string args) {
    var f = new FaceParams(args);
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.transform.position = new Vector3 (-(f.x - 0.5f) * 20, -(f.y - 0.5f) * 20, 0);
    cube.transform.rotation = Quaternion.AngleAxis (-f.r, Vector3.forward);
    cube.GetComponent<Renderer>().material.color = colors[colorIndex];
    colorIndex = (colorIndex+1) % colors.Length;
    faces.Add(f.id, cube);
  }

  public void FaceMove(string args) {
    var f = new FaceParams(args);

    if(faces.ContainsKey(f.id)) {
      GameObject cube = faces[f.id];
      cube.transform.position = new Vector3 (-(f.x - 0.5f) * 20, -(f.y - 0.5f) * 20, 0);
      cube.transform.rotation = Quaternion.AngleAxis (-f.r, Vector3.forward);
    }

  }

  public void FaceExit(string args) {
    var f = new FaceParams(args);
    if(faces.ContainsKey(f.id)) {
      GameObject cube = faces[f.id];
      GameObject.Destroy(cube);
      faces.Remove(f.id);
    }
  }
}
