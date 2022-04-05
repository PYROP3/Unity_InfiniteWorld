using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
    public GameObject _tile;
    public GameObject _player;

    public bool _ignoreX;
    public bool _ignoreY;

    private CircularBuffer<CircularBuffer<GameObject>> _tileMap;
    private int _width;
    private int _height;

    private float vertExtent, horzExtent;
    private float bboxMinX, bboxMinY, bboxMaxX, bboxMaxY;

    private float tileWidth, tileHeight;
    private float halfTileW, halfTileH;
    private float moveX, moveY;

    // Start is called before the first frame update
    void Start()
    {
        int i, j;

        vertExtent = Camera.main.GetComponent<Camera>().orthographicSize * 2;    
        horzExtent = vertExtent * Screen.width / Screen.height;
        Debug.Log("Camera = [" + horzExtent + ", " + vertExtent + "]");

        tileWidth = _tile.GetComponent<Renderer>().bounds.size.x;
        tileHeight = _tile.GetComponent<Renderer>().bounds.size.y;
        halfTileW = (float) (tileWidth / 2.0);
        halfTileH = (float) (tileHeight / 2.0);

        _width = (int) Math.Ceiling(2 + (horzExtent / tileWidth));
        _height = (int) Math.Ceiling(2 + (vertExtent / tileHeight));

        _tileMap = new CircularBuffer<CircularBuffer<GameObject>>(_width);
        for (i = 0; i < _width; i++) {
            CircularBuffer<GameObject> aux = new CircularBuffer<GameObject>(_height);

            for (j = 0; j < _height; j++) {
                var newTile = GameObject.Instantiate(_tile);
                newTile.transform.position = new Vector3(i * tileWidth, j * tileHeight, 0);
                newTile.transform.parent = this.transform;
                aux.Add(newTile);
            }

            _tileMap.Add(aux);
        }
        _tileMap.Reset();

        bboxMinX = 0;
        bboxMaxX = _width * tileWidth;
        bboxMinY = 0;
        bboxMaxY = _height * tileHeight;

        Debug.Log("BBox = [" + bboxMinX + ", " + bboxMaxX + "] x [" + bboxMinY + ", " + bboxMaxY + "]");

        moveX = _width * tileWidth;
        moveY = _height * tileHeight;

        DoUpdate();
    }

    void Update() {
        DoUpdate();
    }

    // Update is called once per frame
    void DoUpdate()
    {
        if (!_ignoreX) {
            var mapX = _player.transform.position.x;
            var minX = mapX - horzExtent / 2.0;
            var maxX = mapX + horzExtent / 2.0;

            Debug.Log("Update -> minX = " + minX + ", maxX = " + maxX);
            Debug.Log("BBox = [" + bboxMinX + ", " + bboxMaxX + "]");

            if (minX <= bboxMinX + halfTileW) {
                do {
                    Debug.Log("Scroll X <-");
                    ScrollXLeft();
                } while (minX <= bboxMinX + halfTileW);
            } else if (maxX >= bboxMaxX - halfTileW) {
                do {
                    Debug.Log("Scroll X ->");
                    ScrollXRight();
                } while (maxX >= bboxMaxX - halfTileW);
            } else {
                Debug.Log("No scroll X");
            }
        }

        if (!_ignoreY) {
            var mapY = _player.transform.position.y;
            var minY = mapY - vertExtent / 2.0;
            var maxY = mapY + vertExtent / 2.0;

            Debug.Log("Update -> minY = " + minY + ", maxY = " + maxY);
            Debug.Log("BBox = [" + bboxMinY + ", " + bboxMaxY + "]");

            if (minY <= bboxMinY + halfTileH) {
                do {
                    Debug.Log("Scroll Y V");
                    ScrollYDown();
                } while (minY <= bboxMinY + halfTileH);
            } else if (maxY >= bboxMaxY - halfTileH) {
                do {
                    Debug.Log("Scroll Y ^");
                    ScrollYUp();
                } while (maxY >= bboxMaxY - halfTileH);
            } else {
                Debug.Log("No scroll Y");
            }
        }
    }

    void ScrollXLeft() {
        var aux = _tileMap.RotateCCW();
        for (int j = 0; j < _height; j++) {
            var moved = aux.Get(j);
            moved.transform.position -= new Vector3(moveX, 0);
        }
        bboxMinX -= tileWidth;
        bboxMaxX -= tileWidth;
    }

    void ScrollXRight() {
        var aux = _tileMap.RotateCW();
        for (int j = 0; j < _height; j++) {
            var moved = aux.Get(j);
            moved.transform.position += new Vector3(moveX, 0);
        }
        bboxMinX += tileWidth;
        bboxMaxX += tileWidth;
    }

    void ScrollYUp() {
        for (int i = 0; i < _width; i++) {
            var moved = _tileMap.Get(i).RotateCW();
            moved.transform.position += new Vector3(0, moveY);
        }
        bboxMinY += tileHeight;
        bboxMaxY += tileHeight;
    }

    void ScrollYDown() {
        for (int i = 0; i < _width; i++) {
            var moved = _tileMap.Get(i).RotateCCW();
            moved.transform.position -= new Vector3(0, moveY);
        }
        bboxMinY -= tileHeight;
        bboxMaxY -= tileHeight;
    }
}
