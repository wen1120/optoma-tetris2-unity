﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape {

  int x;
  int y;
  Position[][] defs;
  int defIndex = 0;
  Block[,] blocks;

  public Shape(string[][] defs, int x, int y, Block[,] blocks) {
    this.x = x;
    this.y = y;
    this.defs = new Position[defs.Length][];
    for(int i=0; i<defs.Length; i++) {
      this.defs[i] = createDefinition(defs[i]);
    }
    this.blocks = blocks;
  }

  public void rotate() {
    updateModelWith(' ');
    int oldIndex = defIndex;
    defIndex = (defIndex + 1) % defs.Length;
    if(!isValid()) {
      defIndex = oldIndex;
    }
    updateModelWith('r');
  }

  public void move(int xDiff, int yDiff) {
    // Debug.Log(string.Format("moving ({0}, {1})", xDiff, yDiff));
    updateModelWith(' ');

    this.x += xDiff;
    this.y += yDiff;

    if(!isValid()) {
      this.x -= xDiff;
      this.y -= yDiff;
    }
    updateModelWith('r');
  }

  private bool isValid() {
    for(int i=0; i<defs[defIndex].Length; i++) {
      if(this.blocks[
        this.y + defs[defIndex][i].y, 
        this.x + defs[defIndex][i].x
      ].state != ' ') return false;
    }
    return true;
  }

  private static Position[] createDefinition(string[] def) {
    Position pivot = Position.ZERO;

    for(int r = 0; r<def.Length; r++) {
      for(int c = 0; c<def[r].Length; c++) {
        if(def[r][c] == 'x') {
          pivot = new Position(c, r);
          goto PIVOT_FOUND;
        }
      }
    }

PIVOT_FOUND:

    var positions = new List<Position>();
    positions.Add(Position.ZERO);

    for(int r = 0; r<def.Length; r++) {
      for(int c = 0; c<def[r].Length; c++) {
        if(def[r][c] == 'o') {
          positions.Add(new Position(c - pivot.x, r - pivot.y));
        }
      }
    }

    return positions.ToArray();
  }

  public void updateModel() {
    updateModelWith('r');
  }

  private void updateModelWith(char state) {
    for(int i=0; i<defs[defIndex].Length; i++) {
      this.blocks[
        this.y + defs[defIndex][i].y, 
        this.x + defs[defIndex][i].x
      ].state = state;
    }
  }

}
