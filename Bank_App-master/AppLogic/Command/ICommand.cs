﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    interface ICommand
    {
        Task ExecuteAsync();
    }
}
