namespace MultiInterfaceContainerExample.Services {
    /*
     * best practice interface multiple implement
     */
    public interface IBaseService<T> where T : class {
        string SayHello(string str);
    }

    public class Hello1Service : IBaseService<Hello1Service> {
        public string SayHello(string str) {
            return $"I'am hello1 service: {str}";
        }
    }

    public class Hello2Service : IBaseService<Hello2Service> {
        public string SayHello(string str) {
            return $"I'am hello2 service: {str}";
        }
    }
    
    
}