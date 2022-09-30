using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ReturnModels
{
    public class ReturnControl
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public object Data { get; set; }
    }

    public class Reply
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string trace { get; set; }
    }

    public class ReplySucess
    {
        public bool Ok { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; } 
        public string json { get; set; }
    }
}
