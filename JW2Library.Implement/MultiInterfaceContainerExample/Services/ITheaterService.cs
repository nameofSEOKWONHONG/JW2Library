using System;

namespace MultiInterfaceContainerExample.Services {
    /*
     * 시나리오
     * 주문자 정보 확인 -> 극장별 예매시작 (my company)
     *                           -> 선점 -> 예매 -> 후속작업 (reservce company)
     *                                                -> 계산 -> 알림 (my company)
     */

    public enum TICKET_TYPE {
        CGV,
        LOTTE,
        MEGABOX
    }
    
    public class CompanyService : ICompanyService {
        public TICKET_TYPE TicketType { get; set; } = TICKET_TYPE.CGV;
        
        public bool CheckUserInfo(string name) {
            return true;
        }

        public bool CheckOut(string name) {
            return true;
        }

        public void Notify(string name) {

        }

        public void Cancel(string name) {
            
        }
    }
    /// <summary>
    ///     기본 B2C 서비스 인터페이스
    /// </summary>
    public interface ICompanyService {
        TICKET_TYPE TicketType { get; set; }
        /// <summary>
        ///     주문자 정보 확인
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CheckUserInfo(string name);

        /// <summary>
        ///     계산
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CheckOut(string name);

        /// <summary>
        ///     알림
        /// </summary>
        /// <param name="name"></param>
        void Notify(string name);

        void Cancel(string name);
    }

    /// <summary>
    ///     극장별 기본 인터페이스
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITheaterService<T> where T : class {
        /// <summary>
        ///     선점
        /// </summary>
        /// <param name="name"></param>
        void PreAction(string name);

        /// <summary>
        ///     예약
        /// </summary>
        /// <param name="name"></param>
        void DoAction(string name);

        /// <summary>
        ///     후속 작업
        /// </summary>
        /// <param name="name"></param>
        void PostAction(string name);

        void Cancel(string name);
    }

    /// <summary>
    ///     CGV 극장 인터페이스 구현
    /// </summary>
    public class CgvService : ITheaterService<CgvService> {
        public void PreAction(string name) {
            Console.WriteLine($"pre reservce : {name}");
        }

        public void DoAction(string name) {
            Console.WriteLine($"reserve : {name}");
        }

        public void PostAction(string name) {
            Console.WriteLine($"post reserve : {name}");
        }

        public void Cancel(string name) {
            Console.WriteLine($"cancel : {name}");
        }
    }

    /// <summary>
    ///     롯데시네마 극장 인터페이스 구현
    /// </summary>
    public class LotteService : ITheaterService<LotteService> {
        public void PreAction(string name) {
            Console.WriteLine($"pre reservce : {name}");
        }

        public void DoAction(string name) {
            Console.WriteLine($"reserve : {name}");
        }

        public void PostAction(string name) {
            Console.WriteLine($"post reserve : {name}");
        }

        public void Cancel(string name) {
            Console.WriteLine($"cancel : {name}");
        }
    }
}