namespace ConferenceAPI.Models
{
    public enum OperationStatus { Success, Failure }

    public class OperationResult<T>
    {
        public T Data { get; private set; }

        public OperationStatus Status { get; private set; }

        public string Message { get; private set; }

        public bool IsSuccess
        {
            get
            {
                return this.Status == OperationStatus.Success;
            }
        }

        public OperationResult(OperationStatus status, string message, T data)
        {
            this.Data = data;
            this.Status = status;
            this.Message = message;
        }

        public static implicit operator bool(OperationResult<T> result)
        {
            return result.IsSuccess;
        }
    }
}