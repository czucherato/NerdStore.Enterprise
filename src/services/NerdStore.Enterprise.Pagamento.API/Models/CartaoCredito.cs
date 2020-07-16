namespace NerdStore.Enterprise.Pagamento.API.Models
{
    public class CartaoCredito
    {
        public CartaoCredito(string nomeCartao, string numeroCartao, string mesAnoVencimento, string cVV)
        {
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            MesAnoVencimento = mesAnoVencimento;
            CVV = cVV;
        }

        public string NomeCartao { get; set; }

        public string NumeroCartao { get; set; }

        public string MesAnoVencimento { get; set; }

        public string CVV { get; set; }
    }
}