namespace CommandWebService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}