using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotVSBot : MonoBehaviour
{
    public GameObject _lineBotA;
    public GameObject _lineBotB;
    private List<Vector3> _PossiblePositions = new();
    [SerializeField] private int _height = 7;
    [SerializeField] private int _width = 15;
    [SerializeField] private float _delay = 1f;
    private Vector3 _variant;
    private int _xpos;
    private int _ypos;
    private bool _rotateLineBotA = false;
    private bool _rotateLineBotB = false;

    private void Start()
    {
        Make_Variants();
        BotGame();
    }


    private void Make_Variants()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height - 1; y++)
            {
                _variant = new Vector3(x - (int)(_width / 2), (y + 0.5f) - (int)(_height / 2), 0);
                _PossiblePositions.Add(_variant);
            }
        }
        for (int x = 0; x < _width - 1; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _variant = new Vector3((x + 0.5f) - (int)(_width / 2), y - (int)(_height / 2), 0);
                _PossiblePositions.Add(_variant);
            }
        }
    }
    private Vector3 GetRandomItem()
    {
        int randomIndex = Random.Range(0, _PossiblePositions.Count); // Генерация случайного индекса
        Vector3 randomItem = _PossiblePositions[randomIndex];
        Vector3 EnemyItem;
        if (randomItem.y % 1 == 0f)//1 бот ставит горизонтальную палку
        {
            EnemyItem = new Vector3(randomItem.x - 0.5f, randomItem.y - 0.5f, randomItem.z);
        }
        else
        {
            EnemyItem = new Vector3(randomItem.x + 0.5f, randomItem.y + 0.5f, randomItem.z);
        }

        _PossiblePositions.Remove(randomItem);
        if (_PossiblePositions.Contains(EnemyItem))
        {
            _PossiblePositions.Remove(EnemyItem);
        }
        return randomItem;
    }

    private void BotGame()
    {
        if (_PossiblePositions.Count < 3)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif

            Application.Quit();

            Debug.Log("Exit");
        }
        TurnBotA();
        StartCoroutine(TurnTimerRoutine(_variant));
    }
    private void TurnBotA()
    {
        _variant = GetRandomItem();
        if (_variant.y % 1 == 0f)//1 бот ставит горизонтальную палку
        {
            _rotateLineBotA = true;
        }
        else
        {
            _rotateLineBotA = false;
        }

        if (_rotateLineBotA)
        {
            Instantiate(_lineBotA, _variant, Quaternion.Euler(0, 0, 90));
        }
        else
        {
            Instantiate(_lineBotA, _variant, Quaternion.identity);
        }
    }
    private void TurnBotB(Vector3 variantBotA)
    {
        Vector3 _variantBotB;
        if (variantBotA.y % 1 == 0f)//1 бот ставит горизонтальную палку
        {
            _rotateLineBotB = false;
            _variantBotB = new Vector3(variantBotA.x - 0.5f, variantBotA.y - 0.5f, variantBotA.z);
        }
        else
        {
            _rotateLineBotB = true;
            _variantBotB = new Vector3(variantBotA.x + 0.5f, variantBotA.y + 0.5f, variantBotA.z);
        }

        if (_rotateLineBotB)
        {
            Instantiate(_lineBotB, _variantBotB, Quaternion.Euler(0, 0, 90));
        }
        else
        {
            Instantiate(_lineBotB, _variantBotB, Quaternion.identity);
        }
    }

    private IEnumerator TurnTimerRoutine(Vector3 variantBotA)
    {
        yield return new WaitForSeconds(_delay);
        TurnBotB(variantBotA);
        yield return new WaitForSeconds(_delay);
        BotGame();
    }
}
