﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Inventories;

namespace UISystem.Inventories
{
    /// <summary>
    /// Allows the `ItemTooltipSpawner` to display the right information.
    /// </summary>
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}