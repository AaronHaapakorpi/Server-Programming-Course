using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

        public NotFoundException(string name)
    :base (String.Format("{0} not found.", name))
    {
    }
}
