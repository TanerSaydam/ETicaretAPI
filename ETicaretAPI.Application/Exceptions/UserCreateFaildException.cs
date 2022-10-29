using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class UserCreateFaildException : Exception
    {
        public UserCreateFaildException()
        {
        }

        public UserCreateFaildException(string message) : base(message)
        {
        }

        public UserCreateFaildException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
