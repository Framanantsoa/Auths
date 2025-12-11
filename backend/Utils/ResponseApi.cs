namespace Utils;

public class ResponseApi
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public object Errors { get; set; }

    // Constructeurs pratiques
    public ResponseApi(
        bool success, string message, object data = default, object errors = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors;
    }

    // Méthodes statiques utilitaires
    public static ResponseApi Ok(string message, object data = default) =>
        new ResponseApi(true, message, data);

    public static ResponseApi Fail(string message, object errors = null) =>
        new ResponseApi(false, message, default, errors);
}
