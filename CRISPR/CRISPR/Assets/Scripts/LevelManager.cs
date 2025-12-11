using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // Cells left
    [SerializeField] int remainingCells = 0;
    HashSet<Cell> tracked = new HashSet<Cell>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterCell(Cell c)
    {
        if (c == null) return;

        if (tracked.Add(c))
        {
            remainingCells = tracked.Count;
        }
    }

    public void UnregisterCell(Cell c)
    {
        if (c == null) return;

        if (tracked.Remove(c))
        {
            remainingCells = tracked.Count;

            // Win when all cells are dead
            if (remainingCells <= 0)
            {
                if (LevelCompletedUI.Instance != null)
                {
                    LevelCompletedUI.Instance.Show();
                }
            }
        }
    }

    public int GetRemainingCellCount()
    {
        return remainingCells;
    }
}