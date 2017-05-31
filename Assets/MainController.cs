﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

  public static bool DEBUG = false;

  private Controller faceController;
  private Block[,] blocks;
  private Shape currentShape;
  private BlockGameObject[] blockGameObjects;
  private float size = 1.1f;
  private float xOffset = -6;
  private float yOffset = 22;



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

	void Start () {
    faceController = GameObject.Find("Controller")
        .GetComponent<Controller>();

    if(DEBUG) {
      faceController.FaceEnter(
          new Controller.FaceParams(0, 0, 0, 0).ToString());
    }

    this.blocks = createBlocks(mapDef);

    initView(this.blocks);

    int index = Random.Range(0, shapeDefs.Length);
    currentShape = new Shape(
        shapeDefs[index], 6, 2, this.blocks, shapeColors[index]);

    StartCoroutine(step());
	}

  private IEnumerator step() {
    while(!isGameOver()) {
      if(currentShape.canMove(0, 1)) {
        currentShape.move(0, 1);
      } else {
        scan();

        int index = Random.Range(0, shapeDefs.Length);
        currentShape = new Shape(
            shapeDefs[index], 6, 2, this.blocks, shapeColors[index]);
      }

      currentShape.updateModel();
      updateBlockObjects();

      yield return new WaitForSeconds(0.2f);
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

    return blocks;
  }

  private void scan() {
    int numRow = blocks.GetLength(0);
    int numCol = blocks.GetLength(1);

    for(int r=numRow-2; r>=0;) {

      int count = 0;

      for(int c=1; c<numCol-1; c++) {

        if(blocks[r, c].state != ' ') {
          count++;
        } else {
          break;
        }
      }

      if(count == (numCol-2)) {
        eliminate(r);
      } else {
        r--;
      }
    }

  }

  private void eliminate(int row) {
    int numCol = blocks.GetLength(1);

    for(int r=row; r>=1; r--) {
      for(int c=1; c<numCol-1; c++) {
        blocks[r, c].state = blocks[r-1, c].state;
      }
    }

    for(int c=1; c<numCol-1; c++) {
      blocks[0, c].state = ' ';
    }
  }

  private void initView(Block[,] blocks) {
    int numRow = blocks.GetLength(0);
    int numCol = blocks.GetLength(1);

    blockGameObjects = new BlockGameObject[numRow * numCol];
    int index = 0;

    for(int r=0; r<numRow; r++) {
      for(int c=0; c<numCol; c++) {
        GameObject t = GameObject.CreatePrimitive(PrimitiveType.Cube);

        t.transform.position = new Vector3(
            size * c + xOffset, - size * r + yOffset, 0); // simple imp

        // t.transform.rotation = Quaternion.Euler(0, 0, 10);

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

      faceController.FaceMove(
          new Controller.FaceParams(0, x, y, 0).ToString());
    }

    bool hasInput = false;

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
    var face = faceController.getFacePosition(0);
    int faceX = (int) Mathf.Round((face.x - xOffset) / size);
    if(faceX < 1) {
      faceX = 1; 
    }
    if (faceX >= blocks.GetLength(1)-1) {
      faceX = blocks.GetLength(1)-1;
    }
    // Debug.Log(faceX);

    while(currentShape.x > faceX && currentShape.canMove(-1, 0)) {
      currentShape.move(-1, 0);
    }
    while(currentShape.x < faceX && currentShape.canMove(1, 0)) {
      currentShape.move(1, 0);
    }

    facePositionCache.Add(face);
    if(facePositionCache.Count > facePositionCacheLimit) {
      facePositionCache.RemoveAt(0);
    }

    if(face.y - facePositionCache[0].y > 1) {
      currentShape.rotate();
    }

    if(hasInput) {
      currentShape.updateModel();
      updateBlockObjects();
    }
	}

  int facePositionCacheLimit = 5;
  List<Vector3> facePositionCache = new List<Vector3>();

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

