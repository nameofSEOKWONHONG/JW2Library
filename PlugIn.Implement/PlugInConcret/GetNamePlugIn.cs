﻿using System;
using PlugInAbstract;

namespace PlugInConcret {
    public class GetNamePlugIn : IPlugIn {
        public object Run(object request) {
            var name = Convert.ToString(request);
            return "Hello " + name;
        }
    }
}