namespace XTestTask.DTO.DChat
{
    public class ConnectedUser
    {
        public int AccountId { get; set; }
        public int CurrentChatId { get; set; } = 0;

        public ConnectedUser(int accountId, int chatId)
        {
            AccountId = accountId;
            CurrentChatId = chatId;
        }

        public ConnectedUser(int accountId)
        {
            AccountId = accountId;
        }
    }
}