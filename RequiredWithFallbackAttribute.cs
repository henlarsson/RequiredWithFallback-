using System;
using System.ComponentModel.DataAnnotations;

namespace Mvc
{
    public class RequiredWithFallbackAttribute : RequiredAttribute
    {
        private string FallbackPropertyName { get; set; }
        
        public RequiredWithFallbackAttribute(string fallbackpropertyname)
        {
            if (string.IsNullOrWhiteSpace(fallbackpropertyname))
            {
                throw new ArgumentNullException("FallbackPropertyName is missing.");
            }            

            this.FallbackPropertyName = fallbackpropertyname;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var isBaseRequiredValid = base.IsValid(value, context);

            if (isBaseRequiredValid == null)
            {
                return isBaseRequiredValid;
            }

            //object instance = context?.ObjectInstance;
            //var fallbackProperty = instance?.GetType()?.GetProperty(this.FallbackPropertyName);
            //if (fallbackProperty == null)
            //{
            //    throw new System.ArgumentException($"PropertyName can not be found in ValidationContext: {this.FallbackPropertyName}.");
            //}
            //var fallbackPropertyValue = fallbackProperty.GetValue(instance, null);

            var fallbackAttributeValue = GetAttributeValue(context);

            return base.IsValid(fallbackAttributeValue, context);
        }

        private object GetAttributeValue(ValidationContext context)
        {
            object instance = context?.ObjectInstance;

            var type = instance?.GetType();

            if (type == null)
            {
                throw new System.ArgumentException($"Type can not be found in ValidationContext.");
            }

            var fallbackProperty = type.GetProperty(this.FallbackPropertyName);
            if (fallbackProperty != null)
            {
                return fallbackProperty.GetValue(instance, null);
            }
            var fallbackField = type.GetField(this.FallbackPropertyName);
            if (fallbackField != null)
            {
                return fallbackField.GetValue(instance);
            }

            //Can't find how to find parameters so can not implement that.

            throw new System.ArgumentException($"PropertyName can not be found in ValidationContext: {this.FallbackPropertyName}.");
        }
    }
}
