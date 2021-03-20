import React, { useState } from 'react';

function Counter() {
    const [count, setCount] = useState<number>(0); //상태가 null일 수도 있고 아닐수도 있을때 Generics 를 활용하시면 좋습니다.
    const onIncrease = () => setCount(count + 1);
    const onDecrease = () => setCount(count - 1);
    return (
        <div>
            <h1>{count}</h1>
            <div>
                <button onClick={onIncrease}>+1</button>
                <button onClick={onDecrease}>-1</button>
            </div>
        </div>
    );
}

export default Counter;

//https://react.vlpt.us/using-typescript/03-ts-manage-state.html 여기까지 함.