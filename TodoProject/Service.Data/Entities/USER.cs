using System;
using FluentValidation;

namespace TodoService.Data {
    public class USER {
        public string USER_ID { get; set; }
        public string USER_NM { get; set; } 
        public string NICK_NM { get; set; }
        public string PASSWORD { get; set; }
        public DateTime REG_DT { get; set; }
        
        public bool IS_EXFIRED { get; set; }
        
        public class UserValidate : AbstractValidator<USER> {
            public UserValidate() {
                RuleFor(m => m.USER_ID).NotEmpty();
                RuleFor(m => m.PASSWORD).NotEmpty();
                RuleFor(m => m.USER_NM).NotEmpty();
                RuleFor(m => m.NICK_NM).NotEmpty();
                RuleFor(m => m.REG_DT).NotEmpty();
            }
        }
    }
}