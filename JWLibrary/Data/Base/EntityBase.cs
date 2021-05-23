using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using FluentValidation;

namespace JWLibrary.Entity {
    public class USER_BASE : ENTITY_BASE {
        [Key]
        public string USER_ID { get; set; }
        
        
        /// <summary>
        /// 작성자
        /// </summary>
        [Required]
        public string W_USERID { get; set; }
        
        /// <summary>
        /// 수정자
        /// </summary>
        [Required]
        public string M_USERID { get; set; }
        
        public class Validator : UserEntityBaseValidator<USER_BASE> {
            public Validator() {

            }
        }
    }

    public class UserEntityBaseValidator<TEntity> : EntityBaseValidator<TEntity> 
    where TEntity : USER_BASE {
        public UserEntityBaseValidator() {
            RuleFor(m => m.USER_ID).NotNull();
            RuleFor(m => m.W_USERID).NotNull();
            RuleFor(m => m.M_USERID).NotNull();
        }
    }

    /// <summary>
    /// ENTITY 기본 구조에 꼭 들어가야 할 컬럼 정의
    /// </summary>
    public class ENTITY_BASE {
        /// <summary>
        /// 작성일
        /// </summary>
        [Required]
        public DateTime W_DATE { get; set; }
        /// <summary>
        /// 수정일
        /// </summary>
        [Required]
        public DateTime M_DATE { get; set; }
    }
    
    public class EntityBaseValidator<TValidator> : AbstractValidator<TValidator> 
        where TValidator : ENTITY_BASE {
        public EntityBaseValidator() {
            RuleFor(m => m.W_DATE).NotNull();
            RuleFor(m => m.M_DATE).NotNull();
        }
    }
}