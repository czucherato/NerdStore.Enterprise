using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace NerdStore.Enterprise.Pagamento.NerdsPag
{
    public class CardHash
    {
        private readonly NerdsPagService _nerdsPagService;

        public CardHash(NerdsPagService nerdsPagService)
        {
            _nerdsPagService = nerdsPagService;
        }

        public string CardHolderName { get; set; }

        public string CardNumber { get; set; }

        public string CardExpirationDate { get; set; }

        public string CardCvv { get; set; }

        public string Generate()
        {
            using var aesAlg = Aes.Create();

            aesAlg.IV = Encoding.Default.GetBytes(_nerdsPagService.EncryptionKey);
            aesAlg.Key = Encoding.Default.GetBytes(_nerdsPagService.ApiKey);

            var encryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            throw new NotImplementedException();
        }
    }
}