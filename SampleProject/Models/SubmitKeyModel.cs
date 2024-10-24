namespace SampleProject.Models
{
    public class SubmitKeyModel
    {

        public string OriginEmail { get; set; } 

        public string EmailPassword { get; set; }   =string.Empty;
        public string SourceEmail { get; set; }

        public string Client { get; set; }
    }

}
