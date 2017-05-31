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
      color = Color.gray;
    } else if(block.state == 'r') {
      color = Color.black;
    } else if(block.state == 'w') {
      color = Color.black;
    } else {
      color = Color.black;
    }
    GetComponent<Renderer>().material.color = color;
  }
}
