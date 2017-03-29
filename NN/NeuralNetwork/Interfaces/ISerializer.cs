﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Interfaces
{
    internal interface ISerializer
    {
        void Serialize(INetwork network, string fileName);

        INetwork Deserialize(string fileName);
    }
}
