using LewCMS.V2.Contents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Validation
{
    public class ClientValidation : Dictionary<Type, IClientValidationFactory>
    {
        private static ClientValidation _factories = new ClientValidation();

        public static ClientValidation Factories { get { return _factories; } }

        public void SetValidation(Type validationAttributeType, IClientValidationFactory factory)
        {
            if (_factories.ContainsKey(validationAttributeType))
	        {
		        _factories[validationAttributeType] = factory;
	        }
            else
            {
                _factories.Add(validationAttributeType, factory);
            }
        }
    }

    public interface IClientValidationFactory
    {
        string GetClientValidationString(ValidationAttribute validationAttribute);
    }

    public class RequiredClientValidationFactory : IClientValidationFactory
    {
        public string GetClientValidationString(ValidationAttribute validationAttribute)
        {
            return string.Format("{{required: {{errorMessage: '{0}'}} }}", validationAttribute.ErrorMessage);
        }
    }

    public class MinLengthClientValidationFactory : IClientValidationFactory
    {
        public string GetClientValidationString(ValidationAttribute validationAttribute)
        {
            return string.Format("{{minLength: {{errorMessage: '{0}', limit: {1}}} }}", validationAttribute.ErrorMessage, (validationAttribute as MinLengthAttribute).Length);
        }
    }

    public static class ClientValidationHelper
    {
        public static string GetClientValidationNotation(this IProperty property)
        {
            string validation = string.Empty;

            foreach (var validationAttribute in property.ValidationAttributes)
            {
                validation = string.Concat(validation, ClientValidation.Factories[validationAttribute.GetType()].GetClientValidationString(validationAttribute));
            }

            return string.Format("{{{0}}}", validation);
        }
    }
}
