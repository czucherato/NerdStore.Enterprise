using System.Collections.Generic;

namespace NerdStore.Enterprise.Core.Communication
{
    public class ResponseErrorMessages
    {
        public ResponseErrorMessages()
        {
            Mensagens = new List<string>();
        }
        public IList<string> Mensagens { get; set; }
    }
}