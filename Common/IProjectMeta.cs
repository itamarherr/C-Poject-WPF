﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Common
{
    public interface IProjectMeta
    { public string Name { get; }
   
        public BitmapImage Icon => new BitmapImage();

        public void Run()
        {
            
        }


    }
}
