using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGameObject : MonoBehaviour {
  public Block block;

  public BlockGameObject(Block b) {
    this.block = b;
  }

  public void updateFromModel() {
    gameObject.SetActive(true);
    switch(block.state) {
      case 'r':
        setColor(Color.red);
        break;
      case 'm':
        setColor(Color.magenta);
        break;
      case 'y':
        setColor(Color.yellow);
        break;
      case 'c':
        setColor(Color.cyan);
        break;
      case 'b':
        setColor(Color.blue);
        break;
      case 's':
        setColor(Color.gray);
        break;
      case 'l':
        setColor(Color.green);
        break;
      case 'w':
        setColor(Color.white);
        break;
      case ' ':
        gameObject.SetActive(false);
        break;
      default:
        setColor(Color.red);
        break;
    }
  }

  private void setColor(Color color) {
    GetComponent<Renderer>().material.color = color;
  }
}
