namespace YourVitebskWebServiceApp.APIModels
{
    public class ResponseModel
    {
        public string ErrorMessage { get; set; }
        public object Content { get; set; }

        public static ResponseModel CreateResponseWithError(string errorMessage)
        {
            return new ResponseModel
            {
                ErrorMessage = errorMessage,
                Content = null
            };
        }

        public static ResponseModel CreateResponseWithContent(object content)
        {
            return new ResponseModel
            {
                ErrorMessage = null,
                Content = content
            };
        }
    }
}
