using System.Text.RegularExpressions;

namespace NerdStore.Enterprise.Core.DomainObjects
{
    public class Email
    {
        protected Email() { }

        public Email(string endereco)
        {
            if (!Validar(endereco)) throw new DomainException("E-mail inválido");
            Endereco = endereco;
        }

        public const int EnderecoMaxLength = 100;
        public string Endereco { get; private set; }

        public static bool Validar(string email)
        {
            string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
            return Regex.IsMatch(email, pattern);
        }
    }
}