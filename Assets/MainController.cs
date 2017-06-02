using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

  public static bool SIM_FACE = false;

  private Controller faceController;
  private Block[,] blocks;
  private Shape currentShape;
  private BlockGameObject[,] blockGameObjects;
  private float size = 1.1f;
  private float xOffset = -22;
  private float yOffset = 22;
  public bool hasPower = true;
  private int initX = 20;
  private int initY = 3;
  private Position upperLeft, lowerRight;

  string[] mapDef = {
    "______________w          w______________", 
    "______________w          w______________", 
    "______________w          w______________", 
    "______________w          w______________", //_spawn
    "______________w          w______________", 
    "______________w          w______________", 
    "__ddd_ddd_ddd_w          w_ddd_d_d__d___", 
    "__d_d_d_d__d__w          w_d_d_ddd_d_d__", 
    "__d_d_ddd__d__w          w_d_d_d_d_ddd__",
    "__ddd_d____d__w          w_ddd_d_d_d_d__",
    "______________w          w______________",
    "______________w          w______________", 
    "______________w          w______________", 
    "______________w          w______________",
    "______________w          w______________",
    "______________w          w______________", 
    "______________w          w______________", 
    "______________w          w______________", 
    "______________w          w______________",
    "______________w          w______________",
    "______________w          w______________",
    "______________w          w______________", 
    "______________w          w______________", 
    "______________w          w______________",
    "______________wwwwwwwwwwww______________"
  };

  public static string[][] I = {
    new string[] {
      "    ",
      "    ",
      "ooxo",
      "    "
    },
    new string[] {
      "  o ",
      "  o ",
      "  x ",
      "  o "
    }
  };

  public static string[][] L = {
    new string[] {
      " o ",
      " x ",
      " oo"
    },
    new string[] {
      "   ",
      "oxo",
      "o  "
    },
    new string[] {
      "oo ",
      " x ",
      " o "
    },
    new string[] {
      "  o",
      "oxo",
      "   "
    }
  };

  public static string[][] J = {
    new string[] {
      " o ",
      " x ",
      "oo "
    },
    new string[] {
      "o  ",
      "oxo",
      "   "
    },
    new string[] {
      " oo",
      " x ",
      " o "
    },
    new string[] {
      "   ",
      "oxo",
      "  o"
    }
  };

  public static string[][] S = {
    new string[] {
      " oo",
      "ox ",
      "   "
    },
    new string[] {
      " o ",
      " xo",
      "  o"
    },
    new string[] {
      "   ",
      " xo",
      "oo "
    },
    new string[] {
      "o  ",
      "ox ",
      " o "
    },
  };

  public static string[][] Z = {
    new string[] {
      "oo ",
      " xo",
      "   "
    },
    new string[] {
      "  o",
      " xo",
      " o "
    },
    new string[] {
      "   ",
      "ox ",
      " oo"
    },
    new string[] {
      " o ",
      "ox ",
      "o  "
    },
  };

  public static string[][] T = {
    new string[] {
      " o ",
      "oxo",
      "   "
    },
    new string[] {
      " o ",
      " xo",
      " o "
    },
    new string[] {
      "   ",
      "oxo",
      " o "
    },
    new string[] {
      " o ",
      "ox ",
      " o "
    }
  };

  public static string[][] O = {
    new string[] {
      "oo",
      "ox",
    }
  };

  public static string[][][] shapeDefs = {
    I, L, J, S, Z, T, O
  };

  public static char[] shapeColors = {
    'r', 'm', 'y', 'c', 'b', 's', 'l'
  };

  private Texture2D paper1;
  private Texture2D paper2;

	void Start () {
    paper1 = (Texture2D) Resources.Load("paper3");
    paper2 = (Texture2D) Resources.Load("paper4");

    Screen.sleepTimeout = SleepTimeout.NeverSleep;
    
    faceController = GameObject.Find("Controller")
        .GetComponent<Controller>();

    if(SIM_FACE) {
      faceController.FaceEnter(
          new Controller.FaceParams(0, 0, 0, 0).ToString());
    }

    this.blocks = createBlocks(mapDef);

    initBlocks(this.blocks);

    createShape();

    if(hasPower) {
      StartCoroutine(step());
    }
	}

  private void createShape() {
    int index = Random.Range(0, shapeDefs.Length);
    currentShape = new Shape(
        shapeDefs[index], initX, initY, this.blocks, shapeColors[index]);
  }

  private IEnumerator step() {
    while(!isGameOver()) {
      if(currentShape.canMove(0, 1)) {
        currentShape.move(0, 1);
      } else {
        scan();

        createShape();
      }

      currentShape.updateModel();
      updateBlockObjects();

      yield return new WaitForSeconds(0.5f);
    }
    gameOver();
  }

  private bool isGameOver() {
    return false; // TODO
  }
  
  private void gameOver() {
    // TODO
  }

  private Block[,] createBlocks(string[] mapDef) {
    int numRow = mapDef.Length;
    int numCol = mapDef[0].Length; // assume # of row > 0
    var blocks = new Block[numRow, numCol];

    for(int r=0; r<numRow; r++) {
      for(int c=0; c<numCol; c++) {
        blocks[r, c] = new Block(c, r, mapDef[r][c]);
      }
    }

    for(int r=0; r<numRow; r++) {
      for(int c=0; c<numCol; c++) {
        if(mapDef[r][c] == ' ') {
          upperLeft = new Position(c, r);
          goto L1;
        }
      }
    }
L1:
    for(int r=numRow-1; r>=0; r--) {
      for(int c=numCol-1; c>=0; c--) {
        if(mapDef[r][c] == ' ') {
          lowerRight = new Position(c, r);
          goto L2;
        }
      }
    }
L2:
    Debug.Log("upper left: "+upperLeft);
    Debug.Log("lower right: "+lowerRight);
    return blocks;
  }

  private void scan() {
    for(int r=lowerRight.y; r>=upperLeft.y;) {

      int count = 0;

      for(int c=upperLeft.x; c<=lowerRight.x; c++) {
        if(blocks[r, c].state == ' ') {
          r--;
          goto L1;
        } 
      }

      eliminate(r);

L1:   ;

    }

  }

  private void eliminate(int row) {
    Debug.Log("eliminate: "+row);

    for(int r=row; r>upperLeft.y; r--) {
      for(int c=upperLeft.x; c<=lowerRight.x; c++) {
        blocks[r, c].state = blocks[r-1, c].state;
      }
    }

    // the first row
    for(int c=upperLeft.x; c<=lowerRight.x; c++) {
      blocks[upperLeft.y, c].state = ' ';
    }
  }

  private void initBlocks(Block[,] blocks) {
    int numRow = blocks.GetLength(0);
    int numCol = blocks.GetLength(1);

    blockGameObjects = new BlockGameObject[numRow, numCol];
    
    // var wood = (Texture2D)Resources.Load("light_wood");

    for(int r=0; r<numRow; r++) {
      for(int c=0; c<numCol; c++) {
        GameObject p = 
            new GameObject();
            // GameObject.CreatePrimitive(PrimitiveType.Cube); 

        p.transform.position = new Vector3(
            size * c + xOffset + Random.Range(-0.05f, 0.05f), 
            - size * r + yOffset + Random.Range(-0.05f, 0.05f) + 0.5f, 
            0);

        // Rigidbody rb = p.AddComponent<Rigidbody>();
        // rb.AddForce(new Vector3(1, 0, 0));

        GameObject t = GameObject.CreatePrimitive(PrimitiveType.Quad);
        t.transform.parent = p.transform;
        t.transform.localPosition = new Vector3(0, -0.5f, 0);
        t.transform.localScale = new Vector3(1, 1, 1);
        t.GetComponent<Renderer>().material.mainTexture = 
            (Random.Range(0, 2) == 0) ? paper1: paper2;

        p.transform.rotation = Quaternion.Euler(
            Random.Range(0f, 10f), 
            0, 
            Random.Range(-2f, 2f));

        // t.GetComponent<Renderer>().material.mainTexture = wood;

        // GameObject b = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // b.transform.position = new Vector3(
        //     size * c + xOffset, - size * r + yOffset, 1);
        // b.GetComponent<Renderer>().material.mainTexture = wood;

        // t.transform.rotation = Quaternion.Euler(0, 0, 10);

        BlockGameObject bgo = p.AddComponent<BlockGameObject>();
        bgo.block = this.blocks[r, c];

        blockGameObjects[r, c] = bgo;
      }
    }
  }
	
	void Update () {
    // Debug.Log(Input.mousePosition);

    if(SIM_FACE) {
      float x = (Screen.width - Input.mousePosition.x) / Screen.width;
      float y = (Screen.height - Input.mousePosition.y) / Screen.height;
      // Debug.Log(string.Format("{0}, {1}", x, y));

      faceController.FaceMove(
          new Controller.FaceParams(0, x, y, 0).ToString());
    }

    bool hasInput = false;

    //keyboard
    if (Input.GetKeyDown(KeyCode.UpArrow)) {
      currentShape.rotate();
      hasInput = true;
    } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
      while(currentShape.canMove(0, 1)) {
        currentShape.move(0, 1);
      }
      hasInput = true;
    } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      currentShape.move(-1, 0);
      hasInput = true;
    } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
      currentShape.move(1, 0);
      hasInput = true;
    }


    // face
    // var face = faceController.getFace(0);
    // int faceX = 
    //     (int) Mathf.Round((face.transform.position.x - xOffset) / size);
    // if(faceX < 1) {
    //   faceX = 1; 
    // }
    // if (faceX >= blocks.GetLength(1)-1) {
    //   faceX = blocks.GetLength(1)-1;
    // }
    // // Debug.Log(faceX);
    //
    // while(currentShape.x > faceX && currentShape.canMove(-1, 0)) {
    //   currentShape.move(-1, 0);
    //   hasInput = true;
    // }
    // while(currentShape.x < faceX && currentShape.canMove(1, 0)) {
    //   currentShape.move(1, 0);
    //   hasInput = true;
    // }
    //
    // facePositionCache.Add(face.transform.position);
    // if(facePositionCache.Count > facePositionCacheLimit) {
    //   facePositionCache.RemoveAt(0);
    // }

    // // if(face.y - facePositionCache[0].y > 1f) {
    // if(face.transform.rotation.eulerAngles.z > 30) {
    //   if(rotateCooldown <= 0) {
    //     currentShape.rotate();
    //     rotateCooldown = .5f;
    //   } 
    //   hasInput = true;
    // }
    // rotateCooldown -=  Time.deltaTime;

    if(hasInput) {
      currentShape.updateModel();

      updateBlockObjects();
    }
	}

  float rotateCooldown = 0;
  int facePositionCacheLimit = 5;
  List<Vector3> facePositionCache = new List<Vector3>();

  private void updateBlockObjects() {
    int numRow = blocks.GetLength(0);
    int numCol = blocks.GetLength(1);

    for(int r=0; r<numRow; r++) {
      for(int c=0; c<numCol; c++) {
        blockGameObjects[r, c].updateFromModel();
      }
    }

    for(int r=0; r<4; r++) {
      for(int c=0; c<numCol; c++) {
        blockGameObjects[r, c].gameObject.SetActive(false);
      }
    }
  }

  public void reset() {
    int numRow = mapDef.Length;
    int numCol = mapDef[0].Length; // assume # of row > 0

    for(int r=0; r<numRow; r++) {
      for(int c=0; c<numCol; c++) {
        blocks[r, c].state = mapDef[r][c];
      }
    }

    createShape();
  }

}

public struct Position {
  public static readonly Position ZERO = new Position(0, 0);
  public readonly int x;
  public readonly int y;

  public Position(int x, int y) {
    this.x = x;
    this.y = y;
  }
  public Position(Position that) {
    this.x = that.x;
    this.y = that.y;
  }
  public override string ToString() {
    return string.Format("({0}, {1})", this.x, this.y);
  }
}


public class Block {
  public int x;
  public int y;
  public char state;

  public Block(int x, int y, char state) {
    this.x = x;
    this.y = y;
    this.state = state;
  }
}

