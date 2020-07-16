namespace NerdStore.Enterprise.Pagamento.NerdsPag
{
    public enum TransactionStatus
    {
        Authorized = 1,
        Paid,
        Refused,
        Chargedback
    }
}