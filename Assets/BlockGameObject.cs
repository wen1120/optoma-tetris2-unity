using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGameObject : MonoBehaviour {
  public Block block;

  public static Color red = new Color32(255, 0, 102, 255);
  public static Color blue = new Color32(0, 204, 255, 255);
  public static Color yellow = new Color32(255, 204, 102, 255);
  public static Color magenta = new Color32(204, 153, 255, 255);
  public static Color gray = new Color32(153, 204, 255, 255);
  public static Color lime = new Color32(153, 255, 51, 255);
  public static Color cyan = new Color32(102, 255, 204, 255);
  public static Color brown = new Color32(255, 153, 0, 255);

  private float seed;

  public void updateFromModel() {
    gameObject.SetActive(true);
    switch(block.state) {
      case 'r':
        setColor(red);
        break;
      case 'm':
        setColor(magenta);
        break;
      case 'y':
        setColor(yellow);
        break;
      case 'c':
        setColor(cyan);
        break;
      case 'b':
        setColor(blue);
        break;
      case 's':
        setColor(gray);
        break;
      case 'l':
        setColor(lime);
        break;
      case 'w':
        setColor(brown);
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
    transform.GetChild(0).GetComponent<Renderer>().material.color = color;
  }

  public void Start() {
    seed = Random.Range(0, 100f);
  }

  public void Update() {
    transform.eulerAngles = new Vector3(
      10f + (Mathf.PerlinNoise(Time.time + seed, 0f)-0.5f) * 20,
      transform.eulerAngles.y,
      transform.eulerAngles.z
    );
    
  }
}
