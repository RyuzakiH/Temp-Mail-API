using System;

namespace TempMail.API.Exceptions
{
    [Serializable]
    public class InvalidDomainException : Exception
    {
        public string Domain { get; set; }

        public InvalidDomainException()
        {

        }

        public InvalidDomainException(string domain)
            : base($"The domain you entered isn't an available domain: {domain}")
        {
            Domain = domain;
        }
    }
}
