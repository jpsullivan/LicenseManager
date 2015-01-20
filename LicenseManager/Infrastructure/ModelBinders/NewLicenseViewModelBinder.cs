using System;
using System.Web.Mvc;

namespace LicenseManager.Infrastructure.ModelBinders
{
    public class NewLicenseViewModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var stepTypeValue = bindingContext.ValueProvider.GetValue("StepType");
            var stepType = Type.GetType((string)stepTypeValue.ConvertTo(typeof(string)), true);
            var step = Activator.CreateInstance(stepType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => step, stepType);
            return step;
        }
    }
}