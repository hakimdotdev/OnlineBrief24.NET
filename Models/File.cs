using System.ComponentModel.DataAnnotations;

namespace OnlineBrief24.Models
{
    public class File
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// z.B. 1011000000000-beliebigername#Buchhaltung#.pdf
        /// </summary>
        public string FileName { get; set; }
        [MaxLength(18)]
        public virtual Parameters Parameters { get; set; }
        public Dispatches Dispatch { get; set; }
        public byte[] ?Document { get; set; }
        
    }
}
