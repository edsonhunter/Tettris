using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tettris.Controller.Shape;
using Tettris.Domain.Interface.Board;
using Tettris.Services.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayScene : MonoBehaviour
{
    [Header("")]
    [SerializeField]
    private List<TetrominoController> TetrominoPrefabs = null;

    [SerializeField]
    private TetrominoController _currentTetromino = null;

    [SerializeField]
    private GameObject _startPosition = null;

    private IGameService GameService { get; set; }

    private void Awake()
    {
        GameService = new GameService();
    }

    private void Start()
    {
        StartNewTetromino();
        StartCoroutine(Turno());
    }

    private void StartNewTetromino()
    {
        _currentTetromino = null;
        var tetromino = GameService.StartNewTetromino();
        _currentTetromino = Instantiate(TetrominoPrefabs[Random.Range(0, TetrominoPrefabs.Count)], _startPosition.transform.position, Quaternion.identity);
        _currentTetromino.Init(tetromino);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameService.Move(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameService.Move(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameService.Move(Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GameService.Rotate(Quaternion.Euler(0, 0, 90f));
        }
    }

    private IEnumerator Turno()
    {
        while (GameService.Running)
        {
            yield return new WaitForSeconds(0.1f);
            
            if (!GameService.NextTurno())
            {
                _currentTetromino.End();
                StartNewTetromino();
            }
        }
    }
}