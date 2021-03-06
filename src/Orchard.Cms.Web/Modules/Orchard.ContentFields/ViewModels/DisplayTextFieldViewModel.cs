﻿using Orchard.ContentFields.Fields;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Metadata.Models;
using Orchard.DisplayManagement.Shapes;

namespace Orchard.ContentFields.ViewModels
{
    public class DisplayTextFieldViewModel
    {
        public TextField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }
    }
}
