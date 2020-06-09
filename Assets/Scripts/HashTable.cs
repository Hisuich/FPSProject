using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HashTable
{
    private static HashTable instance;

    private Dictionary<UInt32, string> hashTable;

    private HashTable()
    {
        hashTable = new Dictionary<UInt32, string>();
    }

    public static HashTable getInstance()
    {
        if (instance == null)
            instance = new HashTable();

        return instance;
    }
    
    public void AddId(string stringId)
    {
    }
}

