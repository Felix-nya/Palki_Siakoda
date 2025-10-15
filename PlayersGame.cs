using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnLine : MonoBehaviour
{
    public GameObject _spawnObject;
    public GameObject _EnemyObject;
    private float _xpos;
    private float _ypos;
    private Vector3 _positionMouse;
    private Vector3 _positionEnemy;
    private Vector3 _positionEnemyMouse;
    private bool _rotateLine = false;
    private bool _rotateEnemy = false;
    [SerializeField] private float _delayEnemy = 1f;
    private bool _Turn = true;
    private int[] _DSU;
    private int _id1;
    private int _id2;
    [SerializeField] private int height = 7;
    [SerializeField] private int width = 15;
    private int size;
    private Scene currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        GameInput.Instance.OnPlayerClick += Player_OnPlayerClick;
        size = height * width;
        _DSU = new int[size];
        MakeDSU();
    }

    private void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        Vector3 pos = GameInput.Instance.GetMousePosition();
        _rotateLine = false;
        if ((pos.y - Mathf.RoundToInt(pos.y) >= 0) && (Mathf.Abs(pos.y - Mathf.RoundToInt(pos.y)) > Mathf.Abs(pos.x - Mathf.RoundToInt(pos.x))))
        {
            _xpos = 0f;
            _ypos = 0.5f;
        }
        else if ((pos.y - Mathf.RoundToInt(pos.y) < 0) && (Mathf.Abs(pos.y - Mathf.RoundToInt(pos.y)) > Mathf.Abs(pos.x - Mathf.RoundToInt(pos.x))))
        {
            _xpos = 0f;
            _ypos = -0.5f;
        }
        else if ((pos.x - Mathf.RoundToInt(pos.x) >= 0) && (Mathf.Abs(pos.y - Mathf.RoundToInt(pos.y)) < Mathf.Abs(pos.x - Mathf.RoundToInt(pos.x))))
        {
            _xpos = 0.5f;
            _ypos = 0f;
        }
        else
        {
            _xpos = -0.5f;
            _ypos = 0f;
        }
        if (_ypos == 0f) _rotateLine = true;
        _xpos += Mathf.RoundToInt(pos.x);
        _ypos += Mathf.RoundToInt(pos.y);
        _positionMouse = new Vector3(_xpos, _ypos, 0);
    }

    private void Player_OnPlayerClick(object sender, System.EventArgs e)
    {
        if (_Turn)
        {
            if (_rotateLine)
            {
                _id1 = (int)(_positionMouse.x - 0.5f) + (int)(width / 2) + ((int)(_positionMouse.y) + (int)(height / 2)) * width;
                _id2 = (int)(_positionMouse.x + 0.5f) + (int)(width / 2) + ((int)(_positionMouse.y) + (int)(height / 2)) * width;
            }
            else
            {
                _id1 = ((int)(_positionMouse.x) + (int)(width / 2)) + ((int)(_positionMouse.y + 0.5f) + (int)(height / 2)) * width;
                _id2 = ((int)(_positionMouse.x) + (int)(width / 2)) + ((int)(_positionMouse.y - 0.5f) + (int)(height / 2)) * width;
            }

            if (DSU_Leader(_id1) == DSU_Leader(_id2))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif

                Application.Quit();

                Debug.Log("Exit");
            }
            else
            {
                Unite_Leaders(_id1, _id2);
            }
            Turn_Player();
        }
        else if (currentScene.name == "PlayerVSPlayer") Turn_Enemy();

        if (currentScene.name == "PlayerVSBot")
        {
            StartCoroutine(TurnTimerRoutine());
        }
    }

    private IEnumerator TurnTimerRoutine()
    {
        yield return new WaitForSeconds(_delayEnemy);
        Turn_Enemy();
    }

    private void Turn_Player()
    {
        if (_rotateLine)
        {
            Instantiate(_spawnObject, _positionMouse, Quaternion.Euler(0, 0, 90));
        }
        else
        {
            Instantiate(_spawnObject, _positionMouse, Quaternion.identity);
        }
        _positionEnemyMouse = _positionMouse;
        _rotateEnemy = _rotateLine;
        _Turn = false;
    }

    private void Turn_Enemy()
    {
        if (currentScene.name == "PlayerVSBot")
        {
            if (_rotateEnemy) _positionEnemy = new Vector3(_positionEnemyMouse.x - 0.5f, _positionEnemyMouse.y - 0.5f, 0);
            else _positionEnemy = new Vector3(_positionEnemyMouse.x + 0.5f, _positionEnemyMouse.y + 0.5f, 0);

            if (_rotateEnemy) Instantiate(_EnemyObject, _positionEnemy, Quaternion.identity);
            else Instantiate(_EnemyObject, _positionEnemy, Quaternion.Euler(0, 0, 90));
        }
        else
        {
            if (_rotateLine) Instantiate(_EnemyObject, _positionMouse, Quaternion.Euler(0, 0, 90));
            else Instantiate(_EnemyObject, _positionMouse, Quaternion.identity);
        }
        _Turn = true;
    }

    private void MakeDSU()
    {
        for (int i = 0; i < size; i++)
        {
            _DSU[i] = i;
        }
    }

    private int DSU_Leader(int element)
    {
        if (_DSU[element] == element) return element;
        return _DSU[element] = DSU_Leader(_DSU[element]);
    }

    private void Unite_Leaders(int element1, int element2)
    {
        element1 = DSU_Leader(element1);
        element2 = DSU_Leader(element2);
        _DSU[element1] = _DSU[element2];
    }

}

