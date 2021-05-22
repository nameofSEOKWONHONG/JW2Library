using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace TodoWebApi.Entities {
    [Table("TODO")]
    public class TODO {
        [Key]
        public int ID { get; set; }
        public string TODO_TEXT { get; set; }
        public DateTime W_DATE { get; set; }
        public DateTime M_DATE { get; set; }
        
        public class Validator : AbstractValidator<TODO> {
            public Validator() {
                RuleFor(m => m.ID).GreaterThan(0);
                RuleFor(m => m.TODO_TEXT).NotNull();
                RuleFor(m => m.W_DATE).NotNull();
                RuleFor(m => m.M_DATE).NotNull();
            }
        }
    }
}