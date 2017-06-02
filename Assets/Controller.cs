using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour { // TODO: rename

  public struct Face {
    public static readonly Face DUMMY = new Face(-1, 0, 0, 0, 0);
    public readonly int id;
    public float x;
    public float y;
    public float r;
    public float w;

    public Face(int id, float x, float y, float r, float w) {
      this.id = id;
      this.x = x;
      this.y = y;
      this.r = r;
      this.w = w;
    }

    public Face(string args) {
      string[] vals = args.Split (';');
      this.id = int.Parse (vals[0]);
      if(vals.Length > 1) {
        this.x = float.Parse (vals[1]);
        this.y = float.Parse (vals[2]);
        this.r = float.Parse (vals[3]);
        this.w = float.Parse (vals[4]);
      } else {
        this.x = 0;
        this.y = 0;
        this.r = 0;
        this.w = 0;
      }
    }

    public void copy(Face that) {
      this.x = that.x;
      this.y = that.y;
      this.r = that.r;
      this.w = that.w;
    }
    public static bool operator ==(Face x, Face y) {
      return x.id == y.id && x.x == y.x && x.y == y.y && 
          x.r == y.r && x.w == y.w;
    }

    public static bool operator !=(Face x, Face y) {
      return !(x == y);
    }

    public override string ToString() {
      return string.Format("{0};{1};{2};{3};{4}", id, x, y, r, w);
    }
  }

  public Dictionary<int, Face> faces = 
      new Dictionary<int, Face>();

  public void FaceEnter(string args) {
    var f = new Face(args);
    faces.Add(f.id, f);
  }

  public void FaceMove(string args) {
    var f = new Face(args);

    if(faces.ContainsKey(f.id)) {
      faces[f.id] = f;
    } else {
      // what?
    }

  }

  public void FaceExit(string args) {
    var f = new Face(args);
    if(faces.ContainsKey(f.id)) {
      faces.Remove(f.id);
    }
  }

  public Dictionary<int, Face> getFaces() {
    return faces;
  }

}
