using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private int _numberOfElement = 5;
    [SerializeField] private string _fileName = "note.dat";

    private ListRand _list = new ListRand();

    private string _path = string.Empty;

    void Start()
    {
        _path = Path.Combine(Application.dataPath, _fileName);

        InitializeList();

        using (FileStream fstream = new FileStream(_path, FileMode.Create))
        {
            _list.Serialize(fstream);
        }

        _list.Clear();

        using (FileStream fstream = File.OpenRead(_path))
        {
            _list.Deserialize(fstream);
        }
    }

    private void InitializeList()
    {
        _list = new ListRand();

        int value = 0;
        for (int i = 0; i < _numberOfElement; i++)
            _list.Add(new ListNode(Convert.ToString(++value)));

        var secondNode = _list.GetByIndex(1);
            secondNode.Rand = secondNode;

        var thirdNode = _list.GetByIndex(2);
            thirdNode.Rand = _list.GetLast();

        var fourthNode = _list.GetByIndex(3);
            fourthNode.Rand = _list.GetFirst();
    }
}
