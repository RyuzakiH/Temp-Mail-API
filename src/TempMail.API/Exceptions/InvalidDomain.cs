using System;
using System.Collections.Generic;
using System.Text;

namespace TempMail.API.Exceptions
{
    [Serializable]
    public class InvalidDomainException : Exception
    {
        public InvalidDomainException()
        {

        }

        public InvalidDomainException(string name)
            : base(string.Format("The domain you entered isn't an available domain: {0}", name))
        {

        }
    }
}
