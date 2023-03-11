using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisjSets
{
    private int[] sets;

    public DisjSets(int numElements)
    {
        sets = new int[numElements];
        for (int i = 0; i < sets.Length; i++)
            sets[i] = -1;
    }

    
    public void Union(int set1_root, int set2_root)
    {
        if (sets[set2_root] < sets[set1_root])  // set 2's root is deeper
            sets[set1_root] = set2_root;        // make set 2's root the new root
        else
        {
            if (sets[set1_root] == sets[set2_root])
                sets[set1_root]--;
            sets[set2_root] = set1_root;        // make set 1's root the new root
        }
    }


    public int Find(int x)
    {
        if (sets[x] < 0)
            return x;
        else
            return sets[x] = Find(sets[x]);
    }
}
