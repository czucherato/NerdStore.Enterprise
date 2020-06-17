using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Enterprise.Core.Communication
{
    public class ResponseResult
    {
        public ResponseResult()
        {
            Errors = new ResponseErrorMessages();
        }
        public string Title { get; set; }

        public int Status { get; set; }

        public ResponseErrorMessages Errors { get; set; }
    }
}
