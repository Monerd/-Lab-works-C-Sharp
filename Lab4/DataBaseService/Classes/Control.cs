namespace DataBaseService
{
    public class Control
    {
        public bool CanStop { get; set; }
        public bool CanPauseAndContinue { get; set; }
        public bool AutoLog { get; set; }
        public string SourceDirectory { get; set; }
        public Control()
        {
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
            SourceDirectory = "D:\\";
        }
    }
}
