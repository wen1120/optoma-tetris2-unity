using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

  public static bool DEBUG = true;

  private Controller faceController;
  private Block[,] blocks;
  Shape currentShape;

  string[] mapDef = {
    "w          w", // spawn
    "w          w", // spawn
    "w          w", // spawn
    "w          w", // spawn
    "w          w", // 20
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w", 
    "w          w", // 10
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w",
    "w          w", // 1
    "wwwwwwwwwwww",
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

  public static string[][] L1 = {
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

  public static string[][] L2 = {
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

  public static string[][] Z1 = {
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
  };

  public static string[][] Z2 = {
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

  public static string[][] S = {
    new string[] {
      "oo",
      "ox",
    }
  };

	void Start () {
    faceController = GameObject.Find("Controller")
        .GetComponent<Controller>();

    if(DEBUG) {
      faceController.FaceEnter(new Controller.FaceParams(0, 0, 0, 0).ToString());
    }

    this.blocks = createBlocks(mapDef);

    initView(this.blocks, 1.1f);

    currentShape = new Shape(T, 3, 3, this.blocks);

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

    return blocks;
  }

  BlockGameObject[] blockGameObjects;

  private void initView(Block[,] blocks, float size) {
    int numRow = blocks.GetLength(0);
    int numCol = blocks.GetLength(1);
    float xOffset = -6;
    float yOffset = 22;

    blockGameObjects = new BlockGameObject[numRow * numCol];
    int index = 0;

    for(int r=0; r<numRow; r++) {
      for(int c=0; c<numCol; c++) {
        GameObject t = GameObject.CreatePrimitive(PrimitiveType.Cube);

        t.transform.position = new Vector3(
            size * c + xOffset, - size * r + yOffset, 0); // simple imp

        BlockGameObject bgo = t.AddComponent<BlockGameObject>();
        bgo.block = this.blocks[r, c];

        blockGameObjects[index++] = bgo;
      }
    }

  }
	
	void Update () {
    // Debug.Log(Input.mousePosition);

    if(DEBUG) {
      float x = (Screen.width - Input.mousePosition.x) / Screen.width;
      float y = (Screen.height - Input.mousePosition.y) / Screen.height;
      // Debug.Log(string.Format("{0}, {1}", x, y));

      faceController.FaceMove(new Controller.FaceParams(0, x, y, 0).ToString());
    }

    if (Input.GetKeyDown(KeyCode.UpArrow)) {
      currentShape.rotate();
    } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
      currentShape.move(0, 1);
    } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
      currentShape.move(-1, 0);
    } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
      currentShape.move(1, 0);
    }

    currentShape.updateModel();

    // for(int i=0; i<blocks.GetLength(0); i++) {
    //   for(int j=0; j<blocks.GetLength(1); j++) {
    //     Debug.Log(blocks[i, j].state);
    //   }
    // }

    updateBlockObjects();
	}

  private void updateBlockObjects() {
    for(int i=0; i<blockGameObjects.Length; i++) {
      blockGameObjects[i].updateFromModel();
    }
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

