﻿using System;
using NerdStore.Enterprise.Core.DomainObjects;

namespace NerdStore.Enterprise.Cliente.API.Models
{
    public class Cliente : Entity, IAggregateRoot
    {
        protected Cliente() { }

        public Cliente(Guid id, string nome, string email, string cpf)
        {
            Id = id;
            Nome = nome;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
        }

        public string Nome { get; private set; }

        public Email Email { get; private set; }

        public Cpf Cpf { get; private set; }

        public bool Excluido { get; private set; }

        // EF Relation

        public Endereco Endereco { get; private set; }

        public void TrocarEmail(string email)
        {
            Email = new Email(email);
        }

        public void AtribuirEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }
    }
}