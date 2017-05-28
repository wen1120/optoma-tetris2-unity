using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Controller : MonoBehaviour { // TODO: rename

  public struct FaceParams {
    public int id;
    public float x;
    public float y;
    public float r;

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
  }

  Dictionary<int, GameObject> faces = new Dictionary<int, GameObject>();

  void FaceEnter(string args) {
    var f = new FaceParams(args);
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.transform.position = new Vector3 (-(f.x - 0.5f) * 20, -(f.y - 0.5f) * 20, 0);
    cube.transform.rotation = Quaternion.AngleAxis (-f.r, Vector3.forward);
    faces.Add(f.id, cube);
  }

  void FaceMove(string args) {
    var f = new FaceParams(args);

    if(faces.ContainsKey(f.id)) {
      GameObject cube = faces[f.id];
      cube.transform.position = new Vector3 (-(f.x - 0.5f) * 20, -(f.y - 0.5f) * 20, 0);
      cube.transform.rotation = Quaternion.AngleAxis (-f.r, Vector3.forward);
    }

  }

  void FaceExit(string args) {
    var f = new FaceParams(args);
    if(faces.ContainsKey(f.id)) {
      GameObject cube = faces[f.id];
      GameObject.Destroy(cube);
      faces.Remove(f.id);
    }
  }
}
