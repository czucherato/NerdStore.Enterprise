namespace NerdStore.Enterprise.Pagamento.NerdsPag
{
    public class NerdsPagService
    {
        public NerdsPagService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }

        public readonly string ApiKey;

        public readonly string EncryptionKey;
    }
}