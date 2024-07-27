using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Handles to random chance.
    /// </summary>
    /// <param name="chance">The value to compare.</param>
    /// <returns>True if less than chance. False if not.</returns>
    public static bool RandomChance(float chance)
    {
        if (Random.Range(0, 100) < chance)
        {
            return true;
        }

        return false;
    }
}
