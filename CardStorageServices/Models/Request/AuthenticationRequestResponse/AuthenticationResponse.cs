namespace CardStorageServices.Models.Request.AuthenticationRequestResponse
{
    public class AuthenticationResponse
    {
        public AuthenticationStatus Status { get; set; }

        public SessionInfo SessionInfo { get; set; }
    }
}
