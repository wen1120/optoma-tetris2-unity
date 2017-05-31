using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGameObject : MonoBehaviour {
  public Block block;

  public BlockGameObject(Block b) {
    this.block = b;
  }

  public void updateFromModel() {
    Color color;

    if(block.state == ' ') {
      color = Color.black;
    } else if(block.state == 'r') {
      color = Color.red;
    } else if(block.state == 'm') {
      color = Color.magenta;
    } else if(block.state == 'y') {
      color = Color.yellow;
    } else if(block.state == 'c') {
      color = Color.cyan;
    } else if(block.state == 'b') {
      color = Color.blue;
    } else if(block.state == 's') {
      color = Color.gray;
    } else if(block.state == 'l') {
      color = Color.green;
    } else if(block.state == 'w') {
      color = Color.white;
    } else {
      color = Color.black;
    }
    GetComponent<Renderer>().material.color = color;
  }
}
