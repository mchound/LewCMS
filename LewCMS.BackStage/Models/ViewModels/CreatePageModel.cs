using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LewCMS.BackStage.Models.ViewModels
{
    public class CreateContentModel
    {
        [Required(ErrorMessage = "Content Type Id is required")]
        public string ContentTypeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }

    public class CreatePageModel : CreateContentModel
    {
        public string ParentId { get; set; }
    }
}