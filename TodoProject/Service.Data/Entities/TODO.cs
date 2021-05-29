using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using JWLibrary.Entity;

namespace TodoService.Data {
    [Table("TODO")]
    public class TODO : ENTITY_BASE {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string TODO_TEXT { get; set; }
        
        public class Validator : EntityBaseValidator<TODO> {
            public Validator() {
                RuleFor(m => m.ID).GreaterThan(0);
                RuleFor(m => m.TODO_TEXT).NotNull();
            }
        }
    }
}