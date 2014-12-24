using Microsoft.WindowsAzure.Mobile.Service;

namespace alphatodoService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }

        public bool Complete { get; set; }
    }
}