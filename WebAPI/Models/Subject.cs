using System;

namespace WebAPI.Models
{
    public class Subject
    {
        public int SubjectCode { get; set; }
        
        public string SubjectName{ get; set; }
        
        public string SubjectDescription{ get; set; }
        
        public string SubjectInitHour { get; set; }
        
        public string SubjectFinishHour { get; set; }
    }
}