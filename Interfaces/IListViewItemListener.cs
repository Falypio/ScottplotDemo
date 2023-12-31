﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismAppDemo.Interfaces
{
    public interface IListViewItemListener
    {
        void OnSelectChanged(int key);
        void OnAdded(int key, string name);

        void OnRemoved(int key);

        void OnCleared();

        void OnVisibilityChanged(bool visbility, int key);
    }
}
