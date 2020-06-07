using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace NerdStore.Enterprise.WebApp.MVC.Extensions
{
    public class CpfValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        public CpfValidationAttributeAdapterProvider()
        {
            _baseProvider = new ValidationAttributeAdapterProvider();
        }

        private readonly IValidationAttributeAdapterProvider _baseProvider;

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is CpfAttribute CpfAttribute)
            {
                return new CpfAttributeAdapter(CpfAttribute, stringLocalizer);
            }

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}