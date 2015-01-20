using System.Collections.Generic;
using System.Web.Mvc;
using LicenseManager.Infrastructure.Attributes;
using LicenseManager.Models.ViewModels;
using Microsoft.Web.Mvc;

namespace LicenseManager.Controllers
{
    public class LicenseController : AppController
    {
        protected List<INewLicenseViewModel> NewLicenseSteps { get; set; }

        public LicenseController(List<INewLicenseViewModel> newLicenseSteps)
        {
            NewLicenseSteps = newLicenseSteps;
        }

        [IntraRoute("license/new", Name = "License-New")]
        public ActionResult New()
        {
            var licenseWizard = new NewLicenseViewModel(NewLicenseSteps);
            return View(licenseWizard);
        }

        [IntraRoute("license/create", HttpVerbs.Post, Name = "License-NextStep-Post")]
        public ActionResult NextStep([Deserialize] NewLicenseViewModel licenseWizard, INewLicenseViewModel step)
        {
            licenseWizard.Steps[licenseWizard.CurrentStepIndex] = step;
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Request["next"]))
                {
                    licenseWizard.CurrentStepIndex++;
                }
                else if (!string.IsNullOrEmpty(Request["prev"]))
                {
                    licenseWizard.CurrentStepIndex--;
                }
                else
                {
                    // TODO: we have finished: all the step partial
                    // view models have passed validation => map them
                    // back to the domain model and do some processing with
                    // the results

                    return Content("thanks for filling this form", "text/plain");
                }
            }
            else if (!string.IsNullOrEmpty(Request["prev"]))
            {
                // Even if validation failed we allow the user to navigate to previous steps
                licenseWizard.CurrentStepIndex--;
            }

            return View("New", licenseWizard);
        }
    }
}