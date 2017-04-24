using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagePool.Models
{
    /// <summary>
    /// Class for show error values
    /// </summary>
    public class ResponseObj
    {
        /// <summary>
        /// get and set Status
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// get and set ErrorCode
        /// </summary>
        public int ResponseCode { get; set; }
        /// <summary>
        /// get and set ErrorMessage
        /// </summary>
        public string ResponseMessage { get; set; }

        public ResponseObj()
        {

        }
        /// <summary>
        /// Parameterized constructor defined for defining the error object.
        /// </summary>
        /// <param name="prmStatus"></param>
        /// <param name="prmErrorCode"></param>
        /// <param name="prmErrorMessage"></param>
        public ResponseObj(int prmStatus, int prmErrorCode, string prmErrorMessage)
        {
            Status = prmStatus;
            ResponseCode = prmErrorCode;
            ResponseMessage = prmErrorMessage;
        }
    }

    /// <summary>
    /// Content Type enum
    /// </summary>
    public class ConstantAndEnum
    {
        /// <summary>
        /// Enum defined for different error codes.
        /// </summary>
        public enum ErrorCode
        {
            General = 1,      // Other exception on controller
            Exception = 2,    // Catch exception
            CriticalError = 3 // Critical Error 
        }
        /// <summary>
        /// Enum defined for different status.
        /// </summary>
        public enum Status
        {
            Ok = 1,    //Result is ok
            Error = 0  // any error occured
        }
    }
}