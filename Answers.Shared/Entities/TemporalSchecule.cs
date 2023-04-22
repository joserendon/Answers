using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Answers.Shared.Entities
{
    public class TemporalSchecule
    {
        public int Id { get; set; }

        public User? User { get; set; }

        public string? UserId { get; set; }

        public Questionnaire? Questionnaire { get; set; }

        public string QuestionnaireId  { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }


    }
}
