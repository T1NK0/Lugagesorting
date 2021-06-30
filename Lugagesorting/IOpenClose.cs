using System;
using System.Collections.Generic;
using System.Text;

namespace Lugagesorting
{
    /// <summary>
    /// Creates an interface (isOpen) since we need it on both the gate and the counter.
    /// </summary>
    public interface IOpenClose
    {
        bool IsOpen { get; set; }
    }
}
